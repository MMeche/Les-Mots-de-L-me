using UnityEngine;

public class BattleFlowState_Tutorial : BattleBaseState
{
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_Tutorial(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_Tutorial), stateMachine)
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"Entering {nameof(BattleFlowState_Tutorial)}");
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "Pouet Pouet" });

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
