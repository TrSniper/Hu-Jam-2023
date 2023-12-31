using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public static event Action OnPlayerAttack;
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDeath;

    [Header("Assign")]
    public int health = 10;
    [SerializeField] private float aimModeSensitivityModifier = 0.5f;
    [SerializeField] private float knockBackForce = 200f;
    [SerializeField] private float attackAnimationTime = 0.2f;

    [Header("Info - No Touch")] [SerializeField]
    private WeaponBase[] weapons;

    public WeaponBase currentWeapon;
    public WeaponBase previousWeapon;
    public int currentWeaponIndex;
    public int previousWeaponIndex;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerAimManager pam;
    private PlayerAnimationManager panim;
    private CrosshairManager cm;

    private Rigidbody rb;

    private PlayerCombatAudioManager pcam;
    private CameraController cameraController;

    private bool isAttackCooldownOver = true;

    //public bool isCombatMode;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        pam = GetComponent<PlayerAimManager>();
        panim = GetComponent<PlayerAnimationManager>();
        cm = GetComponent<CrosshairManager>();
        rb = GetComponent<Rigidbody>();
        pcam = GetComponent<PlayerCombatAudioManager>();
        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();

        //Avoids null refs and other stuff
        previousWeapon = weapons[0];
        currentWeapon = weapons[1];
        currentWeaponIndex = 1;
        currentWeapon.gameObject.SetActive(true);
        cm.ChangeCrosshairImage(currentWeaponIndex);
    }

    private void Update()
    {
        //if (pim.isCombatModeKeyDown)
        //{
        //    isCombatMode = !isCombatMode;
        //    if (psd.isAiming) ToggleAim();
        //}
        //if (!isCombatMode) return;

        if (psd.playerMainState != PlayerStateData.PlayerMainState.Normal) return;

        if (!psd.isAiming)
        {
            if (pim.changeWeaponInput > 0) ChangeWeapon(true);
            else if (pim.changeWeaponInput < 0) ChangeWeapon(false);
        }

        if (pim.isAimKeyDown || pim.isAimKeyUp) ToggleAim();
        if (psd.isAiming && pim.isAttackKey && isAttackCooldownOver && currentWeapon.canAutoFire) Attack();
        else if (psd.isAiming && pim.isAttackKeyDown && isAttackCooldownOver) Attack();
    }

    private void ChangeWeapon(bool isNextWeaponWillBeSelected)
    {
        previousWeapon = currentWeapon;
        previousWeaponIndex = currentWeaponIndex;

        if (isNextWeaponWillBeSelected)
        {
            for (int i = currentWeaponIndex + 1; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    currentWeaponIndex = i;
                    currentWeapon = weapons[i];
                    break;
                }
            }
        }

        else
        {
            for (int i = currentWeaponIndex - 1; i >= 0; i--)
            {
                if (weapons[i] != null)
                {
                    currentWeaponIndex = i;
                    currentWeapon = weapons[i];
                    break;
                }
            }
        }

        panim.PlayWeaponChangeAnimation();
        cm.ChangeCrosshairImage(currentWeaponIndex);
    }

    public void ToggleAim()
    {
        if (!psd.isAiming)
        {
            cameraController.ChangeCameraFov(CameraController.FovMode.AimFov);

            psd.isAiming = true;
            PlayerInputManager.sensitivity *= aimModeSensitivityModifier;

            pcam.ToggleAimSound(true);
        }

        else
        {
            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);
            psd.isAiming = false;
            PlayerInputManager.sensitivity /= aimModeSensitivityModifier;

            pcam.ToggleAimSound(true);
        }
    }

    private async void Attack()
    {
        psd.isAttacking = true;
        StartAttackCooldown();

        OnPlayerAttack?.Invoke();
        currentWeapon.Attack();

        if (!currentWeapon.isLaser)
        {
            pcam.ToggleProjectileSound(true);
            PlayKnockBackAnimation();
        }

        if (currentWeapon.isLaser)
        {
            pcam.ToggleLaserAttackSound(true);
            if (pam.enemy != null) pam.enemy.GetDamage(currentWeapon.damage);
        }

        await UniTask.WaitForSeconds(attackAnimationTime);
        psd.isAttacking = false;
    }

    private async void StartAttackCooldown()
    {
        isAttackCooldownOver = false;
        await UniTask.WaitForSeconds(currentWeapon.cooldownTime);
        isAttackCooldownOver = true;
    }

    public void GetDamage(int damageTakenAmount)
    {
        health -= damageTakenAmount;
        OnHealthChanged?.Invoke(health);
        CheckForDeath();
    }

    private void PlayKnockBackAnimation()
    {
        float knock = knockBackForce;
        if (!GravityManager.isGravityActive) knock *= 10;
        rb.AddForce(-transform.forward * knock);
    }

    private void CheckForDeath()
    {
        if (health <= 0)
        {
            pcam.ToggleDeathSound(true);
            health = 10;
            OnHealthChanged?.Invoke(10);
            OnPlayerDeath?.Invoke();
        }

        //else
        //pcam.ToggleGetHitSound(true);
    }
}