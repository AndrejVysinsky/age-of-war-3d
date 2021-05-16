using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAbilityUpgradeCard : MonoBehaviour, IArrowAbilityUpgraded
{
    [Header("Components")]
    [SerializeField] TMP_Text arrowAbilityName;
    [SerializeField] Image arrowAbilityImage;
    [SerializeField] TMP_Text arrowAbilityDamage;
    [SerializeField] TMP_Text arrowAbilityCooldown;
    [SerializeField] TMP_Text arrowAbilityUpgradePrice;
    [SerializeField] Button arrowAbilityUpgradeButton;

    [SerializeField] GameObject upgradeContainer;
    [SerializeField] GameObject maxLevelContainer;    

    private PlayerController _playerController;

    private void Start()
    {
        upgradeContainer.SetActive(true);
        maxLevelContainer.SetActive(false);

        arrowAbilityUpgradeButton.onClick.AddListener(UpgradeArrowAbility);

        _playerController = FindObjectOfType<PlayerController>();

        OnArrowAbilityUpgraded(_playerController.ArrowAbility.ArrowAbilityData, _playerController.Faction);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    private void UpgradeArrowAbility()
    {
        _playerController.UpgradeArrowAbility();
    }

    public void OnArrowAbilityUpgraded(ArrowAbilityData arrowAbilityData, FactionEnum faction)
    {
        if (faction != FactionEnum.Player)
            return;

        arrowAbilityName.text = arrowAbilityData.Name;
        arrowAbilityImage.sprite = arrowAbilityData.Sprite;
        arrowAbilityDamage.text = arrowAbilityData.Damage.ToString();
        arrowAbilityCooldown.text = arrowAbilityData.Cooldown.ToString();
        arrowAbilityUpgradePrice.text = arrowAbilityData.UpgradePrice.ToString();

        if (arrowAbilityData.UpgradePrice == 0)
        {
            upgradeContainer.SetActive(false);
            maxLevelContainer.SetActive(true);
        }
    }
}