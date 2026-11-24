using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleManager _battleManager;

    private void OnEnable()
    {
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnEnemysTurn += ActionOnTurn;
    }

    private void ActionOnTurn(BattleFlowStateMachineEventContext context)
    {
        _battleManager.BattleManagerEventBus.AttackPlayer(new BattleManagerEventContext { DamageDone = 30 });
    }
}
