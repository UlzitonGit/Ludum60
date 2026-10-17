using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{
    private readonly InputSystem_Actions _actions;

    private Vector2 _currentMove;

    public Vector2 CurrentMove => _currentMove;

    private Vector2 _lookVector;
    
    public event Action<bool> OnCast;

    public InputHandler()
    {
        _actions = new InputSystem_Actions();
        
        _actions.Player.Move.performed += HandleMove;
        _actions.Player.Move.canceled  += HandleMove;
        _actions.Player.Look.performed += HandleLook;
        _actions.Player.Look.canceled += HandleLook;
        _actions.Player.Attack.performed += HandleCast;
        _actions.Player.Attack.canceled += HandleCast;
        _actions.Player.Enable();
    }

    public void Disable()
    {
        _actions.Player.Move.performed -= HandleMove;
        _actions.Player.Move.canceled  -= HandleMove;
        _actions.Player.Look.performed -= HandleLook;
        _actions.Player.Look.canceled -= HandleLook;
        _actions.Player.Attack.performed -= HandleCast;
        _actions.Player.Attack.canceled -= HandleCast;
        _actions.Player.Disable();
        _currentMove = Vector2.zero;
      
    }

    public void Dispose() => Disable();

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        
        _currentMove = value;
      
    }

    private void HandleLook(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        
        _lookVector = value;
    }

    private void HandleCast(InputAction.CallbackContext ctx)
    {
        bool click = ctx.ReadValue<float>() > 0.5f;
        
        OnCast?.Invoke(click);
    }

    public Vector2 GetMovementDirection() => new(_currentMove.x, _currentMove.y);
    
    public Vector2 GetLookDirection() => new(_lookVector.x, _lookVector.y);
}
