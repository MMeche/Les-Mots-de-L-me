using UnityEngine;

public class FlowState_Overworld : BaseState
{ 
    private readonly FlowStateMachine _flowStateMachine;

    public FlowState_Overworld(StateMachine stateMachine) : base(nameof(FlowState_Overworld), stateMachine) 
    {
        _flowStateMachine = (FlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(FlowState_Overworld)}");

		Subscribe();
		_flowStateMachine.FlowStateMachineEventBus.OnOverWorldEnter(new() { PlayerInputs = new() });
		_flowStateMachine.FlowStateMachineEventBus.OnInputMapChange(new() { NewActionMapName = "Player" });
		_flowStateMachine.FlowProgressTracker.ChangeGamePhase(FlowProgressTracker.GamePhase.Overworld);
    }

    public override void Exit()
	{
		Unsubscribe();

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
		_flowStateMachine.ChangeState(_flowStateMachine.FlowStates.FlowStateEnterOverWorld);
	}
}