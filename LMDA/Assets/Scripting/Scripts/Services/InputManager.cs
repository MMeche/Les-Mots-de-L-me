using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    FlowStateMachine _fsm;
    PlayerInputs _playerInputs;

    private void Start()
    {
        _playerInputs.Actionless.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();

        _fsm.FlowStateMachineEventBus.ChangeInputs += ChangeInputMap;
        _fsm.FlowStateMachineEventBus.EnterOverworld += LoadPlayerInputs;
    }

    public void ChangeInputMap(FlowStateMachineEventContext obj)
    {
        _playerInputs.Disable();
        
        switch (obj.NewActionMapName)
        {
            case "Dialog":
                _playerInputs.UI.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case "Player":
                _playerInputs.Player.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case "Pause":
                _playerInputs.UI.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                _playerInputs.Player.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

   

    private void LoadPlayerInputs(FlowStateMachineEventContext obj)
    {
        _playerInputs = obj.PlayerInputs;
        
        _fsm.FlowStateMachineEventBus.EnterOverworld -= LoadPlayerInputs;
    }

    public PlayerInputs GetPlayerInputs()
    {
        return _playerInputs;
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }
}
