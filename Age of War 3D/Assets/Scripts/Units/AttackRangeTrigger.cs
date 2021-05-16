using System.Collections.Generic;
using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour, IUnitDead
{
    [SerializeField] Unit unit;

    private List<Unit> _enemiesInRange;

    private void Awake()
    {
        _enemiesInRange = new List<Unit>();
        unit.OnUnitDeath.AddListener(RemoveUnit);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            //is enemy and is in same line
            if (otherUnit.Line == unit.Line)
            {
                if (otherUnit.Faction != unit.Faction)
                {
                    _enemiesInRange.Add(otherUnit);
                    unit.EnemyInRange = GetEnemyInFront();
                }
            }
        }
        else if (other.TryGetComponent(out Outpost outpost))
        {
            if (outpost.Faction != unit.Faction)
            {
                unit.OutpostInRange = outpost;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            //is enemy and is in same line
            if (otherUnit.Line == unit.Line)
            {
                if (otherUnit.Faction != unit.Faction)
                {
                    RemoveUnit(otherUnit);
                }
            }
        }
    }

    private void RemoveUnit(Unit otherUnit)
    {
        _enemiesInRange.Remove(otherUnit);
        unit.EnemyInRange = GetEnemyInFront();
    }

    private Unit GetEnemyInFront()
    {
        _enemiesInRange.RemoveAll(unit => unit == null);

        if (_enemiesInRange.Count == 0)
            return null;

        Unit enemyInFront = _enemiesInRange[0];
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i].Faction != unit.Faction)
            {
                if (_enemiesInRange[i].UnitID < enemyInFront.UnitID)
                {
                    enemyInFront = _enemiesInRange[i];
                }
            }
        }

        return enemyInFront;
    }

    public void OnUnitDead(Unit unit)
    {
        RemoveUnit(unit);
    }
}
