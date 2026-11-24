using UnityEngine;
using System.Collections.Generic;

public class PlayerOutlineAnimation : MonoBehaviour
{
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleManager _battleManager;
    Animator _animator;
    Outline _outline;
    void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _animator = GetComponent<Animator>();
        _outline = GetComponent<Outline>();
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionChanged += AnimateOutline;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += CheckHasPlayed;
    }

    void AnimateOutline(BattleFlowStateMachineEventContext ctx)
    {
        if(ctx.PlayerStatus  != null)
        {
            List<PlayerStatus> statuses = _battleManager.GetPlayerStatuses();
            int selectedPlayerIndex = statuses.FindIndex(s => s == ctx.PlayerStatus);
            if(transform.GetSiblingIndex() == selectedPlayerIndex)
            {
                _animator.SetTrigger("Selected");
                
            }
            else
            {
                _animator.SetTrigger("Normal");
            }
        }
    }

    private void CheckHasPlayed(BattleFlowStateMachineEventContext ctx)
    {
        _outline.enabled = !_battleManager.HasPlayed[transform.GetSiblingIndex()] && _battleManager.PlayerStillAlive[transform.GetSiblingIndex()];
    }
    private void Update()
    {
        if (_animator.IsInTransition(0))
        {
            _outline.needsUpdate = true;
        }
    }

}
