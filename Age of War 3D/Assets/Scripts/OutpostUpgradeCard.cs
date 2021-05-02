using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutpostUpgradeCard : MonoBehaviour, IOutpostUpgraded
{
    [Header("Components")]
    [SerializeField] TMP_Text outpostName;
    [SerializeField] Image outpostImage;
    [SerializeField] TMP_Text outpostHitpoints;
    [SerializeField] TMP_Text outpostTrainCapacity;
    [SerializeField] TMP_Text outpostMinerCapacity;
    [SerializeField] TMP_Text outpostUpgradeCost;
    [SerializeField] Button outpostUpgradeButton;

    [SerializeField] GameObject upgradeContainer;
    [SerializeField] GameObject maxLevelContainer;

    private PlayerController _playerController;

    private void Start()
    {
        upgradeContainer.SetActive(true);
        maxLevelContainer.SetActive(false);

        outpostUpgradeButton.onClick.AddListener(UpgradeOutpost);

        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    private void UpgradeOutpost()
    {
        _playerController.UpgradeOutpost();
    }

    public void OnOutpostUpgraded(OutpostData outpostData, FactionEnum faction)
    {
        if (faction != FactionEnum.Player)
            return;

        outpostName.text = outpostData.Name;
        outpostImage.sprite = outpostData.Sprite;
        outpostHitpoints.text = outpostData.Health.ToString();
        outpostTrainCapacity.text = outpostData.MaxQueueCapacity.ToString();
        outpostMinerCapacity.text = outpostData.MaxMinerUnits.ToString();
        outpostUpgradeCost.text = outpostData.UpgradePrice.ToString();

        if (outpostData.UpgradePrice == 0)
        {
            upgradeContainer.SetActive(false);
            maxLevelContainer.SetActive(true);
        }
    }
}
