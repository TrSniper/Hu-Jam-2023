using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Assign")]
    public float walkingSpeed = 5f;
    public float runningSpeed = 8f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float rotatingSpeed = 0.1f;
    [SerializeField] private float zeroGravityRotatingSpeed = 0.01f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private float zeroGravityAcceleration = 4f;

    [Header("Info - No Touch")]
    public float leftRightSpeed;
    public float forwardBackwardSpeed;
    [SerializeField] private float verticalSpeedReal;
    private Vector3 movingDirection;

    private Rigidbody rb;
    private PlayerStateData psd;
    private PlayerInputManager pim;

    private CameraController cameraController;
    private Transform cameraTransform;

    private IEnumerator walkRunSpeedRoutine;
    private IEnumerator leftRightSpeedRoutine;
    private IEnumerator forwardBackwardSpeedRoutine;
    private IEnumerator verticalSpeedRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();

        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
        cameraTransform = cameraController.transform;

        //Default Values
        forwardBackwardSpeed = walkingSpeed;
        leftRightSpeed = walkingSpeed;
        verticalSpeedReal = 0;

        //Defaults
        leftRightSpeedRoutine = ChangeLeftRightSpeed(0f);
        forwardBackwardSpeedRoutine = ChangeForwardBackwardSpeed(0f);
        verticalSpeedRoutine = ChangeVerticalSpeed(true, 0f);
    }

    private void Update()
    {
        if (psd.playerMainState != PlayerStateData.PlayerMainState.Normal) return;

        DecideIdleOrMoving();
        DecideWalkingOrRunning();

        ControlGravity();

        HandleJump();

        HandleAscend();
        HandleDescent();
        HandleVerticalMovement();

        HandleRotation();
        HandleMovementSpeeds();
    }

    private void FixedUpdate()
    {
        if (psd.playerMainState != PlayerStateData.PlayerMainState.Normal) return;

        HandleMovement();
    }

    private void DecideIdleOrMoving()
    {
        psd.isMoving = pim.moveInput != Vector2.zero;
        psd.isIdle = !psd.isMoving;
    }

    private void DecideWalkingOrRunning()
    {
        //Idle or walking to running
        if (psd.isMoving && pim.isRunKey && !psd.isRunning && !psd.isAiming)
        {
            psd.isRunning = true;
            psd.isWalking = false;

            cameraController.ChangeCameraFov(CameraController.FovMode.RunningFov);
        }

        //Running or idle to walking
        else if (psd.isMoving && !pim.isRunKey && !psd.isWalking && !psd.isAiming)
        {
            psd.isRunning = false;
            psd.isWalking = true;

            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);
        }

        //Moving to idle
        else if (!psd.isMoving && !psd.isAiming && (psd.isRunning || psd.isWalking))
        {
            psd.isRunning = false;
            psd.isWalking = false;

            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);
        }

        //TODO: better fix
        if (psd.isAiming && psd.isMoving)
        {
            psd.isRunning = false;
            psd.isWalking = true;
        }
    }

    private void HandleMovementSpeeds()
    {
        float movingSpeed;
        if (psd.isIdle) movingSpeed = 0f;
        else if (psd.isWalking) movingSpeed = walkingSpeed;
        else movingSpeed = runningSpeed;

        //TODO: shit

        if ((pim.moveInput.x > 0 && leftRightSpeed != movingSpeed && !isChangeLeftRightSpeedRunningIncreasing) ||
            (pim.moveInput.x < 0 && leftRightSpeed != -movingSpeed && !isChangeLeftRightSpeedRunningDecreasing) ||
            (pim.moveInput.x == 0 && leftRightSpeed != 0f && !isChangeLeftRightSpeedRunningDecreasing))
        {
            StopCoroutine(leftRightSpeedRoutine);
            isChangeLeftRightSpeedRunningIncreasing = false;
            isChangeLeftRightSpeedRunningDecreasing = false;

            leftRightSpeedRoutine = ChangeLeftRightSpeed(movingSpeed * pim.moveInput.x);
            StartCoroutine(leftRightSpeedRoutine);
        }

        if ((pim.moveInput.y > 0 && forwardBackwardSpeed != movingSpeed && !isChangeForwardBackwardSpeedRunningIncreasing) ||
            (pim.moveInput.y < 0 && forwardBackwardSpeed != -movingSpeed && !isChangeForwardBackwardSpeedRunningDecreasing) ||
            (pim.moveInput.y == 0 && forwardBackwardSpeed != 0f && !isChangeForwardBackwardSpeedRunningDecreasing))
        {
            StopCoroutine(forwardBackwardSpeedRoutine);
            isChangeForwardBackwardSpeedRunningIncreasing = false;
            isChangeForwardBackwardSpeedRunningDecreasing = false;

            forwardBackwardSpeedRoutine = ChangeForwardBackwardSpeed(movingSpeed * pim.moveInput.y);
            StartCoroutine(forwardBackwardSpeedRoutine);
        }
    }

    private void ControlGravity()
    {
        if (pim.isGravityKeyDown && !GravityManager.isGravityActive)
        {
            rb.useGravity = true;
            GravityManager.ActivateGravity();
        }
        else if (pim.isGravityKeyDown && GravityManager.isGravityActive)
        {
            rb.useGravity = false;
            GravityManager.DeactivateGravity();
        }
    }

    private void HandleJump()
    {
        if (GravityManager.isGravityActive && psd.isGrounded && pim.isJumpKeyDown)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            psd.isJumping = true;
        }
    }

    private void HandleAscend()
    {
        if (!GravityManager.isGravityActive)
        {
            if (pim.isJumpKeyDown)
            {
                StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(true, verticalSpeed);
                StartCoroutine(verticalSpeedRoutine);

                psd.isAscending = true;
                psd.isDescending = false;
            }

            else if (pim.isJumpKeyUp)
            {
                StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(false, 0);
                StartCoroutine(verticalSpeedRoutine);

                psd.isAscending = false;
            }
        }
    }

    private void HandleDescent()
    {
        if (!GravityManager.isGravityActive)
        {
            if (pim.isDescendKeyDown)
            {
                StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(false, -verticalSpeed);
                StartCoroutine(verticalSpeedRoutine);

                psd.isAscending = false;
                psd.isDescending = true;
            }

            else if (pim.isDescendKeyUp)
            {
                StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(true, 0);
                StartCoroutine(verticalSpeedRoutine);

                psd.isDescending = false;
            }
        }
    }

    private void HandleVerticalMovement()
    {
        if (!GravityManager.isGravityActive) rb.velocity = new Vector3(rb.velocity.x,  verticalSpeedReal, rb.velocity.z);
    }

    private void HandleMovement()
    {
        if (pim.moveInput != Vector2.zero)
        {
            movingDirection = cameraTransform.right * pim.moveInput.x + cameraTransform.forward * pim.moveInput.y;
            movingDirection.y = 0f;
        }

        Vector3 velocity = cameraTransform.right * leftRightSpeed + cameraTransform.forward * forwardBackwardSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void HandleRotation()
    {
        if (psd.isAttacking) return;

        if (psd.isAiming)
        {
            transform.forward = Vector3.Slerp(transform.forward, cameraTransform.forward, rotatingSpeed * 2);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        else if (psd.isMoving && GravityManager.isGravityActive) transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed);
        else if (psd.isMoving && !GravityManager.isGravityActive) transform.forward = Vector3.Slerp(transform.forward, movingDirection, zeroGravityRotatingSpeed);
    }

    #region ChangeSpeed

    [Header("Info - No Touch")]
    [SerializeField] private bool isChangeLeftRightSpeedRunningIncreasing;
    [SerializeField] private bool isChangeLeftRightSpeedRunningDecreasing;
    [SerializeField] private bool isChangeForwardBackwardSpeedRunningIncreasing;
    [SerializeField] private bool isChangeForwardBackwardSpeedRunningDecreasing;

    private IEnumerator ChangeLeftRightSpeed(float leftRightSpeedToReach)
    {
        //Debug.Log("left right: " + (forwardBackwardSpeed < leftRightSpeedToReach));
        if (leftRightSpeed < leftRightSpeedToReach)
        {
            isChangeLeftRightSpeedRunningIncreasing = true;
            while (leftRightSpeed < leftRightSpeedToReach)
            {
                if (GravityManager.isGravityActive) leftRightSpeed += acceleration * Time.deltaTime;
                else leftRightSpeed += zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
            isChangeLeftRightSpeedRunningIncreasing = false;
        }

        else
        {
            isChangeLeftRightSpeedRunningDecreasing = true;
            while (leftRightSpeed > leftRightSpeedToReach)
            {
                if (GravityManager.isGravityActive) leftRightSpeed -= deceleration * Time.deltaTime;
                else leftRightSpeed -= zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
            isChangeLeftRightSpeedRunningDecreasing = false;
        }

        leftRightSpeed = leftRightSpeedToReach;
    }

    private IEnumerator ChangeForwardBackwardSpeed(float forwardBackwardSpeedToReach)
    {
        //Debug.Log("forward backward: " + (forwardBackwardSpeed < forwardBackwardSpeedToReach));
        if (forwardBackwardSpeed < forwardBackwardSpeedToReach)
        {
            isChangeForwardBackwardSpeedRunningIncreasing = true;
            while (forwardBackwardSpeed < forwardBackwardSpeedToReach)
            {
                if (GravityManager.isGravityActive) forwardBackwardSpeed += acceleration * Time.deltaTime;
                else forwardBackwardSpeed += zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
            isChangeForwardBackwardSpeedRunningIncreasing = false;
        }

        else
        {
            isChangeForwardBackwardSpeedRunningDecreasing = true;
            while (forwardBackwardSpeed > forwardBackwardSpeedToReach)
            {
                if (GravityManager.isGravityActive) forwardBackwardSpeed -= deceleration * Time.deltaTime;
                else forwardBackwardSpeed -= zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
            isChangeForwardBackwardSpeedRunningDecreasing = false;
        }

        forwardBackwardSpeed = forwardBackwardSpeedToReach;
    }

    private IEnumerator ChangeVerticalSpeed(bool isIncreasing, float verticalSpeedToReach)
    {
        if (isIncreasing)
        {
            while (verticalSpeedReal < verticalSpeedToReach)
            {
                verticalSpeedReal += zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
        }

        else
        {
            while (verticalSpeedReal > verticalSpeedToReach)
            {
                verticalSpeedReal -= zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
        }

        verticalSpeedReal = verticalSpeedToReach;
    }

    #endregion
}