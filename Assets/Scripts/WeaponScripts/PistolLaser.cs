using UnityEngine;

public class PistolLaser : WeaponBase
{
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        isLaser = true;
    }

    public override void Attack()
    {
        base.Attack();
        Debug.Log("Attack Pistol Laser");
    }

    private void Update()
    {
        lr.SetPosition(0, outTransform.position);
    }
}
