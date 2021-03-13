using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<Unit> unitPrefabs;
    [SerializeField] TextMeshProUGUI queueCapacityText;
    [SerializeField] int maxQueueCapacity;

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

        UpdateQueueCapacityText();

        StartCoroutine(UnitQueue());
    }

    public int SpawnUnit(int unitIndex, Line line, FactionEnum faction, Material factionMaterial, int balance)
    {
        if (unitIndex < 0 || unitIndex > unitPrefabs.Count - 1)
            return 0;

        if (_unitsInQueue.Count >= maxQueueCapacity)
            return 0;

        var unitPrefab = unitPrefabs[unitIndex];

        int cost = unitPrefab.GetComponent<Unit>().GetUnitTierCost(_currentUnitTiers[unitIndex]);
        if (cost > balance)
        {
            return 0;
        }
        var unitObject = Instantiate(unitPrefab, transform);

        var unit = unitObject.GetComponent<Unit>();
        unit.Initialize(_unitIDCounter++, _currentUnitTiers[unitIndex], line, faction, factionMaterial);
        unit.OnUnitDeath.AddListener(RemoveUnit);

        //_spawnedUnits.Add(unit);

        unit.gameObject.SetActive(false);

        _unitsInQueue.Add(unit);
        UpdateQueueCapacityText();
        return cost;
    }

    private void UpdateQueueCapacityText()
    {
        if (queueCapacityText == null)
            return;

        queueCapacityText.text = $"{_unitsInQueue.Count} / {maxQueueCapacity}";
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

        UpdateQueueCapacityText();
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
