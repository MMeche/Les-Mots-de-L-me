using UnityEngine;

public class BattleFlowState_PoeticMinigameFail : BattleBaseState
{ 
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_PoeticMinigameFail(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_PoeticMinigameFail), stateMachine) 
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(BattleFlowState_PoeticMinigameFail)}");

		//_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigameFail(new BattleFlowStateMachineEventContext { });

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
		_battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePoeticMinigameFail);
	}
}