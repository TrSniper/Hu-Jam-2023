using UnityEngine;

public class PistolProjectile : ProjectileWeaponBase
{
    private void Awake()
    {
        OnAwake();
    }

    public override async void Attack()
    {
        base.Attack();
    }
}
