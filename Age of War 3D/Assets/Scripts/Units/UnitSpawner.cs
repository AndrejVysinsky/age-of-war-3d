using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    private List<Unit> _units;

    private int unitIDCounter;

    private void Awake()
    {
        unitIDCounter = 0;
        _units = new List<Unit>();
    }

    public void SpawnUnit(GameObject unitPrefab, Line line, FactionEnum faction)
    {
        var unitObject = Instantiate(unitPrefab, transform);

        var unit = unitObject.GetComponent<Unit>();

        unit.Initialize(unitIDCounter++, line, faction);
        unit.OnUnitDeath.AddListener(RemoveUnit);

        _units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }
}
