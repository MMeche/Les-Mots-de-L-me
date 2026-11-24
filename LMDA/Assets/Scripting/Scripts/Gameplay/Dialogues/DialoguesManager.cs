using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class DialoguesManager : MonoBehaviour
{
    FlowStateMachine _fsm;
    [SerializeField] GameObject SteleUI;
    [SerializeField] GameObject _journalStele;
    [SerializeField] GameObject DialogUI;
    [SerializeField] TutorialState _state;

    private FMOD.Studio.EventInstance _steleVoiceInstance;
    GameObject _tutorialCombat;
    bool _replay = false;

    PlayerInputs _playerInputs;
    void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.EnterOverworld += LoadPlayerInput;
        _fsm.FlowStateMachineEventBus.OnSteleInteraction += ActivateSteleUI;
        _fsm.FlowStateMachineEventBus.OnSteleExit += ActivateSteleUI;
    }

    private void ActivateSteleUI(FlowStateMachineEventContext obj)
    {
        SteleUI.GetComponentInChildren<RawImage>().texture = obj.VisuelStele;
        _tutorialCombat = obj.TutorialEnemy;
        if(obj.Replay)_replay = obj.Replay;
        if (!SteleUI.activeSelf)
        {
            
             SteleUI.GetComponent<SteleScrollbar>().SetMinistele(obj.Ministele);
            
            _steleVoiceInstance = obj.InstanceVoiceStele;
            _steleVoiceInstance.getDescription(out FMOD.Studio.EventDescription description);
            description.getLength(out int lengthMs);
            SteleUI.GetComponent<SteleScrollbar>().SetEvtDuration(lengthMs/1000);
            RuntimeManager.AttachInstanceToGameObject(_steleVoiceInstance, gameObject);
            _playerInputs.UI.Cancel.started += DialogEnd;
            _steleVoiceInstance.start();
            
        }
        else
        {
            _steleVoiceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        Time.timeScale = SteleUI.activeSelf ? 1f : 0f;
        SteleUI.SetActive(!SteleUI.activeSelf);

        

    }

    private void DialogEnd(InputAction.CallbackContext ctx)
    {
        _steleVoiceInstance.release();
        if (!_replay)
        {
            if (!_state.TutoJournalDone)
            {
                _fsm.FlowStateMachineEventBus.UnsubscribeAction(new());
                _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
                _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "JournalUIPanel", "JournalTutorial" } });
            }
            else
            {
                _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterOverWorld);
            }
        }
        else
        {
            _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
            _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "CharUIPanel" } });
        }
        _replay = false;
        _playerInputs.UI.Cancel.started -= DialogEnd;
    }

    public void DialogEnd(bool spawnTuto)
    {
        if (spawnTuto)
        {
            _fsm.FlowStateMachineEventBus.OnCombatMinisteleActivated(new());
            _tutorialCombat.SetActive(true);
        }

        if (!_state.TutoJournalDone)
        {
            _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
            _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "JournalUIPanel", "JournalTutorial" } });
        }
        else
        {
            _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterOverWorld);
            _steleVoiceInstance.release();
        }

    }

    //public void DialogEnd()
    //{
    //    _fsm.FlowStateMachineEventBus.OnCombatMinisteleActivated(new());
    //    _tutorialCombat.SetActive(true);
    //    _steleVoiceInstance.release();
    //    _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterOverWorld);
    //}


    private void LoadPlayerInput(FlowStateMachineEventContext obj)
    {
        _playerInputs = obj.PlayerInputs;

        
        _fsm.FlowStateMachineEventBus.EnterOverworld -= LoadPlayerInput;
    }

}
