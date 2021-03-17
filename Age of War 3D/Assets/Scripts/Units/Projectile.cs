using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float speed;
    [SerializeField] float arcHeight;

    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private IDamagable _self;
    private IDamagable _target;
    private Vector3 _targetPosition;
    private float _damage;

    private bool _isMoving = true;

    public void Initialize(Vector3 startPosition, IDamagable self, IDamagable target, float damage)
    {
        _startPosition = startPosition;
        _currentPosition = startPosition;
        _self = self;
        _target = target;
        _targetPosition = target.Transform.position;
        _damage = damage;

        transform.position = startPosition;

        var forward = self.Transform.forward;

        _targetPosition = self.Transform.position + forward * distance;
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        if (_currentPosition == _targetPosition)
        {
            _isMoving = false;
            Destroy(gameObject, 2f);
            return;
        }

        var targetDistance = Vector3.Distance(_startPosition, _targetPosition);

        _currentPosition = Vector3.MoveTowards(_currentPosition, _targetPosition, Time.deltaTime * targetDistance * speed);

        float arc = MathfArc.GetArcHeightAtPosition(_startPosition, _currentPosition, _targetPosition, arcHeight);

        transform.position = new Vector3(_currentPosition.x, _currentPosition.y + arc, _currentPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            if (damagable.Faction != _self.Faction)
            {
                damagable.TakeDamage(_damage);

                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;

                if (TryGetComponent(out ParticleSystem particleSystem))
                {
                    var emission = particleSystem.emission;
                    emission.enabled = false;
                }
                _isMoving = false;

                Destroy(gameObject, 1f);
            }
        }
    }
}