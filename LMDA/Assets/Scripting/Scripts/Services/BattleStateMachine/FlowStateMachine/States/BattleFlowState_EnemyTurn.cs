using UnityEngine;

public class BattleFlowState_EnemyTurn : BattleBaseState
{
    private readonly BattleFlowStateMachine _flowStateMachine;

    public BattleFlowState_EnemyTurn(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_EnterCombat), stateMachine)
    {
        _flowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"Entering {nameof(BattleFlowState_EnemyTurn)}");

        _flowStateMachine.BattleFlowStateMachineEventBus.EnemysTurn(new BattleFlowStateMachineEventContext { });

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

    }
}
