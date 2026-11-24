using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class LyricMinigameManager : MonoBehaviour
{
    public float AnimationSpeed;
    public float EnemyDamage;
    float _damage=1.5f;

    SplineAnimate _knobAnimation;
    bool _alternativePathEntered=false;
    bool _mainPathEntered = false;
    

    BattleManager _battleManager;
    BattleUIManager _battleUIManager;
    BattleFlowStateMachine _battleFlowStateMachine;
    FlowStateMachine _fsm;

    Coroutine _enemyIncomingDamageCoroutine;
    Coroutine _damageOverTime;


    [SerializeField] GameObject _playerFeedbackBurst;
    [SerializeField] GameObject _playerFeedpackTrail;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _knobAnimation = transform.GetComponentInChildren<SplineAnimate>();

        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();

        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.OnLyricMinigame += EnableLayout;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose += ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead += PlayerDead;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += ExitingLyricMinigame;
        _battleManager.BattleManagerEventBus.OnCursorTriggerEnter += CursorTriggerEnter;
        _battleManager.BattleManagerEventBus.OnCursorTriggerExit += CursorTriggerExit;
        _battleManager.BattleManagerEventBus.OnKnobEndReached += KnobEndReached;
        _fsm.FlowStateMachineEventBus.Pause += PauseActivate;

        _knobAnimation.AnimationMethod = SplineAnimate.Method.Speed;
        _knobAnimation.Easing = SplineAnimate.EasingMode.None;
        _knobAnimation.MaxSpeed = AnimationSpeed;
        _damageOverTime = null;

    }





    void EnableLayout(BattleFlowStateMachineEventContext obj)
    {
        GetComponent<LyricMinigameLayoutGenerator>().enabled = true;
    }


    private void PlayerDead(BattleFlowStateMachineEventContext obj)
    {
        GetComponentInChildren<LyricKnobLogic>().SendDamage();
    }

    private void ExitingLyricMinigame(BattleFlowStateMachineEventContext obj)
    {
        transform.gameObject.SetActive(false);
    }

    void CursorTriggerEnter(BattleManagerEventContext ctx)
    {
        if(!_knobAnimation.IsPlaying) StartMinigame();
        if (ctx.Type != KnobType.classic)
        {
            _alternativePathEntered = true;
            return;
        }
        _mainPathEntered = true;
        _enemyIncomingDamageCoroutine = StartCoroutine(DisplayPlayerDamageOverTime());
    }


    //Les dégâts que va infliger le joueur à l'ennemi. Sont calculés à partir de la distance parcourue (avec le curseur ciblé évidemment).
    IEnumerator DisplayPlayerDamageOverTime()
    {
        float begin = Time.time;
        InvokeRepeating("SpawnFeedbackBurst",0.25f,0.5f);
        while (true)
        {
            BattleUIEventContext ctx = new() { 
                IncomingPlayerDamage = ((Time.time - begin) * (_damage + (1 * _battleManager.GetActivePlayerStatus().Level)) 
                                       *(1 + 0.1f * _battleManager.GetActivePlayerStatus().Stats[StatType.Lyrical])), 
                StackDamage = true };       
            begin = Time.time;            
            _battleUIManager.BattleUIEventBus.UpdateEnemyIncomingDamage(ctx);
            yield return null;
        }
        
        
    }

    void SpawnFeedbackBurst()
    {
        Instantiate(_playerFeedbackBurst, _knobAnimation.transform.position, Camera.main.transform.rotation);
    }

    void CursorTriggerExit(BattleManagerEventContext ctx)
    {
        if (ctx.Type != KnobType.classic)
        {
            _alternativePathEntered = false;
            return;
        }
        _mainPathEntered = false;
        StopCoroutine(_enemyIncomingDamageCoroutine);
        CancelInvoke("SpawnFeedbackBurst");
        if(_damageOverTime==null)_damageOverTime= StartCoroutine(ApplyEnemyDamageOverTime());
    }

    IEnumerator ApplyEnemyDamageOverTime()
    {
        BattleManagerEventContext ctx = new() { DamageDone = EnemyDamage };
        while (true)
        {
            
            if(!_mainPathEntered)_battleManager.BattleManagerEventBus.AttackPlayer(ctx);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void KnobEndReached(BattleManagerEventContext msg)
    {
        CursorTriggerExit(new BattleManagerEventContext() { Type = KnobType.classic});
        StopAllCoroutines();
        CancelInvoke("SpawnFeedbackBurst");
        if (msg.Type == KnobType.classic)
        {
            BattleManagerEventContext ctx = new() { DamageDone = msg.ElapsedTimeHoveringKnob * (_damage + (1 * _battleManager.GetActivePlayerStatus().Level))
                                       * (1 + 0.1f * _battleManager.GetActivePlayerStatus().Stats[StatType.Lyrical])
            };
            _battleManager.BattleManagerEventBus.AttackEnemy(ctx);

            _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
        }
    }

    private void PauseActivate(FlowStateMachineEventContext ctx)
    {
        //GameObject game = transform.GetChild(0).gameObject;
        //game.SetActive(!game.activeSelf);
    }


    void StartMinigame()
    {
        _knobAnimation.enabled = false;
        _knobAnimation.enabled = true;
        _knobAnimation.Play();
    }


    void OnDisable()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead -= PlayerDead;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin -= ExitingLyricMinigame;
        _battleManager.BattleManagerEventBus.OnCursorTriggerEnter -= CursorTriggerEnter;
        _battleManager.BattleManagerEventBus.OnCursorTriggerExit -= CursorTriggerExit;
        _battleManager.BattleManagerEventBus.OnKnobEndReached -= KnobEndReached;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<LyricMinigameManager>();
    }

    void OnDestroy()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= ExitingLyricMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead -= PlayerDead;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin -= ExitingLyricMinigame;
        _battleManager.BattleManagerEventBus.OnCursorTriggerEnter -= CursorTriggerEnter;
        _battleManager.BattleManagerEventBus.OnCursorTriggerExit -= CursorTriggerExit;
        _battleManager.BattleManagerEventBus.OnKnobEndReached -= KnobEndReached;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<LyricMinigameManager>();
    }

    public void SetAnimationDuration(float speed)
    {
        AnimationSpeed = speed;
        _knobAnimation.MaxSpeed = AnimationSpeed;
    }
}
