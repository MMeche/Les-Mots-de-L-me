using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class QTEManager : MonoBehaviour
{
    public int NumberOfOffensiveQTE;
    public int NumberOfDefensiveQTE;
    public int EnemyDamage = 5;

    [SerializeField] GameObject _offensiveQTE;
    [SerializeField] GameObject _defensiveQTE;

    [Header("Spawn postion parameters")]
    [SerializeField, Range(0f, 1f)] public float _minWidthSpawnDistance;
    [SerializeField, Range(0f, 1f)] public float _maxWidthSpawnDistance;
    [SerializeField, Range(0f, 1f)] public float _minHeightSpawnDistance;
    [SerializeField, Range(0f, 1f)] public float _maxHeightSpawnDistance;

    [Header("Spawn interval parameters")]
    [SerializeField, Range(0.5f, 3f)] public float _maxSpawnTimeInterval;
    [SerializeField, Range(0.5f, 3f)] public float _minSpawnTimeInterval;

    private PlayerInputs _playerInputs;
    BattleManager _battleManager;
    BattleFlowStateMachine _battleFlowStateMachine;
    FlowStateMachine _fsm;

    Dictionary<int,GameObject> _qteInputPairs;

    private void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.Pause += PauseActivate;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ExitingNarrativeMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerDead += Lose;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose += Lose;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += ExitingNarrativeMinigame;

        _playerInputs = new();
        _playerInputs.Disable();
        _playerInputs.Narrative.Enable();

        _qteInputPairs = new();
        StartCoroutine(SpawnQTEs());
    }

    IEnumerator SpawnQTEs()
    {
        System.Random random = new System.Random();
        bool isOffensive = true;
        int count = 0;

        while (NumberOfDefensiveQTE > 0 || NumberOfOffensiveQTE > 0)
        {
            isOffensive = random.Next(2) == 0;

            Debug.Log($"QTE{count} is {isOffensive}");
            count++;

            InstantiateQTE(isOffensive);

            yield return new WaitForSeconds(UnityEngine.Random.Range(_minSpawnTimeInterval, _maxSpawnTimeInterval));
        }

        while (NumberOfDefensiveQTE <= 0 && NumberOfOffensiveQTE <= 0 && _qteInputPairs.Count > 0)
        {
            yield return null;
        }

        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
    }

    private void InstantiateQTE(bool isOffensive)
    {
        GameObject instantiatedObject;

        if (isOffensive || (NumberOfDefensiveQTE <= 0 && NumberOfOffensiveQTE > 0))
        {
            instantiatedObject = Instantiate(_offensiveQTE, gameObject.transform);
            NumberOfOffensiveQTE--;
            isOffensive = true;
        }
        else
        {
            instantiatedObject = Instantiate(_defensiveQTE, gameObject.transform);
            NumberOfDefensiveQTE--;
        }
        SetupQTEInfo(instantiatedObject, isOffensive);
    }

    private void SetupQTEInfo(GameObject instantiatedObject, bool isOffensive)
    {
        QTE qte = instantiatedObject.GetComponentInChildren<QTE>();

        System.Random random = new System.Random();
        int randomKeyValue = random.Next(4);

        while (_qteInputPairs.ContainsKey(randomKeyValue))
        {
            randomKeyValue = random.Next(4);
        }

        qte.SetupQTEInfo(randomKeyValue, _playerInputs, !isOffensive, this);
        _qteInputPairs.Add(randomKeyValue, instantiatedObject);

        InitNewObjectPosition(instantiatedObject);
    }

    private void InitNewObjectPosition(GameObject instantiatedObject)
    {
        RectTransform rectTransform = instantiatedObject.GetComponent<RectTransform>();

        float adjustedWith = Screen.width - (rectTransform.rect.width / 2);
        float adjustedHeight = (Screen.height - (rectTransform.rect.height / 2)) * -1;

        float minWidth = adjustedWith * 0.4f;
        float maxWidth = adjustedWith * 0.6f;

        float minHeight = adjustedHeight * 0.4f;
        float maxHeight = adjustedHeight * 0.6f;

        float posX = UnityEngine.Random.Range(minWidth, maxWidth);
        float posY = UnityEngine.Random.Range(minHeight, maxHeight);

        rectTransform.anchoredPosition = new Vector2(posX, posY);
    }

    public void DamagePlayer(float damageMultiplier, int keyValue)
    {
        _qteInputPairs.Remove(keyValue);
        _battleManager.BattleManagerEventBus.DefensiveQTE(new() { NarrativeDamageModifier = damageMultiplier,NarrativeOffensive = false });
        _battleManager.BattleManagerEventBus.AttackPlayer(new() { DamageDone = EnemyDamage * damageMultiplier});
    }

    public void DamageEnemy(float damageMultiplier, int keyValue)
    {
        _qteInputPairs.Remove(keyValue);
        _battleManager.BattleManagerEventBus.OffensiveQTE(new() { NarrativeDamageModifier = damageMultiplier,NarrativeOffensive=true });
        _battleManager.BattleManagerEventBus.AttackEnemy(new() { DamageDone = ((5 + (2*_battleManager.GetActivePlayerStatus().Level-1)) * damageMultiplier) * (1 + 0.1f * _battleManager.GetActivePlayerStatus().Stats[StatType.Narrative]) });
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

    private void ExitingNarrativeMinigame(BattleFlowStateMachineEventContext obj)
    {
        gameObject.SetActive(false);
    }

    private void Lose(BattleFlowStateMachineEventContext obj)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        if (_battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn) _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);      
    }

    private void OnDisable()
    {
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingNarrativeMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= Lose;

        _playerInputs.Disable();

        ServiceProvider.Instance.ServiceLocator.Unregister<QTEManager>();
    }
}
