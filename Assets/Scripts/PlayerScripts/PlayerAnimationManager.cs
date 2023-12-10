using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerController pc;
    private Animator an;
    private CameraController cameraController;

    private bool isRangedAttackAnimationPlaying;
    private bool isHeadbuttAnimationPlaying;

    [Range(0,1)] public float jumpOrMovementValue;
    public float jumpBlendValueChangeTime = 1f;
    public float jumpAnimationTime = 1f;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
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
        }

        an.SetFloat("IdleOrMoving", MovingSpeedToIdleAndMoveBlendValue());
        an.SetFloat("WalkOrRun", MovingSpeedToWalkAndRunBlendValue());

        if (psd.isAiming)
        {
            an.SetFloat("ForwardOrBackward", SingleDimensionSpeedToSingleDimensionBlendValue(true));
            an.SetFloat("LeftOrRight", SingleDimensionSpeedToSingleDimensionBlendValue(false));
        }

        else
        {
            an.SetFloat("ForwardOrBackward", 1);
            an.SetFloat("LeftOrRight", 0);
        }

        an.SetLayerWeight(1, CameraFovToAimLayerWeight());
    }

    private async void InOutJumpBlendValue()
    {
        DOVirtual.Float(0, 1, jumpBlendValueChangeTime, value => jumpOrMovementValue = value).SetEase(Ease.Linear);
        await UniTask.WaitForSeconds(jumpAnimationTime);
        DOVirtual.Float(1, 0, jumpBlendValueChangeTime, value => jumpOrMovementValue = value).SetEase(Ease.Linear);
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