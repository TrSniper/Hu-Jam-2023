using UnityEngine;

public class RifleLaser : LaserWeaponBase
{
    private void Awake()
    {
        OnAwake();
    }

    public override async void Attack()
    {
        base.Attack();
        Debug.Log("Attack Rifle Laser");
    }

    private void Update()
    {
        OnUpdate();
    }
}
