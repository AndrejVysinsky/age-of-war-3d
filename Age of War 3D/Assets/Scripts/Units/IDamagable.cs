using UnityEngine;

public interface IDamagable
{
    /// <summary>
    /// 
    /// </summary>
    FactionEnum Faction { get; }
    /// <summary>
    /// For tracking objects position
    /// </summary>
    Transform Transform { get; }
    void TakeDamage(float damage);
}