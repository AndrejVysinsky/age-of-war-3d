using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    [SerializeField] Unit unit;

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
                    unit.AllyInRange = otherUnit;
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
                    unit.AllyInRange = null;
                }
            }
        }
    }
}
