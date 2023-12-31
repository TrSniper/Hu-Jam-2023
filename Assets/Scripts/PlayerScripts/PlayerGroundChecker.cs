using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float radius = 1f;
    [SerializeField] private float offset = -0.95f;

    [Header("Select")] [SerializeField] private bool gizmos;

    [Header("Info - No Touch")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private int collidedObjectNumber;

    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
        collidedObjectNumber = colliders.Length - 1; //must not include player

        foreach (Collider col in colliders)
        {
            if (col.isTrigger) collidedObjectNumber--;
        }

        psd.isGrounded = collidedObjectNumber > 0;
        if (psd.isGrounded) psd.isJumping = false;
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
    }
}