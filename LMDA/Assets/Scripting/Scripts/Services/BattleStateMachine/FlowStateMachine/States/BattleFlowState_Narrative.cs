using UnityEngine;

public class BattleFlowState_Narrative : BattleBaseState
{
    private readonly BattleFlowStateMachine _battleFlowStateMachine;

    public BattleFlowState_Narrative(BattleStateMachine stateMachine) : base(nameof(BattleFlowState_Narrative), stateMachine)
    {
        _battleFlowStateMachine = (BattleFlowStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"Entering {nameof(BattleFlowState_Narrative)}");
        Cursor.lockState = CursorLockMode.None;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.NarrativeMiniGameStart(new BattleFlowStateMachineEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.ChangeInputMap(new BattleFlowStateMachineEventContext { NewActionMapName = "Narrative" });

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
