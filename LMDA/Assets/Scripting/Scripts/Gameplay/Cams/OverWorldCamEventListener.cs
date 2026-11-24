using Unity.Cinemachine;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class OverWorldCamEventListener : MonoBehaviour
{
    FlowStateMachine _fsm;
    CinemachineBrain _camera;
    private void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _camera = GetComponent<CinemachineBrain>();
        _fsm.FlowStateMachineEventBus.TutorialFinished += TransitionToTutorialView;
    }

    void TransitionToTutorialView(FlowStateMachineEventContext ctx)
    {
        StartCoroutine(TutorialView(0.25f));
        

    }

    IEnumerator TutorialView(float time)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(time);
        _camera.ChannelMask = OutputChannels.Channel02;
        StartCoroutine(WaitUntilEndOfBlendAndComeBack());

    }

    IEnumerator WaitUntilEndOfBlendFollowingView()
    {
        yield return new WaitForFixedUpdate();
        while (_camera.IsBlending)
        {
            yield return null;
        }
        _fsm.FlowStateMachineEventBus.OnCameraBackToFollowingView(new());
    }

    IEnumerator WaitUntilEndOfBlendAndComeBack()
    {
        yield return new WaitForFixedUpdate();
        while (_camera.IsBlending)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        TransitionToFollowingView(new());
    }

    void TransitionToFollowingView(FlowStateMachineEventContext ctx)
    {
        _camera.ChannelMask = OutputChannels.Channel01;
        StartCoroutine(WaitUntilEndOfBlendFollowingView());
    }
}
