using UnityEngine;

public class ProjectileWeaponBase : WeaponBase
{
    [Header("Assign")] [SerializeField] protected GameObject projectile;

    private Transform playerTransform;
    private Transform cameraTransform;

    protected override void OnAwake()
    {
        base.OnAwake();

        playerTransform = GameObject.Find("Player").transform;
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }

    public override void Attack()
    {
        base.Attack();
        Instantiate(projectile, outTransform.position, outTransform.rotation);
    }

    protected override void OnUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit)) outTransform.LookAt(hit.point);
        else outTransform.LookAt(GetMiddleOfTheScreen(pam.attackRange));
    }
}
