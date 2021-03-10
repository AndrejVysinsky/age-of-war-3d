using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] PlayerController playerController;
    [SerializeField] int unitIndex;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerController.SpawnUnit(unitIndex);
    }
}