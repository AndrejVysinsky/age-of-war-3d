using System.Collections.Generic;
using UnityEngine;

public class AIUnitScanner : MonoBehaviour
{
    public class LineUnitHolder
    {
        public Line line;
        public List<Unit> enemyUnits;
        public List<Unit> myUnits;
    }

    public List<LineUnitHolder> LineUnitHolders { get; } = new List<LineUnitHolder>();

    public void Initialize(List<Line> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            LineUnitHolders.Add(new LineUnitHolder
            {
                line = lines[i],
                enemyUnits = new List<Unit>(),
                myUnits = new List<Unit>()
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit otherUnit))
        {
            for (int i = 0; i < LineUnitHolders.Count; i++)
            {
                if (LineUnitHolders[i].line == otherUnit.Line)
                {
                    if (otherUnit.Faction == FactionEnum.Player)
                    {
                        LineUnitHolders[i].enemyUnits.Add(otherUnit);
                    }
                    else
                    {
                        LineUnitHolders[i].myUnits.Add(otherUnit);
                    }
                    otherUnit.OnUnitDeath.AddListener(RemoveUnit);
                }
            }
        }
    }

    private void RemoveUnit(Unit unit)
    {
        for (int i = 0; i < LineUnitHolders.Count; i++)
        {
            if (LineUnitHolders[i].line == unit.Line)
            {
                if (unit.Faction == FactionEnum.Player)
                {
                    LineUnitHolders[i].enemyUnits.Remove(unit);
                }
                else
                {
                    LineUnitHolders[i].myUnits.Remove(unit);
                }
            }
        }
    }
}
