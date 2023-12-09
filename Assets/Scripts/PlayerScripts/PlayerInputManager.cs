using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static float sensitivity;

    private PlayerInputActions pia;

    [Header("Info - No Touch")]
    public Vector2 lookInput;
    public Vector2 moveInput;

    public bool isCombatModeKeyDown;
    public float changeWeaponInput;

    public bool isRunKey;
    public bool isDashKeyDown;

    public bool isJumpKeyDown;
    public bool isJumpKeyUp;
    public bool isDescendKeyDown;
    public bool isDescendKeyUp;

    public bool isAimKeyDown;
    public bool isAimKeyUp;
    public bool isAttackKeyDown;
    public bool isAttackKey;

    public bool isGravityKeyDown;
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

        isCombatModeKeyDown = pia.Player.CombatMode.WasPressedThisFrame();
        changeWeaponInput = pia.Player.ChangeWeapon.ReadValue<float>();

        isRunKey = pia.Player.Run.IsPressed();
        isDashKeyDown = pia.Player.Dash.WasPressedThisFrame();

        isJumpKeyDown = pia.Player.Jump.WasPressedThisFrame();
        isJumpKeyUp = pia.Player.Jump.WasReleasedThisFrame();
        isDescendKeyDown = pia.Player.Descend.WasPressedThisFrame();
        isDescendKeyUp = pia.Player.Descend.WasReleasedThisFrame();

        isAimKeyDown = pia.Player.Aim.WasPressedThisFrame();
        isAimKeyUp = pia.Player.Aim.WasReleasedThisFrame();
        isAttackKeyDown = pia.Player.Attack.WasPressedThisFrame();
        isAttackKey = pia.Player.Attack.IsPressed();

        isGravityKeyDown = pia.Player.Gravity.WasPressedThisFrame();
        //isInteractKeyDown = pia.Player.Interact.WasPressedThisFrame();
    }
}