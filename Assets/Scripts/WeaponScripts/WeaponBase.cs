using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Assign")]
    public int weaponIndex;
    public int damage;
    public int knockBack;
    public float cooldownTime;
    public Transform outTransform;
    public bool canAutoFire;

    [Header("Info - No Touch")] public bool isLaser;

    protected PlayerAimManager pam;
    protected Camera mainCamera;

    protected virtual void OnAwake()
    {
        pam = GameObject.Find("Player").GetComponent<PlayerAimManager>();
        mainCamera = Camera.main;
    }

    protected virtual void OnUpdate() {}
    public virtual void Attack() {}

    protected Vector3 GetMiddleOfTheScreen(float zValue)
    {
        Vector3 viewportMiddle = new Vector3(0.5f, 0.5f, zValue);
        return Camera.main.ViewportToWorldPoint(viewportMiddle);
    }
}
