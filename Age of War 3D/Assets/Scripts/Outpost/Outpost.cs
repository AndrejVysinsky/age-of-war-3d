using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    [SerializeField] List<OutpostData> outpostTiers;
    [SerializeField] HealthSlider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    private OutpostData _outpostData;
    private int _outpostTier;

    private void Awake()
    {
        //hmmm...
        UpgradeOutpost(int.MaxValue);
    }

    public void TakeDamage(float damage)
    {
        healthSlider.SubtractHealth(damage);

        healthText.text = ((int)healthSlider.Health).ToString();

        if (healthSlider.Health <= 0)
        {
            //TODO: defeat
        }
    }

    public int UpgradeOutpost(int balance)
    {
        if (_outpostTier >= outpostTiers.Count - 1)
            return 0;

        if (balance < outpostTiers[_outpostTier + 1].UpgradePrice)
            return 0;

        _outpostData = outpostTiers[_outpostTier++];

        healthSlider.Initialize(_outpostData.Health);

        healthText.text = ((int)_outpostData.Health).ToString();

        return _outpostData.UpgradePrice;
    }
}