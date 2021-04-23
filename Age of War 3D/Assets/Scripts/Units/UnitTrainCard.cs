using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitTrainCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IUnitUpgraded
{
    [Header("Components")]
    [SerializeField] Image unitImage;
    [SerializeField] TextMeshProUGUI priceText;

    [Header("Misc")]
    [SerializeField] int unitIndex;

    public UnitData UnitData { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public int UnitIndex => unitIndex;

    private void Start()
    {
        PlayerController = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.SpawnUnit(unitIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Show tooltip with unit info
        MouseCursor.Instance.SetHandCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Hide tooltip
        MouseCursor.Instance.SetPointerCursor();
    }

    public void OnUnitUpgraded(int upgradedUnitIndex, UnitData upgradedUnitData, FactionEnum faction)
    {
        if (faction != FactionEnum.Green)
            return;

        if (upgradedUnitIndex != unitIndex)
            return;

        UnitData = upgradedUnitData;

        priceText.text = UnitData.TrainCost.ToString();

        if (UnitData.Sprite != null)
        {
            unitImage.sprite = UnitData.Sprite;
        }
    }
}