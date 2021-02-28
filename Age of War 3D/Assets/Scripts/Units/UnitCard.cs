using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] GameObject unitPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        unitSpawner.SpawnUnit(unitPrefab);
    }
}