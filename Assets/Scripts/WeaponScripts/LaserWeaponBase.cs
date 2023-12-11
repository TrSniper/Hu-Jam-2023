using Cysharp.Threading.Tasks;
using UnityEngine;

public class LaserWeaponBase : WeaponBase
{
    [Header("Assign")] [SerializeField] protected float laserTime = 0.1f;

    private LineRenderer lr;

    protected override void OnAwake()
    {
        base.OnAwake();

        lr = GetComponent<LineRenderer>();
        isLaser = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (lr.enabled)
        {
            lr.SetPosition(0, outTransform.position);
            if (isHeldByEnemy) lr.SetPosition(1, pam.transform.position + new Vector3(0f, 2f, 0f));
            else lr.SetPosition(1, GetMiddleOfTheScreen(pam.distanceToHitTarget));
        }
    }

    public override async void Attack()
    {
        base.Attack();

        lr.enabled = true;
        await UniTask.WaitForSeconds(laserTime);
        lr.enabled = false;
    }
}
