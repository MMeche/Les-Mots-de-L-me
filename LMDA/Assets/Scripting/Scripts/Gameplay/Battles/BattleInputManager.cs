using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleInputManager : MonoBehaviour
{
    BattleFlowStateMachine _battleFSM;
    FlowStateMachine _fsm;
    
    PauseManager _pauseManager;

    public PlayerInputs PlayerInputs;

    string ActionMapNameMemory;

    void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);

        _pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();

        _battleFSM = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        PlayerInputs = new();

        _battleFSM.BattleFlowStateMachineEventBus.OnInputMapChange += ChangeInputMap;
        _fsm.FlowStateMachineEventBus.Pause += PauseEvent;
    }

    private void ChangeInputMap(BattleFlowStateMachineEventContext obj)
    {
        PlayerInputs.Disable();
        Debug.Log("Changing Inputs" + obj.NewActionMapName);
        ActionMapNameMemory = obj.NewActionMapName;
        switch (obj.NewActionMapName)
        {
            case "Narrative":
                PlayerInputs.Narrative.Enable();
                break;

            case "Poetic":
                PlayerInputs.Poetic.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case "Lyrical":
                PlayerInputs.Lyrical.Enable();
                break;

            case "Argumentative":
                PlayerInputs.Argumentative.Enable();
                break;

            case "BattleSelection":
                PlayerInputs.BattleSelection.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            default:
                PlayerInputs.UI.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    private void PauseEvent(FlowStateMachineEventContext obj)
    {
        PlayerInputs.Disable();
        if (_pauseManager.IsPaused)
        {
            PlayerInputs.UI.Enable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ChangeInputMap(new() { NewActionMapName = ActionMapNameMemory });
        }
    }
    

    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

}
