using UnityEngine;

public class FlowState_Pause : BaseState
{ 
    private readonly FlowStateMachine _flowStateMachine;

    public FlowState_Pause(StateMachine stateMachine) : base(nameof(FlowState_Pause), stateMachine) 
    {
        _flowStateMachine = (FlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(FlowState_Pause)}");
		_flowStateMachine.FlowStateMachineEventBus.OnPauseEvent(new() { });
		_flowStateMachine.FlowStateMachineEventBus.OnInputMapChange(new() { NewActionMapName = "Pause" });
		Time.timeScale = 0f;
		Subscribe();

    }

    public override void Exit()
	{
		Unsubscribe();
        Time.timeScale = 1f;
        _flowStateMachine.FlowStateMachineEventBus.OnPauseEvent(new() { });
        base.Exit();
    }

	private void Subscribe()
	{
		
	}

	private void Unsubscribe()
	{
		
	}

	private void ChangeState()
	{
		_flowStateMachine.ChangeState(_flowStateMachine.FlowStates.FlowStatePause);
	}
}