using UnityEngine;

public class ProjectileWeaponBase : WeaponBase
{
    [Header("Assign")] [SerializeField] protected GameObject projectile;

    private RaycastHit hit;

    public override void Attack()
    {
        base.Attack();

        if (hit.collider != null) if (!hit.collider.CompareTag("Player")) Instantiate(projectile, outTransform.position, outTransform.rotation);
    }

    protected override void OnUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
        if (Physics.Raycast(ray, out hit)) outTransform.LookAt(hit.point);
        else outTransform.LookAt(GetMiddleOfTheScreen(pam.attackRange));
    }
}
