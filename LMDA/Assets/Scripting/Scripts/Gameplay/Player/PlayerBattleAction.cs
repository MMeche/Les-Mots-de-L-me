using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBattleAction : MonoBehaviour
{
    [SerializeField] float _poeticStrafeDistance = 2.0f;
    [SerializeField] float _basePoeticStrafeSpeed = 10f;
    float _poeticStrafeSpeed = 2.0f;

    BattleFlowStateMachine _battleFlowStateMachine;
    Rigidbody _rb;
    PlayerInputs _playerInputs;
    float _strafeDirection;
    Vector3[] _poeticAnchors = new Vector3[4];
    int _currentPoeticAnchorIndex = 2;
    int _currentTargetAnchors;
    Vector3 _initialPosition;
    Vector3 _poeticVelocity = Vector3.zero;
    BattleInputManager _battleInputManager;
    BattleManager _battleManager;
    PauseManager _pauseManager;

    void OnEnable()
    {   
        _rb = GetComponent<Rigidbody>();
        

        _pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();

        _battleInputManager = ServiceProvider.Instance.ServiceLocator.Get<BattleInputManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _playerInputs = _battleInputManager.PlayerInputs;
        _playerInputs.Poetic.Move.performed += UpdateStrafe;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += PoeticMovementEnable;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += PoeticMovementDisable;
        

        _playerInputs.Poetic.Menu.started += PauseMenu;
        _playerInputs.Poetic.Debug.started += DebugMenu;
        _playerInputs.Lyrical.Menu.started += PauseMenu;
        _playerInputs.Lyrical.Debug.started += DebugMenu;
        _playerInputs.Narrative.Menu.started += PauseMenu;
        _playerInputs.Narrative.Debug.started += DebugMenu;
        _playerInputs.Argumentative.Menu.started += PauseMenu;
        _playerInputs.Argumentative.Debug.started += DebugMenu;

        

        _initialPosition = transform.position;
    }
    

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateStrafe(InputAction.CallbackContext ctx)
    {
        if (_strafeDirection != 0)
        {
            //Cancel the current move if we move the other way
            if (_strafeDirection * (-ctx.ReadValue<Vector2>().x) < 0) _currentPoeticAnchorIndex = _currentTargetAnchors;
            //    //Move to the next if we spam the same direction
            //    else _currentPoeticAnchorIndex = _currentTargetAnchors -1 * (int)(_strafeDirection/Mathf.Abs(_strafeDirection));
        }
        if(_currentTargetAnchors>0 && _currentTargetAnchors<3)_currentPoeticAnchorIndex = _currentTargetAnchors;

        _strafeDirection = -ctx.ReadValue<Vector2>().x;
    }

    

    private void MovePlayer()
    {
        //POETIC MOVEMENT SECTION
        if (_strafeDirection != 0)
        {
            Vector3 target = _rb.position;

            if (_strafeDirection > 0 && _currentPoeticAnchorIndex <= _poeticAnchors.Length - 1)
            {
                _currentTargetAnchors = Mathf.Clamp(_currentPoeticAnchorIndex + 1,0,3);
                target = _poeticAnchors[_currentTargetAnchors];

            }
            else if (_strafeDirection < 0 && _currentPoeticAnchorIndex >= 0)
            {
                _currentTargetAnchors = Mathf.Clamp(_currentPoeticAnchorIndex - 1, 0, 3);
                target = _poeticAnchors[_currentTargetAnchors];

            }

            // Checking if we rare close enough
            if (Vector3.Distance(_rb.position, target) < 0.2f)
            {
                transform.position = target; // snap to target position
                _poeticVelocity = Vector3.zero;
                _currentPoeticAnchorIndex = _currentTargetAnchors;
                _strafeDirection = 0;
            }

            Vector3 move = Vector3.SmoothDamp(_rb.position, target, ref _poeticVelocity, _poeticStrafeDistance / _poeticStrafeSpeed);
            transform.position = move;
        }
    }

    private void PoeticMovementEnable(BattleFlowStateMachineEventContext obj)
    {
        _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        _poeticStrafeSpeed = _basePoeticStrafeSpeed * (1 + 0.2f * _battleManager.GetActivePlayerStatus().Stats[StatType.Poetic]);
        for (int i = 0; i < 4; i++)
        {
            _poeticAnchors[i] = transform.position + new Vector3(0, 0, (i-2) * _poeticStrafeDistance + 3.5f);
        }
        _currentTargetAnchors = 2;
        transform.position = _poeticAnchors[_currentPoeticAnchorIndex];
    }

    private void PoeticMovementDisable(BattleFlowStateMachineEventContext obj)
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _strafeDirection = 0;
        transform.position = _initialPosition;
        _currentPoeticAnchorIndex = 2 ;
    }

    private void PauseMenu(InputAction.CallbackContext ctx)
    {
        if (!_pauseManager.IsPaused) _pauseManager.PauseGame();
    }

    

    private void DebugMenu(InputAction.CallbackContext ctx)
    {
        if (!_pauseManager.IsPaused) _pauseManager.DebugPause();
    }

    private void OnDisable()
    {
        _playerInputs.Poetic.Move.performed -= UpdateStrafe;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame -= PoeticMovementEnable;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= PoeticMovementDisable;

        _playerInputs.Poetic.Menu.started -= PauseMenu;
        _playerInputs.Poetic.Debug.started -= DebugMenu;
        _playerInputs.Lyrical.Menu.started -= PauseMenu;
        _playerInputs.Lyrical.Debug.started -= DebugMenu;
        _playerInputs.Narrative.Menu.started -= PauseMenu;
        _playerInputs.Narrative.Debug.started -= DebugMenu;
        _playerInputs.Argumentative.Menu.started -= PauseMenu;
        _playerInputs.Argumentative.Debug.started -= DebugMenu;

        ServiceProvider.Instance.ServiceLocator.Unregister<PlayerBattleAction>();
    }

    private void OnDestroy()
    {
        _playerInputs.Poetic.Move.performed -= UpdateStrafe;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame -= PoeticMovementEnable;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= PoeticMovementDisable;

        _playerInputs.Poetic.Menu.started -= PauseMenu;
        _playerInputs.Lyrical.Menu.started -= PauseMenu;
        _playerInputs.Narrative.Menu.started -= PauseMenu;
        _playerInputs.Argumentative.Menu.started -= PauseMenu;
        _playerInputs.UI.Cancel.started -= PauseMenu;

        ServiceProvider.Instance.ServiceLocator.Unregister<PlayerBattleAction>();
    }
}
