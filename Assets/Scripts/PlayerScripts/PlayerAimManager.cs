using UnityEngine;

public class PlayerAimManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float interactRange = 5f;
    public float attackRange = 40f;

    [Header("Info - No Touch")]
    public bool canInteract;
    public bool canAttack;
    public float distanceToHitTarget;

    public IDamageable damageable;
    public IInteractable interactable;
    public IInteractable previousInteractable;

    private PlayerStateData psd;
    private CrosshairManager cm;
    private Camera cam;

    private RaycastHit crosshairHit;
    private Ray crosshairRay;
    private int layerMask = ~(1 << 7); //evil bit hack: every layer except layer 7

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        cm = GetComponent<CrosshairManager>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (psd.playerMainState != PlayerStateData.PlayerMainState.Normal) return;

        CastRay();

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) Debug.Log(crosshairHit.collider.gameObject.name);
        #endif
    }

    private void CastRay()
    {
        crosshairRay = cam.ScreenPointToRay(cm.crosshairImage.rectTransform.position);

        canInteract = false;
        canAttack = false;

        damageable = null;
        interactable = null;

        if (Physics.Raycast(crosshairRay, out crosshairHit, attackRange, layerMask))
        {
            distanceToHitTarget = Vector3.Distance(transform.position, crosshairHit.collider.transform.position);

            if (distanceToHitTarget < interactRange)
            {
                canInteract = crosshairHit.collider.CompareTag("Interactable");
                if (canInteract)
                {
                    interactable = crosshairHit.collider.GetComponentInChildren<IInteractable>();
                    previousInteractable = interactable;
                }
            }

            if (distanceToHitTarget < attackRange)
            {
                canAttack = crosshairHit.collider.CompareTag("Enemy");
                if (canAttack) damageable = crosshairHit.collider.GetComponent<IDamageable>();
            }
        }

        else
        {
            distanceToHitTarget = attackRange;
        }
    }
}
