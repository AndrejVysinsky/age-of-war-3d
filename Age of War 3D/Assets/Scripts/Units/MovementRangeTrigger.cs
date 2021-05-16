using System.Collections.Generic;
using UnityEngine;

public class MovementRangeTrigger : MonoBehaviour, IUnitDead
{
    [SerializeField] Unit unit;

    private List<Unit> _unitsInRange;

    private void Awake()
    {
        _unitsInRange = new List<Unit>();
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
            if (otherUnit.Line == unit.Line)
            {
                _unitsInRange.Add(otherUnit);
                unit.UnitInMovementRange = GetUnitInFront();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            if (otherUnit.Line == unit.Line)
            {
                RemoveUnit(otherUnit);
            }
        }
    }

    private void RemoveUnit(Unit otherUnit)
    {
        _unitsInRange.Remove(otherUnit);
        unit.UnitInMovementRange = GetUnitInFront();
    }

    private Unit GetUnitInFront()
    {
        _unitsInRange.RemoveAll(unit => unit == null);

        //either get unit with smallest unitId or get enemy unit

        Unit enemyInFront = unit;
        Unit allyInFront = unit;
        for (int i = 0; i < _unitsInRange.Count; i++)
        {
            if (_unitsInRange[i].Faction == unit.Faction)
            {
                if (_unitsInRange[i].UnitID < allyInFront.UnitID)
                {
                    allyInFront = _unitsInRange[i];
                }
            }
            else
            {
                if (_unitsInRange[i].UnitID < enemyInFront.UnitID)
                {
                    enemyInFront = _unitsInRange[i];
                }
            }
        }

        if (enemyInFront != unit)
            return enemyInFront;

        if (allyInFront != unit)
            return allyInFront;

        return null;
    }

    public void OnUnitDead(Unit unit)
    {
        RemoveUnit(unit);
    }
}
