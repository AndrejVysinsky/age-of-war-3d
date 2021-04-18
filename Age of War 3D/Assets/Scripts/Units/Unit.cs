﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Unit : MonoBehaviour, IDamagable
{
    [SerializeField] HealthSlider unitHealth;
    [SerializeField] protected ColorSwitcher colorSwitcher;
    [SerializeField] protected List<UnitData> unitTiers;
    [SerializeField] GameObject dmgTakenText;

    private bool _isAttacking;

    protected UnitData _unitData;

    protected Vector3 _destination;
    private int _nextCheckpointIndex;

    public Unit EnemyInRange { get; set; }
    public Unit UnitInMovementRange { get; set; }
    public Outpost OutpostInRange { get; set; }

    public UnityEvent<Unit> OnUnitDeath { get; set; }
    
    public int NumberOfUnitTiers => unitTiers.Count;
    public Line Line { get; private set; }
    public int UnitID { get; private set; }
    public FactionEnum Faction { get; protected set; }
    public Transform Transform { get; private set; }
    private float HEIGHT_OFFSET = 2.2f;

    private void Awake()
    {
        _isAttacking = false;

        Transform = transform;
        OnUnitDeath = new UnityEvent<Unit>();
    }

    public UnitData GetUnitData(int currentTier)
    {
        return unitTiers[currentTier];
    }

    public int GetUnitReward()
    {
        return _unitData.Reward;
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

        transform.position = Line.GetSpawnPointPosition(Faction);
        _nextCheckpointIndex = 0;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex, Faction);
        transform.LookAt(_destination);

        if (colorSwitcher != null)
        {
            colorSwitcher.SwitchColors(factionMaterial.color);
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

        if (UnitInMovementRange == null)
        {
            MoveTowardsObjective();
        }
    }

    protected virtual void MoveTowardsObjective()
    {
        if (_destination == Vector3.zero)
            return;

        if (transform.position == _destination)
        {
            if (Line.HasNextCheckpoint(_nextCheckpointIndex, Faction) == false)
            {
                return;
            }

            _destination = Line.GetCheckpointPosition(_nextCheckpointIndex++, Faction);
            transform.LookAt(_destination);
        }

        transform.position = Vector3.MoveTowards(transform.position, _destination, _unitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator AttackMode()
    {
        _isAttacking = true;

        while (true)
        {
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

            yield return new WaitForSeconds(2f);
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

    public virtual void TakeDamage(float damage)
    {
        unitHealth.SubtractHealth(damage);
        
        if (unitHealth.Health <= 0)
        {
            OnUnitDeath?.Invoke(this);
            Destroy(gameObject);
        }
        var position = transform.position;
        position.Set(position.x, position.y + HEIGHT_OFFSET, position.z);
        var text_holder = Instantiate(dmgTakenText, position, Quaternion.identity);
        text_holder.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + damage);
    }

    public float GetHealth()
    {
        return unitHealth.Health;
    }

    public float GetDamage()
    {
        return _unitData.Damage;
    }

    protected float getRandomizedDamage(float unitDamage)
    {
        float percentage = unitDamage * 0.1f;
        decimal acutal_dmg = (decimal)UnityEngine.Random.Range(unitDamage - percentage, unitDamage + percentage);
        float rounded = Convert.ToSingle(RoundDown(acutal_dmg, 1));
        return rounded;
    }

    private decimal RoundDown(decimal i, double decimalPlaces)
    {
        var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
        return Math.Floor(i * power) / power;
    }
}
