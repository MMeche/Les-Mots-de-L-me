using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    BattleFlowStateMachine _bsm;

    private void OnEnable()
    {
        _bsm = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
    }

    public void PlayerAttackStrike()
    {
        _bsm.BattleFlowStateMachineEventBus.PlayerAttackStrike(new());
    }

    public void EnemyAttackStrike()
    {
        _bsm.BattleFlowStateMachineEventBus.EnemyAttackStrike(new());
    }
}
