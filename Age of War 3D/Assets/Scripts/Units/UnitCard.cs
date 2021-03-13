using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IUnitUpgraded
{
    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] PlayerController playerController;
    [SerializeField] int unitIndex;

    private UnitData _unitData;

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
        playerController.SpawnUnit(unitIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Show tooltip with unit info
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Hide tooltip
    }

    public void OnUnitUpgraded(int upgradedUnitIndex, UnitData upgradedUnitData)
    {
        if (upgradedUnitIndex != unitIndex)
            return;

        _unitData = upgradedUnitData;

        priceText.text = _unitData.TrainCost.ToString();
    }
}