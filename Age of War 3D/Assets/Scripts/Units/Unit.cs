using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    private float _health;
    private bool _isAttacking;

    public Unit EnemyInRange { get; set; }
    public Unit AllyInRange { get; set; }

    private Vector3 _destination;
    private int _nextCheckpointIndex;

    public UnityEvent<Unit> OnUnitDeath { get; set; }

    public UnitData UnitData => unitData;
    public Line Line { get; private set; }
    public int UnitID { get; private set; }
    public FactionEnum Faction { get; private set; }

    private void Awake()
    {
        _health = unitData.HitPoints;
        _isAttacking = false;

        OnUnitDeath = new UnityEvent<Unit>();
    }

    public void Initialize(int unitID, Line line, FactionEnum faction)
    {
        UnitID = unitID;
        Line = line;
        Faction = faction;

        transform.position = Line.GetCheckpointPosition(0, Faction);
        _nextCheckpointIndex = 1;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex, Faction);
    }

    private void Update()
    {
        if (EnemyInRange != null)
        {
            StartCoroutine(Attack());
        }

        if (AllyInRange == null)
        {
            MoveTowardsEnemyBase();
        }
    }

    private void MoveTowardsEnemyBase()
    {
        if (_isAttacking)
            return;

        if (_destination == Vector3.zero)
            return;

        if (transform.position == _destination)
        {
            if (Line.HasNextCheckpoint(_nextCheckpointIndex, Faction) == false)
            {
                return;
            }

            _destination = Line.GetCheckpointPosition(_nextCheckpointIndex++, Faction);
        }

        transform.position = Vector3.MoveTowards(transform.position, _destination, UnitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        while (true)
        {
            if (EnemyInRange == null)
            {
                break;
            }

            EnemyInRange.TakeDamage(UnitData.Damage);

            yield return new WaitForSeconds(UnitData.AttackDelay);
        }

        _isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            OnUnitDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
