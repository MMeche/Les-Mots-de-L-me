using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraEventListener : MonoBehaviour
{
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleManager _battleManager;
    CinemachineBrain _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _camera = GetComponent<CinemachineBrain>();
        ServiceProvider.Instance.ServiceLocator.Register(this);
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += TransitionToPoeticMinigameView;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += TransitionToBattleStandardView;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame += TransitionToNarrativeMinigameView;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame += TransitionToArgumentativeMinigameView;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame += TransitionToLyricMinigameView;
    }

    private void TransitionToPoeticMinigameView(BattleFlowStateMachineEventContext obj)
    {
        //transform.SetPositionAndRotation(new Vector3(-21, 9, 0), Quaternion.Euler(25, 90, 0));
        _camera.ChannelMask = OutputChannels.Channel02;
        StartCoroutine(WaitUntilEndOfBlend());
    }

    private void TransitionToNarrativeMinigameView(BattleFlowStateMachineEventContext obj)
    {
        //transform.SetPositionAndRotation(new Vector3(-23, 4.5f, -7), Quaternion.Euler(5, 70, 0));
        _camera.ChannelMask = OutputChannels.Channel03;
        StartCoroutine(WaitUntilEndOfBlend());
        
    }

    private void TransitionToArgumentativeMinigameView(BattleFlowStateMachineEventContext obj)
    {
        //transform.SetPositionAndRotation(new Vector3(-16, 3, -4), Quaternion.Euler(12, 66, 0));
        _camera.ChannelMask = OutputChannels.Channel04;
        StartCoroutine(WaitUntilEndOfBlend());
    }

    private void TransitionToLyricMinigameView(BattleFlowStateMachineEventContext obj)
    {
        _camera.ChannelMask = OutputChannels.Channel05;
        StartCoroutine(WaitUntilEndOfBlend());
    }

    private void TransitionToBattleStandardView(BattleFlowStateMachineEventContext obj)
    {
        //if (Camera.main != null)
        //{
        //    Camera.main.orthographic = false;
        //    _cinemachineCamera.Lens.FieldOfView = 60f;
        //    transform.SetPositionAndRotation(new Vector3(0, 15, -30), Quaternion.Euler(30, 0, 0));   
        //}
        _camera.ChannelMask = OutputChannels.Channel01;
        

    }

    void OnDisable()
    {
        BattleFlowStateMachineEventContext obj = new(); //??
        TransitionToBattleStandardView(obj);

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame -= TransitionToPoeticMinigameView;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= TransitionToBattleStandardView;
        ServiceProvider.Instance.ServiceLocator.Unregister<CameraEventListener>();
    }

    IEnumerator WaitUntilEndOfBlend()
    {
        yield return new WaitForFixedUpdate();
        while (_camera.IsBlending)
        {
            yield return null;
        }
        _battleManager.BattleManagerEventBus.CamPlaced(new());
    }
}
