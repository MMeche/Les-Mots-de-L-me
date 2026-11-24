using UnityEngine;

public class BattleFlowState_Poetic : BattleBaseState
{ 
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_Poetic(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_Poetic), stateMachine) 
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(BattleFlowState_Poetic)}");

		_battleFlowStateMachine.BattleFlowStateMachineEventBus.PoeticMinigameStart(new BattleFlowStateMachineEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "Poetic" });

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
		_battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePoetic);
	}
}