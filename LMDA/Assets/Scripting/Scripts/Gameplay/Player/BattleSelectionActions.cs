using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleSelectionActions : MonoBehaviour
{
    [SerializeField] List<Transform> _anchors;
    int _targetAnchor;

    BattleFlowStateMachine _battleFlowStateMachine;
    BattleInputManager _battleInputManager;
    BattleManager _battleManager;
    FlowStateMachine _fsm;
    PlayerInputs _playerInputs;
    PauseManager _pauseManager;
    AudioManager _audioManager;
    FMODEvents _sfxEvents;
    public bool MouseSelection;

    private void Start()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
    }

    private void OnEnable()
    {
        _pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();

        _battleInputManager = ServiceProvider.Instance.ServiceLocator.Get<BattleInputManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();
        
        if(_battleManager.HasPlayed != null)
        {
            if (!_battleManager.HasPlayed[1] && _battleManager.GetPlayerStatuses()[1].Health > 0) _targetAnchor = 1;
            else if (!_battleManager.HasPlayed[2] && _battleManager.GetPlayerStatuses()[2].Health > 0) _targetAnchor = 2;
            else _targetAnchor = 0;
            
            _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionChange(new()
            {
                PlayerStatus = _battleManager.GetPlayerStatuses()[_targetAnchor]
            });
        }
        else _targetAnchor = 1;

        transform.position = _anchors[_targetAnchor].position;
        

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ActivateSelection;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionCancel += ActivateSelection;

        _playerInputs = _battleInputManager.PlayerInputs;
        _playerInputs.BattleSelection.Menu.performed += PauseMenu;
        _playerInputs.UI.Cancel.started += CancelMenu;
        _playerInputs.BattleSelection.Debug.started += DebugMenu;

        
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if((ctx.ReadValue<Vector2>().x < 0 || ctx.ReadValue<Vector2>().y > 0))
        {
            _targetAnchor++;
            _targetAnchor = _targetAnchor % 3;
            while (_battleManager.HasPlayed[_targetAnchor] || _battleManager.GetPlayerStatuses()[_targetAnchor].Health<=0)
            {
                _targetAnchor++;
                _targetAnchor = _targetAnchor % 3;
            }
        }
        if ((ctx.ReadValue<Vector2>().x > 0 || ctx.ReadValue<Vector2>().y < 0))
        {
            _targetAnchor--;
            if (_targetAnchor < 0)
            {
                _targetAnchor = 2;
            }
            while (_battleManager.HasPlayed[_targetAnchor] || _battleManager.GetPlayerStatuses()[_targetAnchor].Health <= 0)
            {
                _targetAnchor--;
                if (_targetAnchor < 0)
                {
                    _targetAnchor = 2;
                }
            }
        }

        transform.position = _anchors[_targetAnchor].position;
        _audioManager.PlayOneShotAttached(_sfxEvents.UIHover, _anchors[_targetAnchor].gameObject);
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionChange(new()
        {
            PlayerStatus = _battleManager.GetPlayerStatuses()[_targetAnchor],
            EnemyStatus = _battleManager.GetEnemyStatus()
        });
    }

    public void Move(int anchor)
    {
        if (!_battleManager.HasPlayed[anchor] && !_pauseManager.IsPaused)
        {
            _targetAnchor = anchor;
            transform.position = _anchors[_targetAnchor].position;
            _audioManager.PlayOneShotAttached(_sfxEvents.UIHover, _anchors[_targetAnchor].gameObject);
            _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionChange(new()
            {
                PlayerStatus = _battleManager.GetPlayerStatuses()[_targetAnchor],
                EnemyStatus = _battleManager.GetEnemyStatus()
            });
        }
    }

    private void Chose(InputAction.CallbackContext ctx)
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionDone(new()
        {
            PlayerStatus = _battleManager.GetPlayerStatuses()[_targetAnchor],
            EnemyStatus = _battleManager.GetEnemyStatus()
        });
        _battleManager.HasPlayed[_targetAnchor] = true;

        //bool allPlayed = true;
        //foreach(var b in _battleManager.HasPlayed)
        //{
        //    if (b == false) allPlayed = false;
        //}
        //if (allPlayed)
        //{
        //    for(int i = 0; i < _battleManager.HasPlayed.Length; i++)
        //    {
        //        if (_battleManager.PlayerStillAlive[i])
        //            _battleManager.HasPlayed[i] = false;
        //    }
        //}
        _audioManager.PlayOneShotAttached(_sfxEvents.UIClick, _anchors[_targetAnchor].gameObject);
        _playerInputs.BattleSelection.Move.started -= Move;
        _playerInputs.BattleSelection.Validate.started -= Chose;
    }

    public void Chose(int anchor)
    {
        if (!_battleManager.HasPlayed[anchor] && !_pauseManager.IsPaused)
        {
            _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionDone(new()
            {
                PlayerStatus = _battleManager.GetPlayerStatuses()[_targetAnchor],
                EnemyStatus = _battleManager.GetEnemyStatus()
            });
            _battleManager.HasPlayed[_targetAnchor] = true;

            //bool allPlayed = true;
            //foreach (var b in _battleManager.HasPlayed)
            //{
            //    if (b == false) allPlayed = false;
            //}
            //if (allPlayed)
            //{
            //    for (int i = 0; i < _battleManager.HasPlayed.Length; i++)
            //    {
            //        if (_battleManager.PlayerStillAlive[i])
            //            _battleManager.HasPlayed[i] = false;
            //    }
            //}
            _audioManager.PlayOneShotAttached(_sfxEvents.UIClick, _anchors[_targetAnchor].gameObject);
            _playerInputs.BattleSelection.Move.started -= Move;
            _playerInputs.BattleSelection.Validate.started -= Chose;
            MouseSelection = false;
        }
    }

    private void ActivateSelection(BattleFlowStateMachineEventContext obj)
    {
        _playerInputs.BattleSelection.Move.started += Move;
        _playerInputs.BattleSelection.Validate.started += Chose;
        MouseSelection = true;
    }
    private void PauseMenu(InputAction.CallbackContext ctx)
    {
        if (!_pauseManager.IsPaused && _fsm.GetCurrentState() != _fsm.FlowStates.FlowStateEnterDialog) _pauseManager.PauseGame();
    }

    private void CancelMenu(InputAction.CallbackContext ctx)
    {
        _pauseManager.Cancel();
    }

    private void DebugMenu(InputAction.CallbackContext ctx)
    {
        if(!_pauseManager.IsPaused && _fsm.GetCurrentState() != _fsm.FlowStates.FlowStateEnterDialog) _pauseManager.DebugPause();
    }

    private void OnDisable()
    {
        _playerInputs.BattleSelection.Move.performed -= Move;
        ServiceProvider.Instance.ServiceLocator.Unregister<BattleSelectionActions>();
    }

    private void OnDestroy()
    {
        _playerInputs.BattleSelection.Move.performed -= Move;
        ServiceProvider.Instance.ServiceLocator.Unregister<BattleSelectionActions>();
    }
}
    
