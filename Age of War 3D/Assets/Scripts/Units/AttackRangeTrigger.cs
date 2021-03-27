using System.Collections.Generic;
using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour
{
    [SerializeField] Unit unit;

    private List<Unit> _enemiesInRange;

    private void Awake()
    {
        _enemiesInRange = new List<Unit>();
        unit.OnUnitDeath.AddListener(RemoveUnit);
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
                    _enemiesInRange.Remove(otherUnit);
                    unit.EnemyInRange = GetEnemyInFront();
                }
            }
        }
    }

    private void RemoveUnit(Unit unit)
    {

    }

    private Unit GetEnemyInFront()
    {
        _enemiesInRange.RemoveAll(unit => unit == null);

        int index = 0;
        int minId = int.MaxValue;
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i].UnitID < minId)
            {
                index = i;
                minId = _enemiesInRange[i].UnitID;
            }
        }

        if (minId == int.MaxValue)
            return null;

        return _enemiesInRange[index];
    }
}
