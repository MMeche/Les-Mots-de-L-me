using UnityEngine;

public class BattleFlowState_Lyric : BattleBaseState
{ 
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_Lyric(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_Lyric), stateMachine) 
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
		Debug.Log($"Entering {nameof(BattleFlowState_Lyric)}");

		_battleFlowStateMachine.BattleFlowStateMachineEventBus.LyricMinigameStart(new BattleFlowStateMachineEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "Lyric" });

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
		_battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateLyric);
	}
}