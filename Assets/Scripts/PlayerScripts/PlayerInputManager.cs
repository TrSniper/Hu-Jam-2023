using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static float sensitivity;

    private PlayerInputActions pia;

    [Header("Info - No Touch")]
    public Vector2 lookInput;
    public Vector2 moveInput;
    public bool isJumpKeyDown;
    public bool isRunKey;
    public bool isRunKeyDown;
    public bool isRunKeyUp;
    public bool isAimKeyDown;
    public bool isAimKeyUp;
    public bool isAttackKeyDown;
    public bool isInteractKeyDown;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Player.Enable();

        sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    private void Update()
    {
        lookInput = pia.Player.Look.ReadValue<Vector2>() * sensitivity;
        lookInput.y *= -1; //Fixes y axis inversion
        moveInput = pia.Player.Movement.ReadValue<Vector2>();

        //isJumpKeyDown = pia.Player.Jump.WasPressedThisFrame();

        //isRunKey = pia.Player.Run.IsPressed();
        //isRunKeyDown = pia.Player.Run.WasPressedThisFrame();
        //isRunKeyUp = pia.Player.Run.WasReleasedThisFrame();

        //isAimKeyDown = pia.Player.Aim.WasPressedThisFrame();
        //isAimKeyUp = pia.Player.Aim.WasReleasedThisFrame();
        //isAttackKeyDown = pia.Player.Attack.WasPressedThisFrame();

        //isInteractKeyDown = pia.Player.Interact.WasPressedThisFrame();
    }
}