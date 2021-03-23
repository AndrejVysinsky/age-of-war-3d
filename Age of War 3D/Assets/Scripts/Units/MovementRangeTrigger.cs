using System.Collections.Generic;
using UnityEngine;

public class MovementRangeTrigger : MonoBehaviour
{
    [SerializeField] Unit unit;

    private List<Unit> _unitInRange;

    private void Awake()
    {
        _unitInRange = new List<Unit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            if (otherUnit.Line == unit.Line)
            {
                _unitInRange.Add(otherUnit);
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
                _unitInRange.Remove(otherUnit);
                unit.UnitInMovementRange = GetUnitInFront();
            }
        }
    }

    private Unit GetUnitInFront()
    {
        _unitInRange.RemoveAll(unit => unit == null);

        Unit enemyInFront = null;
        //find ally in front of unit
        for (int i = 0; i < _unitInRange.Count; i++)
        {
            if (_unitInRange[i].Faction == unit.Faction)
            {
                if (_unitInRange[i].UnitID < unit.UnitID)
                    return _unitInRange[i];
            }
            else
            {
                enemyInFront = _unitInRange[i];
            }
        }

        return enemyInFront;
    }
}
