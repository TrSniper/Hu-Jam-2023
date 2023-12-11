using UnityEngine;

public class ProjectileWeaponBase : WeaponBase
{
    [Header("Assign")] [SerializeField] protected GameObject projectile;

    public override void Attack()
    {
        base.Attack();

        if (hit.collider != null && !hit.collider.CompareTag("Player")) Instantiate(projectile, outTransform.position, outTransform.rotation);
    }
}
