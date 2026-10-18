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

    public event Action<bool> OnInteract;

    private bool _isStoped;

    private bool _stopMovement;

    public InputHandler()
    {
        _actions = new InputSystem_Actions();
        
        _actions.Player.Move.performed += HandleMove;
        _actions.Player.Move.canceled  += HandleMove;
        
        _actions.Player.Look.performed += HandleLook;
        _actions.Player.Look.canceled += HandleLook;
        
        _actions.Player.Attack.performed += HandleCast;
        _actions.Player.Attack.canceled += HandleCast;
        
        _actions.Player.Interact.performed += HandleInteract;
        _actions.Player.Interact.canceled += HandleInteract;
        
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
        
        _actions.Player.Interact.performed -= HandleInteract;
        _actions.Player.Interact.canceled -= HandleInteract;
        _actions.Player.Disable();
        _currentMove = Vector2.zero;
      
    }

    public void Dispose() => Disable();

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        if(_isStoped || _stopMovement) return;
        
        Vector2 value = ctx.ReadValue<Vector2>();
        
        _currentMove = value;
      
    }

    private void HandleLook(InputAction.CallbackContext ctx)
    {
        if(_isStoped) return;
        
        Vector2 value = ctx.ReadValue<Vector2>();
        
        _lookVector = value;
    }

    private void HandleCast(InputAction.CallbackContext ctx)
    {
        if(_isStoped) return;
        
        bool click = ctx.ReadValue<float>() > 0.5f;
        
        OnCast?.Invoke(click);
    }

    private void HandleInteract(InputAction.CallbackContext ctx)
    {
        if(_isStoped) return;
        
        bool click = ctx.ReadValue<float>() > 0.5f;
        
        OnInteract?.Invoke(click);
    }

    public void SetIsStoped(bool isStoped)
    {
        if (isStoped)
        {
            _isStoped = isStoped;
            
            _currentMove = new Vector2(0, 0);

            _lookVector = new Vector2(0, 0);
        }
        else
        {
            _isStoped = isStoped;
        }
    }

    public void SetStopMovement(bool stopMovement)
    {
        if (stopMovement)
        {
            _stopMovement = stopMovement;
            _currentMove = new Vector2(0, 0);
        }
        else
        {
            _stopMovement = stopMovement;
        }
    }

    public Vector2 GetMovementDirection() => new(_currentMove.x, _currentMove.y);
    
    public Vector2 GetLookDirection() => new(_lookVector.x, _lookVector.y);
}
