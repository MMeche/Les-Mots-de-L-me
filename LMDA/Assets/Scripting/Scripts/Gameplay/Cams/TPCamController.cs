using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TPCamController : MonoBehaviour
{
    [SerializeField] 
    float _zoomSpeed = 2f, 
          _zoomLerpSpeed = 2f, 
          _minDistance = 2f, 
          _maxDistance = 2f;

    PlayerInputs _inputs;

    CinemachineOrbitalFollow _orbitalFollow;

    float _currentZoom, _targetZoom, _zoomDelta;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _inputs = new PlayerInputs();
        _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        _currentZoom = _orbitalFollow.Radius;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Zoom.performed += HandleZoomingInput;
    }

    private void HandleZoomingInput(InputAction.CallbackContext ctx)
    {
        _zoomDelta = ctx.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        if (_zoomDelta != 0)
            HandleZooming();
        
    }

    private void HandleZooming()
    {
        if( _orbitalFollow != null )
        {
            _targetZoom = Mathf.Clamp(_orbitalFollow.Radius - _zoomDelta * _zoomSpeed, _minDistance, _maxDistance);
            _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.fixedDeltaTime * _zoomLerpSpeed);
            _orbitalFollow.Radius = _currentZoom;
            _zoomDelta = 0;
        }
    }
}
