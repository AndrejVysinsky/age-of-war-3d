using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Units/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] float hitPoints;
    [SerializeField] float movementSpeed;
    [SerializeField] float damage;
    [SerializeField] float attackDelay;
    [SerializeField] float attackRange;

    public string Name => name;
    public float HitPoints => hitPoints;
    public float MovementSpeed => movementSpeed;
    public float Damage => damage;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
}