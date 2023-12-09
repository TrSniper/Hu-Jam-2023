using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float interactRange = 5f;
    public float attackRange = 40f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    [Header("Info - No Touch")]
    public bool canInteract;
    public bool canAttack;
    public float distanceToHitTarget;

    public IDamageable damageable;
    public IInteractable interactable;
    public IInteractable previousInteractable;

    private PlayerStateData psd;
    private Camera cam;
    private Image crosshairImage;

    private RaycastHit crosshairHit;
    private Ray crosshairRay;
    private int layerMask = ~(1 << 7); //evil bit hack

    private Color temporaryColor;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        cam = Camera.main;
        crosshairImage = GetComponentInChildren<Image>();

        //Default value
        temporaryColor = Color.white;
        temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
    }

    private void Update()
    {
        if (psd.playerMainState != PlayerStateData.PlayerMainState.Normal) return;

        CastRay();
        HandleCrosshairColor();

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) Debug.Log(crosshairHit.collider.gameObject.name);
        #endif
    }

    private void CastRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

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
    }

    private void HandleCrosshairColor()
    {
        //TODO: DON'T DETECT DEAD ENEMIES

        //We can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it

        temporaryColor = Color.white;

        if (canInteract)
        {
            temporaryColor.a = 1f;
        }

        else if (canAttack && psd.isAiming)
        {
            temporaryColor = Color.red;
            temporaryColor.a = 1f;
        }

        else temporaryColor.a = opacity;

        crosshairImage.color = temporaryColor;
    }
}
