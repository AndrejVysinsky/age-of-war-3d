using System.Collections.Generic;
using UnityEngine;

public class AIUnitScanner : MonoBehaviour
{
    private Dictionary<Line, List<Unit>> _unitsInLine = new Dictionary<Line, List<Unit>>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            if (otherUnit.Faction == FactionEnum.Green)
            {
                if (_unitsInLine.ContainsKey(otherUnit.Line) == false)
                {
                    _unitsInLine.Add(otherUnit.Line, new List<Unit>());
                }
                _unitsInLine[otherUnit.Line].Add(otherUnit);

                otherUnit.OnUnitDeath.AddListener(RemoveUnit);
            }
        }
    }

    private void RemoveUnit(Unit unit)
    {
        _unitsInLine[unit.Line].Remove(unit);
    }
}
