using System.Collections.Generic;
using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    [SerializeField] Unit unit;

    private List<Unit> _alliesInRange;

    private void Awake()
    {
        _alliesInRange = new List<Unit>();
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
                    unit.EnemyInRange = otherUnit;
                }
                else
                {
                    _alliesInRange.Add(otherUnit);
                    unit.AllyInRange = GetAllyInFront();
                }
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
                    unit.EnemyInRange = null;
                }
                else
                {
                    _alliesInRange.Remove(otherUnit);
                    unit.AllyInRange = GetAllyInFront();
                }
            }
        }
    }

    private Unit GetAllyInFront()
    {
        _alliesInRange.RemoveAll(unit => unit == null);

        for (int i = 0; i < _alliesInRange.Count; i++)
        {
            if (_alliesInRange[i].UnitID < unit.UnitID)
            {
                return _alliesInRange[i];
            }
        }
        return null;
    }
}
