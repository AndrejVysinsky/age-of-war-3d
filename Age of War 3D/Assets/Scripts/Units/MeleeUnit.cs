
public class MeleeUnit : Unit
{
    protected override void DealDamage(IDamagable damagable)
    {
        base.DealDamage(damagable);

        damagable.TakeDamage(_unitData.Damage);
    }
}