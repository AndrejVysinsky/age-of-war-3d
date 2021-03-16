﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamagable
{
    [SerializeField] HealthSlider unitHealth;
    [SerializeField] ColorSwitcher materialSwitcher;
    [SerializeField] List<UnitData> unitTiers;

    private bool _isAttacking;

    protected UnitData _unitData;

    private Vector3 _destination;
    private int _nextCheckpointIndex;

    public Unit EnemyInRange { get; set; }
    public Unit AllyInRange { get; set; }
    public Outpost OutpostInRange { get; set; }

    public UnityEvent<Unit> OnUnitDeath { get; set; }
    
    public int NumberOfUnitTiers => unitTiers.Count;
    public Line Line { get; private set; }
    public int UnitID { get; private set; }
    public FactionEnum Faction { get; private set; }

    private void Awake()
    {
        _isAttacking = false;

        OnUnitDeath = new UnityEvent<Unit>();
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
            _unitData = unitTiers[0];
        }
        else
        {
            _unitData = unitTiers[unitTier];
        }

        unitHealth.Initialize(_unitData.HitPoints, false);

        Line = line;
        Faction = faction;

        transform.position = Line.GetCheckpointPosition(0, Faction);
        _nextCheckpointIndex = 1;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex, Faction);

        if (materialSwitcher != null)
        {
            materialSwitcher.SwitchColors(factionMaterial.color);
        }
    }

    private void Update()
    {
        if (_isAttacking == false)
        {
            if (EnemyInRange != null || OutpostInRange != null)
            {
                StartCoroutine(AttackMode());
            }
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

        transform.position = Vector3.MoveTowards(transform.position, _destination, _unitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator AttackMode()
    {
        _isAttacking = true;

        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (unitHealth.Health <= 0)
            {
                break;
            }

            //prioritize enemies before outpost
            if (EnemyInRange != null)
            {
                Attack(EnemyInRange);
            }
            else if (OutpostInRange != null)
            {
                Attack(OutpostInRange);
            }
            else
            {
                break;
            }
        }

        _isAttacking = false;
    }

    private void Attack(IDamagable damagable)
    {
        //play animation

        //at animation end deal damage
        DealDamage(damagable);
    }

    protected virtual void DealDamage(IDamagable damagable)
    {
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
