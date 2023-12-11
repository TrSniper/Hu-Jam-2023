using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerCombatManager pcm;
    private PlayerController pc;
    private Animator an;
    private CameraController cameraController;

    [Header("Assign")] [SerializeField] private float weaponChangeTime = 0.2f;
    private Tweener rifleCarryLayerWeightTween;
    public float rifleCarryLayerWeight;

    //[Range(0,1)] public float jumpOrMovementValue;
    //public float jumpBlendValueChangeTime = 1f;
    //public float jumpAnimationTime = 1f;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pcm = GetComponent<PlayerCombatManager>();
        pc = GetComponent<PlayerController>();
        an = transform.GetComponentInChildren<Animator>();
        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
    }

    private void Update()
    {
        //if (pim.isJumpKeyDown) InOutJumpBlendValue();
        //an.SetFloat("JumpOrMovement", jumpOrMovementValue);

        if (psd.isJumping && GravityManager.isGravityActive)
        {
            an.Play("Jump");
        }

        else if (psd.isIdle || psd.isMoving)
        {
            if (GravityManager.isGravityActive) an.Play("IdleOrMoving");
            else an.Play("ZeroGravityIdleOrMoving");

            an.SetFloat("IdleOrMoving", MovingSpeedToIdleAndMoveBlendValue());
            an.SetFloat("WalkOrRun", MovingSpeedToWalkAndRunBlendValue());
        }

        if (psd.isAiming)
        {
            an.SetFloat("ForwardOrBackward", SingleDimensionSpeedToSingleDimensionBlendValue(true));
            an.SetFloat("LeftOrRight", SingleDimensionSpeedToSingleDimensionBlendValue(false));

            if (pcm.currentWeaponIndex is 0 or 1) an.Play("AimPistol", 2);
            else if (pcm.currentWeaponIndex is 2 or 3) an.Play("AimRifle", 2);
        }

        else
        {
            an.SetFloat("ForwardOrBackward", 1);
            an.SetFloat("LeftOrRight", 0);
        }

        if (psd.isAttacking)
        {
            if (pcm.currentWeaponIndex is 0 or 1) an.Play("AttackPistol", 3);
            else if (pcm.currentWeaponIndex is 2 or 3) an.Play("AttackRifle", 3);
        }

        an.SetLayerWeight(1, rifleCarryLayerWeight);
        an.SetLayerWeight(2, CameraFovToAimLayerWeight());
    }

    public async void PlayWeaponChangeAnimation()
    {
        //TODO: better grouping

        //Pistol to pistol
        if (pcm.currentWeaponIndex is 0 or 1 && pcm.previousWeaponIndex is 0 or 1)
        {
            pcm.previousWeapon.gameObject.SetActive(false);
            pcm.currentWeapon.gameObject.SetActive(true);

            return;
        }

        rifleCarryLayerWeightTween?.Kill();

        //Rifle to pistol
        if (pcm.currentWeaponIndex is 0 or 1 && pcm.previousWeaponIndex is 2 or 3)
        {
            rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 0, weaponChangeTime,
                value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);

            await UniTask.WaitForSeconds(weaponChangeTime);
            pcm.previousWeapon.gameObject.SetActive(false);
            pcm.currentWeapon.gameObject.SetActive(true);

            return;
        }

        //Pistol to rifle
        if (pcm.currentWeaponIndex is 2 or 3 && pcm.previousWeaponIndex is 0 or 1)
        {
            pcm.previousWeapon.gameObject.SetActive(false);
            pcm.currentWeapon.gameObject.SetActive(true);

            rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 1, weaponChangeTime,
                value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);

            await UniTask.WaitForSeconds(weaponChangeTime);
            return;
        }

        //Rifle to rifle
        if (pcm.currentWeaponIndex is 2 or 3 && pcm.previousWeaponIndex is 2 or 3)
        {
            Debug.Log("1");
            rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 0, weaponChangeTime,
                value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);

            await UniTask.WaitForSeconds(weaponChangeTime);
            pcm.previousWeapon.gameObject.SetActive(false);
            pcm.currentWeapon.gameObject.SetActive(true);

            Debug.Log("2");
            rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 1, weaponChangeTime,
                value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);
        }
    }

    private void ChangeRifleCarryLayerWeight(bool isIncreasing)
    {
        rifleCarryLayerWeightTween?.Kill();

        if (isIncreasing) rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 1, weaponChangeTime,
            value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);

        else rifleCarryLayerWeightTween = DOVirtual.Float(rifleCarryLayerWeight, 0, weaponChangeTime,
            value => rifleCarryLayerWeight = value).SetEase(Ease.Linear);
    }

    private async void InOutJumpBlendValue()
    {
        //DOVirtual.Float(0, 1, jumpBlendValueChangeTime, value => jumpOrMovementValue = value).SetEase(Ease.Linear);
        //await UniTask.WaitForSeconds(jumpAnimationTime);
        //DOVirtual.Float(1, 0, jumpBlendValueChangeTime, value => jumpOrMovementValue = value).SetEase(Ease.Linear);
    }

    private float SingleDimensionSpeedToSingleDimensionBlendValue(bool isForwardBackward)
    {
        float speed;
        if (isForwardBackward) speed = pc.forwardBackwardSpeed;
        else speed = pc.leftRightSpeed;

        if (speed > 0) return speed / pc.walkingSpeed;
        else if (speed < 0) return speed / pc.walkingSpeed;
        else return 0;
    }

    private float MovingSpeedToWalkAndRunBlendValue()
    {
        float movingSpeed = new Vector2(pc.leftRightSpeed, pc.forwardBackwardSpeed).magnitude;

        if (movingSpeed < pc.walkingSpeed) return 0;
        return (movingSpeed - pc.walkingSpeed) / (pc.runningSpeed - pc.walkingSpeed);
    }

    private float MovingSpeedToIdleAndMoveBlendValue()
    {
        float movingSpeed = new Vector2(pc.leftRightSpeed, pc.forwardBackwardSpeed).magnitude;

        return movingSpeed / pc.walkingSpeed;
    }

    private float CameraFovToAimLayerWeight()
    {
        return 1 - ((cameraController.cam.m_Lens.FieldOfView - cameraController.aimFovValue) /
                    (cameraController.defaultFovValue - cameraController.aimFovValue));
    }
}