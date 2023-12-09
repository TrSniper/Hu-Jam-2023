using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour, IDamageable
{

    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private int gunDamage = 5;

    [Header("Assign")] [SerializeField] private Transform gunLineOutTransform;

    [Header("Assign")]
    [SerializeField] private float rangedAttackCooldownTime = 1f;
    [SerializeField] private float aimModeSensitivityModifier = 0.5f;
    [SerializeField] private float knockBackAmount = 5f;
    [SerializeField] private float knockBackDuration = 0.2f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private CrosshairManager cm;
    //private PlayerCombatAudioManager pcam;

    private Camera mainCamera;
    private CameraController cameraController;
    //private LineRenderer gunLineRenderer;

    private bool isRangedAttackCooldownOver = true;
    private float rangedAttackAnimationTime;

    public event Action<int> OnHealthChanged;
    public static event Action OnPlayerDeath;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        cm = GetComponent<CrosshairManager>();
        //pcam = GetComponent<PlayerCombatAudioManager>();

        mainCamera = Camera.main;
        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
        //gunLineRenderer = gunLineOutTransform.GetComponent<LineRenderer>();

        //Default value
        //rangedAttackAnimationTime = pam.rangedAttackAnimationHalfDuration * 2;
    }

    private void Update()
    {
        if (pim.isAimKeyDown || pim.isAimKeyUp) ToggleAim();

        if (psd.isAiming && pim.isAttackKeyDown && !psd.isAttacking && isRangedAttackCooldownOver) Attack();

        //if (gunLineRenderer.enabled)
        //{
        //    gunLineRenderer.SetPosition(0, gunLineOutTransform.position);
        //    gunLineRenderer.SetPosition(1, GetMiddleOfTheScreen(cm.attackRange));
        //}
    }

    private void ToggleAim()
    {
        if (!psd.isAiming)
        {
            cameraController.ChangeCameraFov(CameraController.FovMode.AimFov);

            psd.isAiming = true;
            PlayerInputManager.sensitivity *= aimModeSensitivityModifier;

            //pcam.ToggleAimSound(true);
        }

        else
        {
            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);

            psd.isAiming = false;
            PlayerInputManager.sensitivity /= aimModeSensitivityModifier;

            //pcam.ToggleAimSound(true);
        }
    }

    private async void Attack()
    {
        psd.isAttacking = true;
        StartAttackCooldown();

        //gunLineRenderer.enabled = true;
        //pcam.ToggleAttackSound(true);

        cm.damageable?.GetDamage(gunDamage, transform.forward);
        await UniTask.WaitForSeconds(rangedAttackAnimationTime);

        //gunLineRenderer.enabled = false;
        psd.isAttacking = false;
    }

    private Vector3 GetMiddleOfTheScreen(float zValue)
    {
        Vector3 viewportMiddle = new Vector3(0.5f, 0.5f, zValue);
        return mainCamera.ViewportToWorldPoint(viewportMiddle);
    }

    private async void StartAttackCooldown()
    {
        isRangedAttackCooldownOver = false;
        await UniTask.WaitForSeconds(rangedAttackCooldownTime);
        isRangedAttackCooldownOver = true;
    }

    public async void GetDamage(int damageTakenAmount, Vector3 attackerTransformForward)
    {
        health -= damageTakenAmount;
        OnHealthChanged?.Invoke(health);
        if (CheckForDeath()) return;

        PlayKnockBackAnimation(attackerTransformForward);
        await UniTask.WaitForSeconds(knockBackDuration);
    }

    //TODO: NO REPETITION
    private async void PlayKnockBackAnimation(Vector3 attackerTransformForward)
    {
        float animationSpeed = knockBackAmount / knockBackDuration;
        float movingDistance = 0f;

        while (movingDistance < knockBackAmount)
        {
            movingDistance += Time.deltaTime * animationSpeed;

            Vector3 tempPosition = transform.position + attackerTransformForward * (Time.deltaTime * animationSpeed);
            transform.position = tempPosition;

            await UniTask.NextFrame();
        }
    }

    private bool CheckForDeath()
    {
        if (health <= 0)
        {
            //pcam.ToggleDeathSound(true);
            health = 10;
            OnHealthChanged?.Invoke(10);
            OnPlayerDeath?.Invoke();
            return true;
        }

        //else
        //pcam.ToggleGetHitSound(true);
        return false;
    }

    //This is stupid v2 2/3
    public Transform GetTransform()
    {
        return transform;
    }
}