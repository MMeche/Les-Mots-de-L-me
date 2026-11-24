using System;

public class FlowStateMachineEventBus
{
    public event Action<FlowStateMachineEventContext> OnUIActivated;
    public void ActivateUI(FlowStateMachineEventContext context) => OnUIActivated?.Invoke(context);

    public event Action<FlowStateMachineEventContext> OnUIChangeSheet;
    public void UIChangeSheet(FlowStateMachineEventContext context) => OnUIChangeSheet?.Invoke(context);

    public event Action<FlowStateMachineEventContext> EnterOverworld;
    public void OnOverWorldEnter(FlowStateMachineEventContext context) => EnterOverworld?.Invoke(context);

    public event Action<FlowStateMachineEventContext> OnSteleInteraction;
    public void SteleInteraction(FlowStateMachineEventContext context) => OnSteleInteraction?.Invoke(context);

    public event Action<FlowStateMachineEventContext> OnSteleExit;
    public void ExitStele(FlowStateMachineEventContext context) => OnSteleExit?.Invoke(context);

    public event Action<FlowStateMachineEventContext> Pause;
    public void OnPauseEvent(FlowStateMachineEventContext context) => Pause?.Invoke(context);

    public event Action<FlowStateMachineEventContext> ChangeInputs;
    public void OnInputMapChange(FlowStateMachineEventContext context) => ChangeInputs?.Invoke(context);

    public event Action<FlowStateMachineEventContext> CombatMinisteleActivated;
    public void OnCombatMinisteleActivated(FlowStateMachineEventContext context) => CombatMinisteleActivated?.Invoke(context);

    public event Action<FlowStateMachineEventContext> TutorialFinished;
    public void OnTutorialFinish(FlowStateMachineEventContext context) => TutorialFinished?.Invoke(context);

    public event Action<FlowStateMachineEventContext> CameraBackToFollowingView;
    public void OnCameraBackToFollowingView(FlowStateMachineEventContext context) => CameraBackToFollowingView?.Invoke(context);

    public event Action<FlowStateMachineEventContext> StatTutorialFinished;
    public void OnStatTutorialFinish(FlowStateMachineEventContext context) => StatTutorialFinished?.Invoke(context);

    public event Action<FlowStateMachineEventContext> JournalTutorialFinished;
    public void OnJournalTutorialFinish(FlowStateMachineEventContext context) => JournalTutorialFinished?.Invoke(context);

    public event Action<FlowStateMachineEventContext> LevelFinished;
    public void OnLevelFinish(FlowStateMachineEventContext context) => LevelFinished?.Invoke(context);

    public event Action<FlowStateMachineEventContext> OnActionSubscribed;
    public void SubscribeAction(FlowStateMachineEventContext context) => OnActionSubscribed?.Invoke(context);
    
    public event Action<FlowStateMachineEventContext> OnActionUnsubscribed;
    public void UnsubscribeAction(FlowStateMachineEventContext context) => OnActionUnsubscribed?.Invoke(context);

    public event Action<FlowStateMachineEventContext> OnIntroFinished;
    public void IntroFinished(FlowStateMachineEventContext context) => OnIntroFinished?.Invoke(context);
}
