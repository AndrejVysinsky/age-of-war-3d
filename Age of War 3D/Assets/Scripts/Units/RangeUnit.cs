using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeUnit : Unit
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject firePoint;

    protected override void DealDamage(IDamagable damagable)
    {
        base.DealDamage(damagable);

        //spawn projectile
        var projectileObject = Instantiate(projectilePrefab);

        //initialize with start position, end position, damage
        projectileObject.GetComponent<Projectile>().Initialize(firePoint.transform.position, this, damagable, _unitData.Damage);
    }
}