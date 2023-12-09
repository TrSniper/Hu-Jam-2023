using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour, IDamageable
{
    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private float aimModeSensitivityModifier = 0.5f;
    [SerializeField] private float knockBackDuration = 0.2f;
    [SerializeField] private float attackAnimationTime = 0.2f;

    [Header("Info - No Touch")]
    [SerializeField] private WeaponBase[] weapons;
    [SerializeField] private WeaponBase currentWeapon;
    [SerializeField] private int currentWeaponIndex;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerAimManager pam;
    private CrosshairManager cm;
    //private PlayerCombatAudioManager pcam;
    private CameraController cameraController;

    private bool isAttackCooldownOver = true;

    //public bool isCombatMode;

    public event Action<int> OnHealthChanged;
    public static event Action OnPlayerDeath;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        pam = GetComponent<PlayerAimManager>();
        cm = GetComponent<CrosshairManager>();
        //pcam = GetComponent<PlayerCombatAudioManager>();
        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();

        ChangeWeapon(true);
    }

    private void Update()
    {
        //if (pim.isCombatModeKeyDown)
        //{
        //    isCombatMode = !isCombatMode;
        //    if (psd.isAiming) ToggleAim();
        //}
        //if (!isCombatMode) return;

        if (pim.changeWeaponInput > 0) ChangeWeapon(true);
        else if (pim.changeWeaponInput < 0) ChangeWeapon(false);

        if (pim.isAimKeyDown || pim.isAimKeyUp) ToggleAim();
        if (psd.isAiming && pim.isAttackKey && isAttackCooldownOver && currentWeapon.canAutoFire) Attack();
        else if (psd.isAiming && pim.isAttackKeyDown && isAttackCooldownOver) Attack();
    }

    private void ChangeWeapon(bool isNextWeaponWillBeSelected)
    {
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

        cm.ChangeCrosshairImage(currentWeaponIndex);
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

        //pcam.ToggleAttackSound(true);
        pam.damageable?.GetDamage(currentWeapon.damage, transform.forward);
        currentWeapon.Attack();
        if (currentWeapon.isLaser) PlayKnockBackAnimation(-transform.forward);

        await UniTask.WaitForSeconds(attackAnimationTime);

        psd.isAttacking = false;
    }

    private async void StartAttackCooldown()
    {
        isAttackCooldownOver = false;
        await UniTask.WaitForSeconds(currentWeapon.cooldownTime);
        isAttackCooldownOver = true;
    }

    public async void GetDamage(int damageTakenAmount, Vector3 attackerTransformForward)
    {
        health -= damageTakenAmount;
        OnHealthChanged?.Invoke(health);
        if (CheckForDeath()) return;

        PlayKnockBackAnimation(attackerTransformForward);
        await UniTask.WaitForSeconds(knockBackDuration);
    }

    private void PlayKnockBackAnimation(Vector3 attackerTransformForward)
    {

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