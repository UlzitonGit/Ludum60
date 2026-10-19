using System;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera _camera;
    
    private InputHandler _inputHandler;
    
    private Vector3 _velocity;
    private float _pitch;
    private float _yaw;
    private Vector2 _smoothedLook;
    private Vector2 _lookVelocity;
    
   
    [Inject]
    public void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        
      
    }

    private void FixedUpdate()
    {
        Movement(_inputHandler.GetMovementDirection());
        HandleLook(_inputHandler.GetLookDirection());
    }

    private void Movement(Vector2 move)
    {
        Vector3 direction = transform.right * move.x + transform.forward * move.y;
        
        if (direction.sqrMagnitude > 1f) direction.Normalize();
        
        Vector3 displacement = direction * (settings.MovementSpeed * Time.deltaTime);
        
        _controller.Move(displacement);
    }
    
    private void HandleLook(Vector2 look)
    {

        _smoothedLook = Vector2.SmoothDamp(
            _smoothedLook, look, ref _lookVelocity,
            settings.LookSmoothing, Mathf.Infinity, Time.unscaledDeltaTime);

        _yaw   += _smoothedLook.x * settings.RotationSpeed;
        _pitch -= _smoothedLook.y * settings.RotationSpeed;
        _pitch  = Mathf.Clamp(_pitch, settings.MinPitch, settings.MaxPitch);

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        if (_camera != null)
            _camera.transform.localRotation = Quaternion.Euler(_pitch * PlayerPrefs.GetFloat("Sensa"), 0f, 0f);
    }
    
    
}
