using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Button upgradeUnitsButton;
    [SerializeField] Button upgradeOutpostButton;

    [SerializeField] GameObject upgradeUnitsUI;
    [SerializeField] GameObject upgradeOutpostUI;

    [SerializeField] Color activeButtonColor;
    [SerializeField] Color inactiveButtonColor;

    private void Start()
    {
        upgradeUnitsButton.onClick.AddListener(ShowUnitUpgrades);
        upgradeOutpostButton.onClick.AddListener(ShowOutpostUpgrades);

        upgradeUnitsButton.image.color = activeButtonColor;
        upgradeOutpostButton.image.color = inactiveButtonColor;
    }

    private void ShowOutpostUpgrades()
    {
        upgradeUnitsButton.image.color = inactiveButtonColor;
        upgradeOutpostButton.image.color = activeButtonColor;

        upgradeUnitsUI.SetActive(false);
        upgradeOutpostUI.SetActive(true);
    }

    private void ShowUnitUpgrades()
    {
        upgradeUnitsButton.image.color = activeButtonColor;
        upgradeOutpostButton.image.color = inactiveButtonColor;

        upgradeUnitsUI.SetActive(true);
        upgradeOutpostUI.SetActive(false);
    }
}
