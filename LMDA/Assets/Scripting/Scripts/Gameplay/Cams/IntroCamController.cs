using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using Unity.VisualScripting;

public class IntroCamController : MonoBehaviour
{
    [SerializeField] CinemachineBrain _cinemamachineBrain;
    [SerializeField] TutorialState _tutoState;
    CinemachineSplineDolly _cinemachineSplineDolly;
    CinemachineSplineDollyLookAtTargets _cinemachineSplineDollyLookAtTargets;

    FlowStateMachine _flowStateMachine;

    private void Start()
    {
        if (!_tutoState.IntroCamDone)
        {
            StartCoroutine(CamMovement());
        }
        
    }

    private void OnEnable()
    {
        _flowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _cinemachineSplineDolly = GetComponent<CinemachineSplineDolly>();
        _cinemachineSplineDollyLookAtTargets = GetComponent<CinemachineSplineDollyLookAtTargets>();
        if (_tutoState.IntroCamDone)
        {
            _cinemamachineBrain.ChannelMask = OutputChannels.Channel01;
        }
    }

    IEnumerator CamMovement()
    {
        yield return new WaitForSeconds(1);

        _cinemachineSplineDolly.AutomaticDolly.Enabled = true;

        while (_cinemachineSplineDolly.CameraPosition < 0.98)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        _flowStateMachine.FlowStateMachineEventBus.IntroFinished(new());
        _tutoState.IntroCamDone = true;
        _flowStateMachine.FlowStateMachineEventBus.OnInputMapChange(new() { NewActionMapName = "Player" });
        _cinemamachineBrain.ChannelMask = OutputChannels.Channel01;
    }
}
