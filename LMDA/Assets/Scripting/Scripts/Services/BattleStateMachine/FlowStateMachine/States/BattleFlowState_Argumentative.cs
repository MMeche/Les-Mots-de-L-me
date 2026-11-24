using UnityEngine;

public class BattleFlowState_Argumentative : BattleBaseState
{ 
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_Argumentative(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_Argumentative), stateMachine) 
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(BattleFlowState_Argumentative)}");

		_battleFlowStateMachine.BattleFlowStateMachineEventBus.ArgumentativeMinigameStart(new BattleFlowStateMachineEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "Argumentative" });

        Subscribe();
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
		_battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateArgumentative);
	}
}