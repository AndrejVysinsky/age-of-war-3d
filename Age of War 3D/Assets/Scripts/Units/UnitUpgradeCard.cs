using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeCard : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text unitName;
    [SerializeField] Image unitImage;
    [SerializeField] TMP_Text unitHitpoints;
    [SerializeField] TMP_Text unitAttackValue;
    [SerializeField] TMP_Text unitUpgradeCost;
    [SerializeField] Button unitUpgradeButton;

    [SerializeField] GameObject upgradeContainer;
    [SerializeField] GameObject maxLevelContainer;

    [Header("Misc")]
    [SerializeField] UnitTrainCard unitCard;

    private void Start()
    {
        unitUpgradeButton.onClick.AddListener(UpgradeUnit);
    }

    private void OnEnable()
    {
        ShowUnitInfo(unitCard.UnitData);
    }
    
    private void UpgradeUnit()
    {
        unitCard.PlayerController.UpgradeUnit(unitCard.UnitIndex);

        ShowUnitInfo(unitCard.UnitData);
    }

    private void ShowUnitInfo(UnitData upgradedUnitData)
    {
        unitName.text = upgradedUnitData.Name;
        unitImage.sprite = upgradedUnitData.Sprite;
        unitHitpoints.text = upgradedUnitData.HitPoints.ToString();
        unitAttackValue.text = upgradedUnitData.Damage.ToString();
        unitUpgradeCost.text = upgradedUnitData.UpgradeCost.ToString();

        if (upgradedUnitData.UpgradeCost == 0)
        {
            upgradeContainer.SetActive(false);
            maxLevelContainer.SetActive(true);
        }
        else
        {
            upgradeContainer.SetActive(true);
            maxLevelContainer.SetActive(false);
        }
    }
}
