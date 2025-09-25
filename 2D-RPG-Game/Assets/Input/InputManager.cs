using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static event Action OnJump;
    public static event Action OnRestart;
    public static event Action OnDash;
    public static event Action OnLeftMouseButton;
    private PlayerInputSystem _input;

    void Awake()
    {
        _input = new PlayerInputSystem();
    }

    public Vector2 MovementVector()
    {
        return _input.Player.Move.ReadValue<Vector2>();
    }
    private void OnJumpEvent(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }


    void OnEnable()
    {
        _input.Enable();
        _input.Player.Jump.performed += OnJumpEvent;
        _input.Player.Restart.performed += OnRestartButtonEvent;
        _input.Player.Dash.performed += OnDashEvent;
        _input.Player.Attack.performed += OnAttackEvent;
    }

    private void OnAttackEvent(InputAction.CallbackContext context)
    {
        OnLeftMouseButton?.Invoke();
    }

    private void OnDashEvent(InputAction.CallbackContext context)
    {
        OnDash?.Invoke();
    }

    private void OnRestartButtonEvent(InputAction.CallbackContext context)
    {
        OnRestart?.Invoke();
    }

    void OnDisable()
    {
        _input.Disable();
        _input.Player.Jump.performed -= OnJumpEvent;
    }

}
