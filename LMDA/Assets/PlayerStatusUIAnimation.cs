using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIAnimation : MonoBehaviour
{
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleManager _battleManager;
    BattleUIManager _buim;
    Animator _animator;
    PlayerStatus _playerStatus;

    Vector2 _startPos;
    void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _buim = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _animator = GetComponent<Animator>();
        _playerStatus = GetComponent<PlayerStatusUIManager>().GetStatus();

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += Normal;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionChanged += AnimateUISelection;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone += AnimateUIDone;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionCancel += AnimateUISelection;


        _buim.BattleUIEventBus.OnTutoDisplay += ToggleInteraction;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += ToggleInteraction;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame += ToggleInteraction;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame += ToggleInteraction;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame += ToggleInteraction;

    }

    private void Start()
    {
        _startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    void ToggleInteraction(BattleFlowStateMachineEventContext ctx)
    {
        
        _animator.SetTrigger("Battle");
    }

    void ToggleInteraction(BattleUIEventContext ctx)
    {
        _animator.SetTrigger("Battle");
    }

    void Normal(BattleFlowStateMachineEventContext ctx)
    {
        _animator.SetTrigger("Normal");
    }

    void AnimateUISelection(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx.PlayerStatus != null)
        {
            if (ctx.PlayerStatus == _playerStatus)
            {
                _animator.SetTrigger("Selected");

            }
            else
            {
                _animator.SetTrigger("Normal");
            }
        }
        else
        {
            _animator.SetTrigger("Normal");
        }
    }

    void AnimateUIDone(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx.PlayerStatus != null)
        {
            if (ctx.PlayerStatus == _playerStatus)
            {
                _animator.SetTrigger("Choosed");

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void ResetPos()
    {
        GetComponent<RectTransform>().anchoredPosition = _startPos;
    }
}
