using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private PlayerControls playerControls;

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool IsPlayerJumped()
    {
        return playerControls.Player.Jump.ReadValue<float>() > 0f;
        //return playerControls.Player.Jump.triggered;
    }

    public bool IsPlayerAttack()
    {
        return playerControls.Player.Attack.ReadValue<float>() > 0f;
    }

    public bool IsPlayerAttackSecondary()
    {
        return playerControls.Player.Attack2.ReadValue<float>() > 0f;
    }

    public bool IsPlayerReloading()
    {
        return playerControls.Player.Reload.triggered;
    }

    public bool IsPlayerTimeSlow()
    {
        return playerControls.Player.TimeSlow.triggered;
    }

    public bool IsPlayerSwitchGun1()
    {
        return playerControls.Player.SwitchGun1.triggered;
    }

    public bool IsPlayerSwitchGun2()
    {
        return playerControls.Player.SwitchGun2.triggered;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
