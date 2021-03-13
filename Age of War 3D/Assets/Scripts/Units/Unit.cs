﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitHealth unitHealth;
    [SerializeField] List<UnitData> unitTiers;

    private bool _isAttacking;

    public Unit EnemyInRange { get; set; }
    public Unit AllyInRange { get; set; }

    private Vector3 _destination;
    private int _nextCheckpointIndex;

    public UnityEvent<Unit> OnUnitDeath { get; set; }

    public UnitData UnitData { get; private set; }
    public int NumberOfUnitTiers => unitTiers.Count;
    public Line Line { get; private set; }
    public int UnitID { get; private set; }
    public FactionEnum Faction { get; private set; }

    private void Awake()
    {
        _isAttacking = false;

        OnUnitDeath = new UnityEvent<Unit>();
    }

    public int GetUnitTrainCost(int currentTier)
    {
        return unitTiers[currentTier].TrainCost;
    }

    public int GetUnitUpgradeCost(int tierToUpgrade)
    {
        return unitTiers[tierToUpgrade].UpgradeCost;
    }

    public UnitData GetUnitData(int currentTier)
    {
        return unitTiers[currentTier];
    }

    public void Initialize(int unitID, int unitTier, Line line, FactionEnum faction, Material factionMaterial)
    {
        UnitID = unitID;

        if (unitTier < 0 || unitTier > unitTiers.Count - 1)
        {
            UnitData = unitTiers[0];
        }
        else
        {
            UnitData = unitTiers[unitTier];
        }

        unitHealth.Initialize(UnitData.HitPoints);

        Line = line;
        Faction = faction;

        transform.position = Line.GetCheckpointPosition(0, Faction);
        _nextCheckpointIndex = 1;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex, Faction);

        GetComponent<MeshRenderer>().material = factionMaterial;
    }

    private void Update()
    {
        if (EnemyInRange != null && _isAttacking == false)
        {
            StartCoroutine(Attack());
        }

        if (AllyInRange == null)
        {
            MoveTowardsEnemyBase();
        }
    }

    private void MoveTowardsEnemyBase()
    {
        if (_isAttacking)
            return;

        if (_destination == Vector3.zero)
            return;

        if (transform.position == _destination)
        {
            if (Line.HasNextCheckpoint(_nextCheckpointIndex, Faction) == false)
            {
                return;
            }

            _destination = Line.GetCheckpointPosition(_nextCheckpointIndex++, Faction);
        }

        transform.position = Vector3.MoveTowards(transform.position, _destination, UnitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (EnemyInRange == null || unitHealth.Health <= 0)
            {
                break;
            }

            EnemyInRange.TakeDamage(UnitData.Damage);
        }

        _isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        unitHealth.SubtractHealth(damage);
        
        if (unitHealth.Health <= 0)
        {
            OnUnitDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
