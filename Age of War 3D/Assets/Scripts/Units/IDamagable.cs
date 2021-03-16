using UnityEngine;

public interface IDamagable
{
    FactionEnum Faction { get; }
    Transform Transform { get; }
    void TakeDamage(float damage);
}