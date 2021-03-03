using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject unitPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerController.SpawnUnit(unitPrefab);
    }
}