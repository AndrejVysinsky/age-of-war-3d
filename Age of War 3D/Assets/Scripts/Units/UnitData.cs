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
    [SerializeField] int trainCost;
    [SerializeField] int upgradeCost;
    [SerializeField] int reward;

    public string Name => name;
    public float HitPoints => hitPoints;
    public float MovementSpeed => movementSpeed;
    public float Damage => damage;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
    public int TrainCost => trainCost;
    public int UpgradeCost => upgradeCost;
    public int Reward => reward;
}