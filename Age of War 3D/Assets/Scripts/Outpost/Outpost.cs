
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Outpost : MonoBehaviour, IDamagable
{
    [SerializeField] List<OutpostData> outpostTiers;
    [SerializeField] HealthSlider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] MenuScript menuScript;
    [SerializeField] GameObject dmgTakenText;

    private OutpostData _outpostData;
    private int _outpostTier;
    private float HEIGHT_OFFSET = 0.5f;

    public bool UnderAttack { get; private set; }
    public FactionEnum Faction { get; private set; }
    public Transform Transform { get; private set; }
    public int MaxMinerUnits => _outpostData.MaxMinerUnits;
    public OutpostData OutpostData => _outpostData;

    private void Awake()
    {
        Transform = transform;
    }

    public OutpostData Initialize(FactionEnum faction)
    {
        Faction = faction;

        Upgrade(0);

        return _outpostData;
    }

    public void TakeDamage(float damage)
    {
        healthSlider.SubtractHealth(damage);

        StartCoroutine(setUnderAttack());

        healthText.text = ((int)healthSlider.Health).ToString();

        var position = transform.position;
        position.Set(position.x, position.y + HEIGHT_OFFSET, position.z);
        var text_holder = Instantiate(dmgTakenText, position, Quaternion.Euler(Camera.main.transform.eulerAngles));
        text_holder.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + damage);

        if (healthSlider.Health <= 0)
        {
            if (Faction == FactionEnum.Player)
            {
                menuScript.GameOver();
            }
            else
            {
                menuScript.GameWin();
            }
        }
    }

    private IEnumerator setUnderAttack()
    {
        if (!UnderAttack)
        {
            UnderAttack = true;

            yield return new WaitForSeconds(5f);

            UnderAttack = false;
        }
    }

    public int UpgradeOutpost(int balance, out OutpostData outpostData)
    {
        outpostData = null;

        if (_outpostTier >= outpostTiers.Count - 1)
            return 0;

        int upgradePrice = outpostTiers[_outpostTier].UpgradePrice;

        if (balance < upgradePrice || upgradePrice == 0)
            return 0;

        Upgrade(_outpostTier + 1);
        outpostData = _outpostData;

        return upgradePrice;
    }

    private void Upgrade(int tier)
    {
        float dmg = 0;
        if (tier > 0)
        {
            dmg = outpostTiers[_outpostTier].Health - GetHealth();
        }
        _outpostTier = tier;
        _outpostData = outpostTiers[_outpostTier];
        healthSlider.Initialize(_outpostData.Health, _outpostData.Health - dmg, true);
        healthText.text = ((int)_outpostData.Health - dmg).ToString();

        EventManager.Instance.ExecuteEvent<IOutpostUpgraded>((x, y) => x.OnOutpostUpgraded(_outpostData, Faction));
    }

    public float GetHealth()
    {
        return healthSlider.Health;
    }

    public float GetUpgradePrice()
    {
        return _outpostData.UpgradePrice;
    }
}