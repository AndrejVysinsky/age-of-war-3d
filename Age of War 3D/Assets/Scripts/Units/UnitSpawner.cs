using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<Unit> unitPrefabs;

    private int[] _currentUnitTiers;
    private List<Unit> _spawnedUnits;

    private int _unitIDCounter;

    public int NumberOfDifferentUnits => unitPrefabs.Count;

    private void Awake()
    {
        _unitIDCounter = 0;
        _currentUnitTiers = new int[unitPrefabs.Count];
        _spawnedUnits = new List<Unit>();
    }

    public void SpawnUnit(int unitIndex, Line line, FactionEnum faction, Material factionMaterial)
    {
        if (unitIndex < 0 || unitIndex > unitPrefabs.Count - 1)
            return;

        var unitPrefab = unitPrefabs[unitIndex];

        var unitObject = Instantiate(unitPrefab, transform);

        var unit = unitObject.GetComponent<Unit>();
        unit.Initialize(_unitIDCounter++, _currentUnitTiers[unitIndex], line, faction, factionMaterial);
        unit.OnUnitDeath.AddListener(RemoveUnit);

        _spawnedUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _spawnedUnits.Remove(unit);
    }

    public void UpgradeUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex > unitPrefabs.Count - 1)
            return;

        if (_currentUnitTiers[unitIndex] >= unitPrefabs[unitIndex].NumberOfUnitTiers)
            return;

        _currentUnitTiers[unitIndex]++;
    }
}
