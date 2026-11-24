using System.Linq;
using Ink.Parsed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SecondaryPlayerStatusUIManager : MonoBehaviour
{
    BattleUIManager _battleUIManager;
    BattleFlowStateMachine _battleFlowStateMachine;

    PlayerStatus _secondaryStatus1;
    PlayerStatus _secondaryStatus2;

    [SerializeField] TMP_Text _name1TextField;
    [SerializeField] TMP_Text _name2TextField;

    [SerializeField] Image _healthBar1;
    [SerializeField] Image _healthBar2;

    void OnEnable()
    {

        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _battleUIManager.BattleUIEventBus.OnSetBaseValue += SetBaseValue;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone += ToggleStatuses;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ToggleStatuses;
    }

    private void OnDisable()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone -= ToggleStatuses;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ToggleStatuses;
    }

    void SetBaseValue(BattleUIEventContext ctx)
    {
        if (ctx != null)
        {
            try
            {
                List<PlayerStatus> statuses = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
                statuses.Remove(ctx.PlayerStatus);

                //Pour que ça roule correctement !
                if(statuses[0]== _secondaryStatus1 && statuses[1] != _secondaryStatus2)
                {
                    _secondaryStatus1 = statuses[1];
                    _secondaryStatus2 = statuses[0];
                }
                else
                {
                    _secondaryStatus1 = statuses[0];
                    _secondaryStatus2 = statuses[1]; 
                }
                    
                SetUI();
            }
            catch
            {
                Debug.LogWarning($"Unable to set secondary UI status");
            }
        }
    }

    void ToggleStatuses(BattleFlowStateMachineEventContext ctx)
    {
        for(int i =0;i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }
    }

    void SetUI()
    {
        //
        _name1TextField.text = _secondaryStatus1.CharName;
        _healthBar1.fillAmount = _secondaryStatus1.Health / _secondaryStatus1.MaxHealth;

        //
        _name2TextField.text = _secondaryStatus2.CharName;
        _healthBar2.fillAmount = _secondaryStatus2.Health / _secondaryStatus2.MaxHealth;
    }
}
