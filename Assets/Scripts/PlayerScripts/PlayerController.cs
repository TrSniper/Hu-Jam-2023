using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float rotatingSpeed = 0.1f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private float zeroGravityAcceleration = 4f;

    [Header("Info - No Touch")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float verticalSpeedReal;
    private Vector3 movingDirection;

    private Rigidbody rb;
    private PlayerStateData psd;
    private PlayerInputManager pim;

    private CameraController cameraController;
    private Transform cameraTransform;

    private IEnumerator walkRunSpeedRoutine;
    private IEnumerator verticalSpeedRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();

        cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
        cameraTransform = cameraController.transform;

        //Default Value
        movingSpeed = walkingSpeed;
        verticalSpeedReal = 0;
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

            if (walkRunSpeedRoutine != null) StopCoroutine(walkRunSpeedRoutine);
            walkRunSpeedRoutine = ChangeMovingSpeed(true, runningSpeed);
            StartCoroutine(walkRunSpeedRoutine);
        }

        //Running or idle to walking
        else if (psd.isMoving && !pim.isRunKey && !psd.isWalking && !psd.isAiming)
        {
            //From running
            if (psd.isRunning)
            {
                if (walkRunSpeedRoutine != null) StopCoroutine(walkRunSpeedRoutine);
                walkRunSpeedRoutine = ChangeMovingSpeed(false, walkingSpeed);
                StartCoroutine(walkRunSpeedRoutine);
            }

            //From idle
            else
            {
                if (walkRunSpeedRoutine != null) StopCoroutine(walkRunSpeedRoutine);
                walkRunSpeedRoutine = ChangeMovingSpeed(true, walkingSpeed);
                StartCoroutine(walkRunSpeedRoutine);
            }

            psd.isRunning = false;
            psd.isWalking = true;

            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);
        }

        //Moving to idle
        else if (!psd.isMoving)
        {
            psd.isRunning = false;
            psd.isWalking = false;

            cameraController.ChangeCameraFov(CameraController.FovMode.DefaultFov);

            if (walkRunSpeedRoutine != null) StopCoroutine(walkRunSpeedRoutine);
            walkRunSpeedRoutine = ChangeMovingSpeed(false, 0f);
            StartCoroutine(walkRunSpeedRoutine);
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
                if (verticalSpeedRoutine != null) StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(true, verticalSpeed);
                StartCoroutine(verticalSpeedRoutine);

                psd.isAscending = true;
                psd.isDescending = false;
            }

            else if (pim.isJumpKeyUp)
            {
                if (verticalSpeedRoutine != null) StopCoroutine(verticalSpeedRoutine);
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
                if (verticalSpeedRoutine != null) StopCoroutine(verticalSpeedRoutine);
                verticalSpeedRoutine = ChangeVerticalSpeed(false, -verticalSpeed);
                StartCoroutine(verticalSpeedRoutine);

                psd.isAscending = false;
                psd.isDescending = true;
            }

            else if (pim.isDescendKeyUp)
            {
                if (verticalSpeedRoutine != null) StopCoroutine(verticalSpeedRoutine);
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

        rb.velocity = new Vector3(movingDirection.x * movingSpeed, rb.velocity.y, movingDirection.z * movingSpeed);
    }

    private void HandleRotation()
    {
        if (psd.isAttacking) return;

        if (psd.isAiming)
        {
            transform.forward = Vector3.Slerp(transform.forward, cameraTransform.forward, rotatingSpeed * 2);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        else if (psd.isMoving) transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed);
    }

    private IEnumerator ChangeMovingSpeed(bool isIncreasing, float movingSpeedToReach)
    {
        if (isIncreasing)
        {
            while (movingSpeed < movingSpeedToReach)
            {
                if (GravityManager.isGravityActive) movingSpeed += acceleration * Time.deltaTime;
                else movingSpeed += zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
        }

        else
        {
            while (movingSpeed > movingSpeedToReach)
            {
                if (GravityManager.isGravityActive) movingSpeed -= deceleration * Time.deltaTime;
                else movingSpeed -= zeroGravityAcceleration * Time.deltaTime;
                yield return null;
            }
        }

        movingSpeed = movingSpeedToReach;
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
}