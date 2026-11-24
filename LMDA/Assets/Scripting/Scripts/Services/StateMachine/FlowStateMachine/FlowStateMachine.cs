using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// FlowStateMachine manage the flow of phases in the gameplay.
/// New states must be entered manually in FlowStates.
/// </summary>
public class FlowStateMachine : StateMachine
{
    public FlowStates FlowStates;
    public FlowStateMachineEventBus FlowStateMachineEventBus;
    public FlowProgressTracker FlowProgressTracker;

    private void Awake()
    {
        FlowStates = new FlowStates(this);
        FlowStateMachineEventBus = new FlowStateMachineEventBus();
        FlowProgressTracker = new FlowProgressTracker();
    }

    private void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceProvider.Instance.ServiceLocator.Unregister(typeof(FlowStateMachine));
    }

    protected override BaseState GetInitialState()
    {
        Debug.Log("Initializing first state");
		return FlowStates.FlowStateEnterOverWorld;
    }
}
