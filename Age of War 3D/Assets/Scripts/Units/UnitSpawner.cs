﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<Unit> unitPrefabs;

    private int[] _currentUnitTiers;
    private List<Unit> _spawnedUnits;

    private List<Unit> _unitsInQueue;

    private int _unitIDCounter;

    public int NumberOfDifferentUnits => unitPrefabs.Count;

    private void Awake()
    {
        _unitIDCounter = 0;
        _currentUnitTiers = new int[unitPrefabs.Count];
        _spawnedUnits = new List<Unit>();
        _unitsInQueue = new List<Unit>();

        StartCoroutine(UnitQueue());
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

        //_spawnedUnits.Add(unit);

        unit.gameObject.SetActive(false);

        _unitsInQueue.Add(unit);
    }

    private IEnumerator UnitQueue()
    {
        while (true)
        {
            if (_unitsInQueue.Count != 0)
            {
                //TODO: variable time based on unit
                //TODO: queue visual indicator -> maybe indicator on which line unit will spawn
                var time = 1f;

                //wait until TrainUnit() is over
                yield return StartCoroutine(TrainUnit(_unitsInQueue[0], time));
            }

            yield return null;
        }
    }

    private IEnumerator TrainUnit(Unit unit, float time)
    {
        yield return new WaitForSeconds(time);

        _unitsInQueue.Remove(unit);
        _spawnedUnits.Add(unit);

        unit.gameObject.SetActive(true);
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
