using UnityEngine;

public class FlowState_Cinematic : BaseState
{ 
    private readonly FlowStateMachine _flowStateMachine;

    public FlowState_Cinematic(StateMachine stateMachine) : base(nameof(FlowState_Cinematic), stateMachine) 
    {
        _flowStateMachine = (FlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(FlowState_Cinematic)}");

		_flowStateMachine.FlowStateMachineEventBus.OnInputMapChange(new() { NewActionMapName = "Dialog" });
		Subscribe();

    }

    public override void Exit()
	{
		Unsubscribe();
        Debug.Log($"Exiting {nameof(FlowState_Cinematic)}");
        _flowStateMachine.FlowStateMachineEventBus.ExitStele(new() {});
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
		_flowStateMachine.ChangeState(_flowStateMachine.FlowStates.FlowStateEnterDialog);
	}
}