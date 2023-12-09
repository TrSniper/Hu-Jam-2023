using UnityEngine;

public class PistolLaser : LaserWeaponBase
{
    private void Awake()
    {
        OnAwake();
    }

    public override async void Attack()
    {
        base.Attack();
        Debug.Log("Attack Pistol Laser");
    }

    private void Update()
    {
        OnUpdate();
    }
}
