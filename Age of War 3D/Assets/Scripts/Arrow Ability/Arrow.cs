using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float fallingSpeed;
    [SerializeField] AudioSource audioSource;

    private float _damage;
    private float _targetY;
    private FactionEnum _faction;

    private bool _isOnGround = false;

    public void Initialize(Vector3 spawnPosition, float targetY, float damage, FactionEnum faction)
    {
        transform.position = spawnPosition;
        _damage = damage;
        _targetY = targetY;
        _faction = faction;

        audioSource.PlayDelayed(1f);
    }

    private void Update()
    {
        if (transform.position.y <= _targetY)
        {
            _isOnGround = true;
            Destroy(gameObject, 2f);
            return;
        }

        var position = transform.position;

        position.y -= Time.deltaTime * fallingSpeed;

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnGround)
            return;

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