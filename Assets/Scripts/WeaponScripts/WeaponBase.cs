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

    protected Camera mainCamera;
    protected PlayerAimManager pam;

    protected virtual void OnAwake()
    {
        mainCamera = Camera.main;
        pam = GameObject.Find("Player").GetComponent<PlayerAimManager>();
    }

    protected virtual void OnUpdate() {}
    public virtual async void Attack() {}
}
