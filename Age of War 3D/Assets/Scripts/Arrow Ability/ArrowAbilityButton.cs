using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowAbilityButton : MonoBehaviour, IArrowAbilityUpgraded, IPointerDownHandler
{
    [SerializeField] Image abilityImage;
    [SerializeField] TMP_Text abilityPrice;
    [SerializeField] TMP_Text hotKeyText;
    [SerializeField] KeyCode hotKey;

    [Header("Timer")]
    [SerializeField] Image timerImage;

    private PlayerController _playerController;

    private float _remainingCooldown;
    private float _cooldown;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

        hotKeyText.text = hotKey.ToString();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            UseAbility();
        }

        if (_remainingCooldown <= 0)
        {
            timerImage.fillAmount = 0;
            timerImage.gameObject.SetActive(false);
            return;
        }

        _remainingCooldown -= Time.deltaTime;

        timerImage.fillAmount = _remainingCooldown / _cooldown;
    }

    public void OnArrowAbilityUpgraded(ArrowAbilityData arrowAbilityData, FactionEnum faction)
    {
        RefreshTimer(arrowAbilityData.Cooldown);

        abilityImage.sprite = arrowAbilityData.Sprite;
        abilityPrice.text = arrowAbilityData.Price.ToString();
    }

    private void UseAbility()
    {
        if (_remainingCooldown > 0)
            return;

        _playerController.UseArrowAbility();
        RefreshTimer(_playerController.ArrowAbility.Cooldown);
    }

    private void RefreshTimer(float timer)
    {
        _remainingCooldown = timer;
        _cooldown = timer;
        timerImage.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UseAbility();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseCursor.Instance.SetHandCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseCursor.Instance.SetPointerCursor();
    }
}