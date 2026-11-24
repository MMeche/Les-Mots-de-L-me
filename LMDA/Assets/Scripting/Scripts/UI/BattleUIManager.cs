using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    private BattleFlowStateMachine _battleFlowStateMachine;
    //private FlowStateMachine _fsm;

    public BattleUIEventContext BattleUIEventContext;
    public BattleUIEventBus BattleUIEventBus;

    EnemyStatus _enemyBaseStatus;
    PlayerStatus _playerBaseStatus;
    [SerializeField] private UIContainer _userInterfaceElements;
    [SerializeField] private List<GameObject> _uiObject = new();
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] GameObject _victoryScreen;
    [SerializeField] TutorialState _tutorialStatus;

    private Dictionary<string, GameObject> _nameUIObjectPairs = new();

    private void OnEnable()
    {
        BattleUIEventBus = new BattleUIEventBus();
        BattleUIEventContext = new BattleUIEventContext();
        ServiceProvider.Instance.ServiceLocator.Register(this);

        BattleUIEventBus.OnTutoDisplay += TutorialStateChange;

        BuildDictionary();

        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.UIActivated += ActivateUI;
        
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.UISetup += SetupGameBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone += SetupGameBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone += ToggleReturnButton;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionChanged += ChangePlayerBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionCancel += ToggleReturnButton;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionCancel += UnusedPlayerStatusEnable;


        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.StartBattle += PlayerCombatOptionToggle;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += PlayersTurn;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnEnemysTurn += EnemysTurn;
        BattleUIEventBus.OnTutoDisplay += CombatUIMinimize;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += CombatUIMinimize;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame += CombatUIMinimize;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame += CombatUIMinimize;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame += CombatUIMinimize;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose += DeathScreen;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += VictoryScreen;

        //_fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        //_fsm.FlowStateMachineEventBus.Pause += PauseActivateUI;
    }

  
    private void OnDisable()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.UIActivated -= ActivateUI;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.StartBattle -= SetupGameBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone -= SetupGameBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionChanged -= ChangePlayerBaseValues;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersExit -= PlayersTurn;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnEnemysTurn -= EnemysTurn;

        ServiceProvider.Instance.ServiceLocator.Unregister<BattleUIManager>();
    }
    
    private void TutorialStateChange(BattleUIEventContext obj)
    {
        ActivateUI(new() { UINames = new string[] { obj.TutoPanelName } });
        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateTutorial);
    }

    private void EnemysTurn(BattleFlowStateMachineEventContext obj)
    {
        //BattleUIEventBus.InteractionToggle(new BattleUIEventContext() { });
    }

    private void PlayersTurn(BattleFlowStateMachineEventContext obj)
    {
        UnusedPlayerStatusEnable(new());
        TryGetDictionaryPrefabFromName("InfoPlayerSelection", out var uiObject1);
        uiObject1.SetActive(true);
        BattleUIEventBus.InteractionToggle(new BattleUIEventContext() { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame += CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame += CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame += CombatUIMinimize;
    }


    private void UnusedPlayerStatusEnable(BattleFlowStateMachineEventContext obj)
    {
        for(int i = 7; i < 10; i++)
        {
            TryGetDictionaryPrefabFromName(_uiObject[i].name, out var uiObject);
            if (!uiObject.activeSelf) uiObject.SetActive(true);
        }
    }

    private void ToggleReturnButton(BattleFlowStateMachineEventContext ctx)
    {
        TryGetDictionaryPrefabFromName("Retour", out var uiObject);
        uiObject.SetActive(!uiObject.activeSelf);
        TryGetDictionaryPrefabFromName("InfoPlayerSelection", out var uiObject1);
        uiObject1.SetActive(!uiObject1.activeSelf);
        TryGetDictionaryPrefabFromName("InfoMinigameSelection", out var uiObject2);
        uiObject2.SetActive(!uiObject2.activeSelf);
    }

    private void CombatUIMinimize(BattleUIEventContext ctx)
    {
        TryGetDictionaryPrefabFromName("InfoMinigameSelection", out var uiObject);
        uiObject.SetActive(!uiObject.activeSelf);

        TryGetDictionaryPrefabFromName("Retour", out var uiObject1);
        uiObject1.SetActive(!uiObject1.activeSelf);
        BattleUIEventBus.InteractionToggle(new());

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame -= CombatUIMinimize;
    }

    private void CombatUIMinimize(BattleFlowStateMachineEventContext ctx)
    {
        TryGetDictionaryPrefabFromName("InfoMinigameSelection", out var uiObject);
        uiObject.SetActive(!uiObject.activeSelf);

        TryGetDictionaryPrefabFromName("Retour", out var uiObject1);
        uiObject1.SetActive(!uiObject1.activeSelf);
        BattleUIEventBus.InteractionToggle(new());

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnNarrativeMiniGame -= CombatUIMinimize;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnArgumentativeMinigame -= CombatUIMinimize;
    }
    

    private void SetupGameBaseValues(BattleFlowStateMachineEventContext ctx)
    {
        //TryGetDictionaryPrefabFromName("PlayerCombatOptions", out var uiObject);
        //uiObject.SetActive(true);
        if (ctx.PlayerStatus != null && ctx.EnemyStatus != null)
        {
            BattleUIEventBus.SetBaseValue(new BattleUIEventContext
            {
                PlayerStatus = ctx.PlayerStatus,
                EnemyStatus = ctx.EnemyStatus,
            });
        }
        //if(ctx.EnemyStatus != null ) BattleUIEventBus.SetBaseValue(new BattleUIEventContext
        //{
        //    EnemyStatus = ctx.EnemyStatus
        //});

        BattleUIEventBus.InteractionToggle(new BattleUIEventContext() { });
    }

    private void ChangePlayerBaseValues(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx.PlayerStatus != null)
        {
            BattleUIEventBus.SetBaseValue(new BattleUIEventContext
            {
                PlayerStatus = ctx.PlayerStatus,
            });
        }
    }

    private void ActivateUI(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx == null)
        {
            Debug.LogWarning($"WARNING: Context of type {typeof(FlowStateMachineEventContext).FullName} is null");
            return;
        }

        if (ctx.UINames.Length < 0 || ctx.UINames == null)
            return;

        foreach (var name in ctx.UINames)
        {
            if (!TryGetDictionaryPrefabFromName(name, out var uiObject))
            {
                Debug.LogWarning($"WARNING: Can't find KeyValuePair with name {name}");
                return;
            }
            Debug.Log($"{name}");
            uiObject.SetActive(!uiObject.activeSelf);
            //uiObject.name = "Fuck yu";
        }
    }

    private void DeathScreen(BattleFlowStateMachineEventContext ctx)
    {
        _deathScreen.SetActive(true);
    }

    private void VictoryScreen(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx.ExpToAdd == 0)
        {
            _victoryScreen.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
        }
        else
        {
            _victoryScreen.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Vous avez gagné " + ctx.ExpToAdd + " points d'expérience";
        }
        _victoryScreen.SetActive(true);
    }

    //private void PauseActivateUI(FlowStateMachineEventContext ctx)
    //{
    //    if (!TryGetDictionaryPrefabFromName(name, out var uiObject))
    //}

    private bool TryGetDictionaryPrefabFromName(string name, out GameObject uiObject)
    {
        uiObject = null;

        foreach (KeyValuePair<string, GameObject> keyValuePair in _nameUIObjectPairs)
        {
            if (keyValuePair.Key == name)
            {
                uiObject = keyValuePair.Value;
            }
        }

        return uiObject != null;
    }

    private void BuildDictionary()
    {
        foreach (var name in _userInterfaceElements.UserInterfaces)
        {
            foreach (var prefab in _uiObject)
            {
                if (name.Name == prefab.name)
                {
                    _nameUIObjectPairs.Add(name.Name, prefab);
                }
            }
        }
    }


}
