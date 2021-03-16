using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Outpost : MonoBehaviour, IDamagable
{
    [SerializeField] List<OutpostData> outpostTiers;
    [SerializeField] HealthSlider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] MenuScript menuScript;

    private FactionEnum _faction;
    private OutpostData _outpostData;
    private int _outpostTier;

    public OutpostData Initialize(FactionEnum faction)
    {
        _faction = faction;

        Upgrade(0);

        return _outpostData;
    }

    public void TakeDamage(float damage)
    {
        healthSlider.SubtractHealth(damage);

        healthText.text = ((int)healthSlider.Health).ToString();

        if (healthSlider.Health <= 0)
        {
            if (_faction == FactionEnum.Green)
            {
                menuScript.GameOver();
            }
            else
            {
                menuScript.GameWin();
            }
        }
    }

    public int UpgradeOutpost(int balance, out OutpostData outpostData)
    {
        outpostData = null;

        if (_outpostTier >= outpostTiers.Count - 1)
            return 0;

        if (balance < outpostTiers[_outpostTier + 1].UpgradePrice)
            return 0;

        Upgrade(_outpostTier + 1);
        outpostData = _outpostData;

        return _outpostData.UpgradePrice;
    }

    private void Upgrade(int tier)
    {
        _outpostTier = tier;
        _outpostData = outpostTiers[_outpostTier];
        healthSlider.Initialize(_outpostData.Health, true);
        healthText.text = ((int)_outpostData.Health).ToString();
    }
}