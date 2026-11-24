using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// FlowStateMachine manage the flow of phases in the gameplay.
/// New states must be entered manually in FlowStates.
/// </summary>
public class BattleFlowStateMachine : BattleStateMachine
{
    public BattleStates BattleStates;
    public BattleFlowStateMachineEventBus BattleFlowStateMachineEventBus;

    private void Awake()
    {
        BattleStates = new BattleStates(this);
        BattleFlowStateMachineEventBus = new BattleFlowStateMachineEventBus();
    }

    private void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceProvider.Instance.ServiceLocator.Unregister(typeof(BattleFlowStateMachine));
    }

    protected override BattleBaseState GetInitialState()
    {
        Debug.Log("Initializing first state");
		return BattleStates.BattleFlowStateEnterCombat;
    }
}
