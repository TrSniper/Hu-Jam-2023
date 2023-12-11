using UnityEngine;

public class ProjectileBulletDamage : MonoBehaviour
{
    [Header("Assign")] [SerializeField] private WeaponBase weapon;
    private PlayerCombatManager pcm;

    private void Awake()
    {
        pcm = GameObject.Find("Player").GetComponent<PlayerCombatManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnemyStateManager>().GetDamage(weapon.damage);
        }

        if (collision.collider.CompareTag("Player")) pcm.GetDamage(weapon.damage);
    }
}
