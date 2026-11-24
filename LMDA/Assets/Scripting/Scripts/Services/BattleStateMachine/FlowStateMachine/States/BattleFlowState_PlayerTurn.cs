using UnityEngine;

public class BattleFlowState_PlayerTurn : BattleBaseState
{
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_PlayerTurn(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_EnterCombat), stateMachine)
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"Entering {nameof(BattleFlowState_PlayerTurn)}");
        Cursor.lockState = CursorLockMode.None;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.PlayersTurn(new());
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "BattleSelection" });

        Subscribe();
    }

    public override void Exit()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ExitPlayersTurn(new BattleFlowStateMachineEventContext { });
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
