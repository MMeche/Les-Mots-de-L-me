using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PoeticMiniGameLayoutGenerator))]
public class PoeticMinigameManager : MonoBehaviour
{
    public int ContactAllowed = 0;
    public int EnemyDamage;
    
    int _contactOccured = 0;
    int _orbCollected = 0;
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleManager _battleManager;
    FlowStateMachine _fsm;

    void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();

        _contactOccured = 0;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPoeticMinigame += EnableLayout;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose += ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead += ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += ExitingPoeticMinigame;
        _battleManager.BattleManagerEventBus.OnPoeticCollisionOccured += PoeticCollisionOccured;
        _battleManager.BattleManagerEventBus.OnPoeticEndTriggered += PoeticEndTriggered;
        _battleManager.BattleManagerEventBus.OnPoeticOrbCaught += PoeticOrbCaught;
        _fsm.FlowStateMachineEventBus.Pause += PauseActivate;

        
    }

    private void EnableLayout(BattleFlowStateMachineEventContext obj)
    {
        GetComponent<PoeticMiniGameLayoutGenerator>().enabled = true;
    }

    private void ExitingPoeticMinigame(BattleFlowStateMachineEventContext obj)
    {
        if (_battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn)
        {
            _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
        }
        transform.gameObject.SetActive(false);
    }

    // Messages Broadcasted by the layouts elements (orbs, obstacles, endTrigger)
    void PoeticCollisionOccured(BattleManagerEventContext context)
    {
        _contactOccured++;
        //Il faut infliger des dégâts au joueur.
        if (_contactOccured > ContactAllowed && _battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn)
        {
            BattleManagerEventContext ctx = new() { DamageDone = EnemyDamage};
            _battleManager.BattleManagerEventBus.AttackPlayer(ctx);
        }
    }
    void PoeticEndTriggered(BattleManagerEventContext context)
    {
        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
    }
    void PoeticOrbCaught(BattleManagerEventContext context)
    {
        _orbCollected++;
        //Il faut infliger des dégâts à l'ennemi
        BattleManagerEventContext ctx = new() { DamageDone = (15 + (_battleManager.GetActivePlayerStatus().Level - 1 * 5)) * (1 + 0.1f * _battleManager.GetActivePlayerStatus().Stats[StatType.Poetic]) };
        _battleManager.BattleManagerEventBus.AttackEnemy(ctx);   
    }

    private void PauseActivate(FlowStateMachineEventContext ctx)
    {
        GameObject kid;
        for(int i = 0; i < transform.childCount; i++)
        {
            kid = transform.GetChild(i).gameObject;
            kid.SetActive(!kid.activeSelf);
        }
    }

    //
    void OnDisable()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin -= ExitingPoeticMinigame;
        _battleManager.BattleManagerEventBus.OnPoeticCollisionOccured -= PoeticCollisionOccured;
        _battleManager.BattleManagerEventBus.OnPoeticEndTriggered -= PoeticEndTriggered;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<PoeticMinigameManager>();
    }

    void OnDestroy()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead -= ExitingPoeticMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin -= ExitingPoeticMinigame;
        _battleManager.BattleManagerEventBus.OnPoeticCollisionOccured -= PoeticCollisionOccured;
        _battleManager.BattleManagerEventBus.OnPoeticEndTriggered -= PoeticEndTriggered;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<PoeticMinigameManager>();
    }
}
