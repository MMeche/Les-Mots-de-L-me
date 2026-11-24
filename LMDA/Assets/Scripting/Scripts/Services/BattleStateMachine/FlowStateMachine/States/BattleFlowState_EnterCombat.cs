using UnityEngine;

public class BattleFlowState_EnterCombat : BattleBaseState
{ 
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_EnterCombat(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_EnterCombat), stateMachine) 
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(BattleFlowState_EnterCombat)}");

		_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleStart(new BattleFlowStateMachineEventContext { });
		_battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "BattleSelection" });
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
		_battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateEnterCombat);
	}
}