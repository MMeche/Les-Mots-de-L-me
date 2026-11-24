using UnityEngine;

public class BattleFlowState_EndCombat : BattleBaseState
{
    private readonly BattleFlowStateMachine _flowStateMachine;

    public BattleFlowState_EndCombat(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_EndCombat), stateMachine)
    {
        _flowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"Entering {nameof(BattleFlowState_EndCombat)}");
        _flowStateMachine.BattleFlowStateMachineEventBus.BattleLost(new() { });
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
