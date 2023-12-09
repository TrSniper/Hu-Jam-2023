using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Assign")]
    public int weaponIndex;
    public bool isLaser;
    public int damage;
    public int knockBack;
    public float cooldownTime;
    public Transform outTransform;

    public virtual void Attack() {}
}
