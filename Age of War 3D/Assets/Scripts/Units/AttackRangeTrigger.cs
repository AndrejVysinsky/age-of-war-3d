using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour
{
    [SerializeField] Unit unit;

    private void Awake()
    {
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
                    unit.EnemyInRange = otherUnit;
                }
            }
        }
        else if (other.TryGetComponent(out Outpost outpost))
        {
            unit.OutpostInRange = outpost;
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
            }
        }
    }

    private void RemoveUnit(Unit unit)
    {

    }
}
