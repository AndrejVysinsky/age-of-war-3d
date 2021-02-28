using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    private float _health;
    private bool _isAttacking;

    private Vector3 _destination;
    private List<Unit> _enemiesInRange;

    public UnitData UnitData => unitData;
    public FactionEnum Faction { get; private set; }

    private void Awake()
    {
        _enemiesInRange = new List<Unit>();

        _health = unitData.HitPoints;
        _isAttacking = false;
    }

    public void Initialize(Vector3 destination, FactionEnum faction)
    {
        _destination = destination;
        Faction = faction;
    }

    private void Update()
    {
        if (_enemiesInRange.Count > 0 && _isAttacking == false)
        {
            StartCoroutine(Attack());
        }

        MoveTowardsEnemyBase();
    }

    private void MoveTowardsEnemyBase()
    {
        if (_isAttacking)
            return;

        if (_destination == Vector3.zero)
            return;

        if (transform.position == _destination)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _destination, UnitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        while (true)
        {
            var enemy = GetFirstEnemyInRange();

            if (enemy != null)
            {
                break;
            }

            enemy.TakeDamage(UnitData.Damage);

            yield return new WaitForSeconds(UnitData.AttackDelay);
        }

        _isAttacking = false;
    }

    private Unit GetFirstEnemyInRange()
    {
        _enemiesInRange.RemoveAll(enemy => enemy == null);

        if (_enemiesInRange.Count > 0)
            return _enemiesInRange[0];
        else
            return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit))
        {
            if (unit.Faction != Faction)
            {
                _enemiesInRange.Add(unit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit))
        {
            if (unit.Faction != Faction)
            {
                _enemiesInRange.Remove(unit);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
