﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<Unit> unitPrefabs;
    [SerializeField] TextMeshProUGUI queueCapacityText;

    private int[] _currentUnitTiers;
    private List<Unit> _spawnedUnits;
    private GoldController _goldController;
    private BaseGameController _gameController;

    private List<Unit> _unitsInQueue;
    private List<int> _unitsInQueueIndex;
    private int _maxQueueCapacity;

    private int _spawnedMiners;

    private int _unitIDCounter;

    public int NumberOfDifferentUnits => unitPrefabs.Count;

    private void Awake()
    {
        _unitIDCounter = 0;
        _currentUnitTiers = new int[unitPrefabs.Count];
        _spawnedUnits = new List<Unit>();
        _unitsInQueue = new List<Unit>();
        _unitsInQueueIndex = new List<int>();
        _goldController = GetComponent<GoldController>();
        _gameController = GetComponent<BaseGameController>();

        _spawnedMiners = 0;

        for (int i = 0; i < unitPrefabs.Count; i++)
        {
            EventManager.Instance.ExecuteEvent<IUnitUpgraded>((x, y) => x.OnUnitUpgraded(i, unitPrefabs[i].GetUnitData(_currentUnitTiers[i]), _gameController.Faction));
        }

        UpdateQueueCapacityText();

        StartCoroutine(UnitQueue());
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    public int SpawnUnit(int unitIndex, Line line, FactionEnum faction, Material factionMaterial, int balance)
    {
        if (unitIndex < 0 || unitIndex > unitPrefabs.Count - 1)
            return 0;

        if (_unitsInQueue.Count >= _maxQueueCapacity)
            return 0;

        var unitPrefab = unitPrefabs[unitIndex];

        int cost = unitPrefab.GetComponent<Unit>().GetUnitData(_currentUnitTiers[unitIndex]).TrainCost;
        if (cost > balance)
        {
            return 0;
        }

        var unitObject = Instantiate(unitPrefab, transform);

        var unit = unitObject.GetComponent<Unit>();
        unit.Initialize(_unitIDCounter++, _currentUnitTiers[unitIndex], line, faction, factionMaterial);
        if (unitIndex != 4)
        {
            unit.OnUnitDeath.AddListener(RemoveUnit); // TODO zisti na kereho anciasa to nejde na minera
        }

        unit.gameObject.SetActive(false);

        _unitsInQueue.Add(unit);
        _unitsInQueueIndex.Add(unitIndex);
        UpdateQueueCapacityText();
        return cost;
    }

    public bool SpawnMinerUnit(int unitIndex, FactionEnum faction, Material factionMaterial, int balance)
    {
        if (_spawnedMiners >= GetComponent<Outpost>().MaxMinerUnits)
            return false;

        var unitPrefab = unitPrefabs[unitIndex];

        var unitObject = Instantiate(unitPrefab, transform);

        var unit = unitObject.GetComponent<MinerUnit>();
        unit.Initialize(faction, factionMaterial);

        _spawnedMiners++;

        return true;
    }

    private void UpdateQueueCapacityText()
    {
        if (queueCapacityText == null)
            return;

        queueCapacityText.text = $"{_unitsInQueue.Count} / {_maxQueueCapacity}";
    }

    public void OnQueueCapacityChanged(int newCapacity)
    {
        _maxQueueCapacity = newCapacity;

        UpdateQueueCapacityText();
    }

    private IEnumerator UnitQueue()
    {
        while (true)
        {
            if (_unitsInQueue.Count != 0)
            {
                //wait until TrainUnit() is over
                EventManager.Instance.ExecuteEvent<IUnitTrainingStarted>((x, y) => x.OnUnitTrainingStarted(_unitsInQueueIndex[0], _gameController.Faction));

                yield return StartCoroutine(TrainUnit(_unitsInQueue[0], _unitsInQueue[0].GetUnitData().TrainTime));
            }

            yield return null;
        }
    }

    private IEnumerator TrainUnit(Unit unit, float time)
    {
        yield return new WaitForSeconds(time);

        _unitsInQueue.Remove(unit);
        _unitsInQueueIndex.RemoveAt(0);
        _spawnedUnits.Add(unit);

        unit.gameObject.SetActive(true);

        UpdateQueueCapacityText();
    }

    public void RemoveUnit(Unit unit)
    {
        _goldController.AddBalance(unit.GetUnitReward());
        _spawnedUnits.Remove(unit);
    }

    public int UpgradeUnit(int unitIndex, int balance)
    {
        if (unitIndex < 0 || unitIndex > unitPrefabs.Count - 1)
            return 0;

        if (_currentUnitTiers[unitIndex] + 1 >= unitPrefabs[unitIndex].NumberOfUnitTiers)
            return 0;

        if (balance < unitPrefabs[unitIndex].GetUnitData(_currentUnitTiers[unitIndex] + 1).UpgradeCost)
            return 0;

        _currentUnitTiers[unitIndex]++;

        EventManager.Instance.ExecuteEvent<IUnitUpgraded>((x, y) => x.OnUnitUpgraded(unitIndex, unitPrefabs[unitIndex].GetUnitData(_currentUnitTiers[unitIndex]), _gameController.Faction));

        if (_currentUnitTiers[unitIndex] == 1)
            return 0;

        return unitPrefabs[unitIndex].GetUnitData(_currentUnitTiers[unitIndex] - 1).UpgradeCost;
    }

    public List<Unit> GetUnitPrefabs()
    {
        return unitPrefabs;
    }

    public int[] GetCurrentUnitTiers()
    {
        return _currentUnitTiers;
    }
}
