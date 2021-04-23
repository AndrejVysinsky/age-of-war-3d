using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAbilityButton : MonoBehaviour, IArrowAbilityUpgraded
{
    [SerializeField] Button button;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    [SerializeField] TMP_Text timerText;

    private ArrowAbility _arrowAbility;

    private float _remainingCooldown;
    private float _cooldown;

    private void Start()
    {
        _arrowAbility = FindObjectOfType<PlayerController>().ArrowAbility;

        button.onClick.AddListener(UseAbility);
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
        if (_remainingCooldown <= 0)
        {
            timerImage.fillAmount = 0;
            timerText.text = "";
            return;
        }

        _remainingCooldown -= Time.deltaTime;

        timerImage.fillAmount = _remainingCooldown / _cooldown;
        timerText.text = Mathf.FloorToInt(_remainingCooldown).ToString();
    }

    public void OnArrowAbilityUpgraded(ArrowAbilityData arrowAbilityData, FactionEnum faction)
    {
        RefreshTimer(arrowAbilityData.Cooldown);
    }

    private void UseAbility()
    {
        if (_remainingCooldown > 0)
            return;

        _arrowAbility.UseAbility();
        RefreshTimer(_arrowAbility.Cooldown);
    }

    private void RefreshTimer(float timer)
    {
        _remainingCooldown = timer;
        _cooldown = timer;
    }
}