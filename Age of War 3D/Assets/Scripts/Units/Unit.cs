using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    private float _health;
    private bool _isAttacking;

    private Unit _enemyInRange;
    private Unit _allyInRange;

    private Vector3 _destination;
    private int _nextCheckpointIndex;

    public UnityEvent<Unit> OnUnitDeath { get; set; }

    public UnitData UnitData => unitData;
    public Line Line { get; private set; }
    public FactionEnum Faction { get; private set; }

    private void Awake()
    {
        _health = unitData.HitPoints;
        _isAttacking = false;

        OnUnitDeath = new UnityEvent<Unit>();
    }

    public void Initialize(Line line, FactionEnum faction)
    {
        Line = line;
        Faction = faction;

        transform.position = Line.GetCheckpointPosition(0);
        _nextCheckpointIndex = 1;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex);
    }

    private void Update()
    {
        if (_enemyInRange != null)
        {
            StartCoroutine(Attack());
        }

        if (_allyInRange == null)
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
            if (Line.HasNextCheckpoint(_nextCheckpointIndex) == false)
            {
                return;
            }

            _destination = Line.GetCheckpointPosition(_nextCheckpointIndex++);
        }

        transform.position = Vector3.MoveTowards(transform.position, _destination, UnitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        while (true)
        {
            if (_enemyInRange == null)
            {
                break;
            }

            _enemyInRange.TakeDamage(UnitData.Damage);

            yield return new WaitForSeconds(UnitData.AttackDelay);
        }

        _isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            //is enemy and is in same line
            if (unit.Line == Line)
            {
                if (unit.Faction != Faction)
                {
                    _enemyInRange = unit;
                }
                else
                {
                    _allyInRange = unit;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            //is enemy and is in same line
            if (unit.Line == Line)
            {
                if (unit.Faction != Faction)
                {
                    _enemyInRange = null;
                }
                else
                {
                    _allyInRange = null;
                }
            }
        }
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
