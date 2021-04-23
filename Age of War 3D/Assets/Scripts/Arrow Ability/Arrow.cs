using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float fallingSpeed;

    private float _damage;
    private float _targetY;
    private FactionEnum _faction;

    public void Initialize(Vector3 spawnPosition, float targetY, float damage, FactionEnum faction)
    {
        transform.position = spawnPosition;
        _damage = damage;
        _targetY = targetY;
        _faction = faction;
    }

    private void Update()
    {
        if (transform.position.y <= _targetY)
        {
            Destroy(gameObject, 2f);
            return;
        }

        var position = transform.position;

        position.y -= Time.deltaTime * fallingSpeed;

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit.Faction != _faction)
            {
                unit.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}