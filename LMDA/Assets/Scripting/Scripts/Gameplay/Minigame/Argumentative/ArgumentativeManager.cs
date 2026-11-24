using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using NUnit.Framework;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;

public class ArgumentativeManager : MonoBehaviour
{
    [Header("Public atomic parameters")]
    public int MaxNbOfDefensiveCard;
    public int MaxNbOfOffensiveCard;
    public int NbOfNeutralCard;
    public int NbOfSelectionSlot;
    public float EnemyDamage;
    int _damage = 3;

    public List<string> EnemyHintSentences;
    public ArgComboData EnemyLoadedCombo;

    [Header("Intern logic parameter")]
    [SerializeField] GameObject _cardTemplate;
    [SerializeField] GameObject _enemyCardTemplate;
    [SerializeField] GameObject _slotTemplate;
    [SerializeField] GameObject _enemyHintTemplate;
    [SerializeField] GameObject _validationButton;
    [SerializeField] Transform _cardGrid;
    [SerializeField] Transform _word;
    [SerializeField] Transform _enemyWord;
    [SerializeField] SelectionGridLogic _selectionGrid;
    [SerializeField] Transform _enemyComboGrid;
    [SerializeField] Transform _enemyHintGrid;
    [SerializeField] Transform _playerHint;
    [SerializeField] GameObject _comboInfo;
    [SerializeField] GameObject _enemyComboInfo;

    [Header("Feedback")]
    [SerializeField] GameObject _playerFeedbackSquareBurst;

    int[] _rdRepartition;

    List<ArgWordCardLogic> _argCards;
    List<GameObject> _argSlots;

    List<ArgComboData> _comboDatas;

    RectTransform _displayZone;
    float _width;
    float _height;

    BattleManager _battleManager;
    BattleFlowStateMachine _battleFlowStateMachine;
    BattleUIManager _battleUIManager;
    FlowStateMachine _fsm;

    List<string> _offensiveWords;
    List<string> _defensiveWords;
    List<string> _neutralWords;

    private void OnEnable()
    {
        _displayZone = transform.GetChild(0).GetComponent<RectTransform>();
        _width = _displayZone.rect.width;
        _height = _displayZone.rect.height;

        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();

        _rdRepartition = GenerateRandomTernaryArray(MaxNbOfDefensiveCard, MaxNbOfOffensiveCard,NbOfNeutralCard); // 0 = defensive then 1 = offensive then 2 = neutral
        _argCards = new();
        _argSlots = new();
        _comboDatas = new();
        _word.GetComponent<TMP_Text>().text = "";
        _enemyWord.GetComponent<TMP_Text>().text = "";

        //List<ArgCardType> firstCombo = new();
        //firstCombo.Add(ArgCardType.offensive);
        //firstCombo.Add(ArgCardType.defensive);
        //firstCombo.Add(ArgCardType.offensive);

        //ArgComboData newCombo = (ArgComboData)ScriptableObject.CreateInstance("ArgComboData");
        //newCombo.EnemiDamageModifier = 0.9f;
        //newCombo.ComboProfile = firstCombo;
        //newCombo.DamageModifier = 1.2f;
        foreach (var combo in _battleManager.GetActivePlayerStatus().ComboList)_comboDatas.Add(combo);

        _offensiveWords = _battleManager.GetEnemyStatus().WordList.WeaknessWords;
        _defensiveWords = _battleManager.GetEnemyStatus().WordList.CounterWords;
        _neutralWords = _battleManager.GetEnemyStatus().WordList.NeutralWords;

        PopulateCardsList();
        PopulateSlotsList();
        DisplayEnemyCombo();
        //PopulateHintsList();
        ComboUpdate(new() { LoadedCombo = new()});

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += ExitingArgMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose += ExitingArgMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += ExitingArgMinigame;
        _battleUIManager.BattleUIEventBus.OnArgWordCardClicked += ArgCardClicked;
        _battleManager.BattleManagerEventBus.OnComboUpdate += ComboUpdate;
        _battleManager.BattleManagerEventBus.OnComboAchieved += ComboAchieved;
        _fsm.FlowStateMachineEventBus.Pause += PauseActivate;
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
    }


    void PopulateCardsList()
    {
        int NbOfDefensiveCard = UnityEngine.Random.Range(0, MaxNbOfDefensiveCard+1);
        int NbOfOffensiveCard = UnityEngine.Random.Range(1, MaxNbOfOffensiveCard+1);
        //for (int i = 0; i < NbOfDefensiveCard+NbOfOffensiveCard+NbOfNeutralCard; i++)
        //{
        //    GameObject newObject = Instantiate(_cardTemplate,_cardGrid);
        //    ArgWordCardLogic newCard = newObject.GetComponent<ArgWordCardLogic>();
        //    newCard.SetArgCardType((ArgCardType)_rdRepartition[i]);
        //    ColorBlock cb = newObject.GetComponent<Button>().colors;

        //    if(newCard.GetArgCardType()==ArgCardType.offensive)cb.normalColor = new Color(1f, 0, 0);
        //    else if(newCard.GetArgCardType() == ArgCardType.defensive) cb.normalColor = new Color(0, 1f, 0);

        //    cb.selectedColor = cb.normalColor;
        //    newObject.GetComponent<Button>().colors = cb;

        //    //newCard.SetInputFieldText(FetchWordFromList(newCard.GetArgCardType()));
        //    _argCards.Add(newObject);
        //}

        for(int i =0;i< NbOfDefensiveCard; i++)
        {
            GameObject newObject = Instantiate(_cardTemplate, _cardGrid);
            ArgWordCardLogic newCard = newObject.GetComponent<ArgWordCardLogic>();
            newCard.SetArgCardType(ArgCardType.defensive);
            _argCards.Add(newObject.GetComponent<ArgWordCardLogic>());
        }
        for (int i = 0; i < NbOfOffensiveCard; i++)
        {
            GameObject newObject = Instantiate(_cardTemplate, _cardGrid);
            ArgWordCardLogic newCard = newObject.GetComponent<ArgWordCardLogic>();
            newCard.SetArgCardType(ArgCardType.offensive);
            _argCards.Add(newObject.GetComponent<ArgWordCardLogic>());
        }
        //for (int i = 0; i < NbOfNeutralCard; i++)
        //{
        //    GameObject newObject = Instantiate(_cardTemplate, _cardGrid);
        //    ArgWordCardLogic newCard = newObject.GetComponent<ArgWordCardLogic>();
        //    newCard.SetArgCardType(ArgCardType.neutral);
        //    _argCards.Add(newObject.GetComponent<ArgWordCardLogic>());
        //}
    }

    void PopulateSlotsList()
    {
        for(int i = 0; i < NbOfSelectionSlot; i++)
        {
            GameObject newObject = Instantiate(_slotTemplate, _selectionGrid.transform);
            newObject.transform.SetAsFirstSibling();
            _argSlots.Add(newObject);
        }
    }

    void DisplayEnemyCombo()
    {
        foreach(var card in EnemyLoadedCombo.ComboProfile)
        {
            Instantiate(_enemyCardTemplate, _enemyComboGrid).GetComponent<ArgWordCardLogic>().SetArgCardType(card);
        }
        _enemyComboInfo.GetComponent<ComboInfoDisplay>().UpdateEnemyDisplay((int)EnemyLoadedCombo.EnemyComboDamage);
    }

    void PopulateHintsList()
    {
        foreach(var hint in EnemyHintSentences)
        {
            GameObject newHint = Instantiate(_enemyHintTemplate, _enemyHintGrid);
            newHint.transform.GetChild(0).GetComponent<TMP_Text>().text = hint;
        }
    }

    void SetEnemyText(float PlayerDamage)
    {
        _enemyHintGrid.GetComponent<TMP_Text>().text = "Va recevoir "+(int)PlayerDamage+" dégâts";
    }

    void SetPlayerText(float EnemyDamage)
    {
        _playerHint.GetComponent<TMP_Text>().text = "Va recevoir " + (int)EnemyDamage + " dégâts";
    }

    void ArgCardClicked(BattleUIEventContext ctx) 
    {
        //if(ctx.Card.parent == _cardGrid)
        //{
            
        //    return;
        //}
        if(ctx.Card.parent.parent == _selectionGrid.transform)
        {
            if (ctx.Card.GetComponent<ArgWordCardLogic>().GetArgCardType() == ArgCardType.neutral) Destroy(ctx.Card.gameObject);
            else ctx.Card.SetParent(_cardGrid,false);
            _selectionGrid.GetComponent<SelectionGridLogic>().UpdateEmptySlots(ctx.Card);
            return;
        }
        _selectionGrid.GetComponent<SelectionGridLogic>().AddCard(ctx.Card);
    }

    void ComboUpdate(BattleManagerEventContext ctx)
    {
        float totalDamage = (_damage + (_battleManager.GetActivePlayerStatus().Level*4))*(1+0.1f* _battleManager.GetActivePlayerStatus().Stats[StatType.Argumentative]);
        float totalEnemiDamage = EnemyDamage*EnemyLoadedCombo.EnemyComboDamage;
        //foreach (var targetCombo in _comboDatas)
        //{
        //    bool validation = combo
        //                      .SkipWhile(x => x != targetCombo.ComboProfile[0])
        //                      .Take(targetCombo.ComboProfile.Count)
        //                      .SequenceEqual(targetCombo.ComboProfile);
        //    if (validation)
        //    {
        //        totalDamage += targetCombo.DamageModifier * Damage;
        //        totalEnemiDamage += targetCombo.EnemiDamageModifier*EnemyDamage;
        //    }
        //}
        int totalAtkModifier =0;
        int totalDefModifier = 0;
        foreach (var targetCombo in _comboDatas)
        {
            int comboCount = 0;

            for (int i = 0; i <= ctx.LoadedCombo.Count - targetCombo.ComboProfile.Count; i++)
            {
                // Take a slice of 'combo' starting at i with the same length as the profile
                var slice = ctx.LoadedCombo.Skip(i).Take(targetCombo.ComboProfile.Count);

                // Check if this slice matches the combo profile
                if (slice.SequenceEqual(targetCombo.ComboProfile))
                {
                    comboCount++;
                }
            }

            if (comboCount > 0)
            {
                totalDamage += comboCount * targetCombo.DamageModifier ;
                totalAtkModifier += comboCount * (int)targetCombo.DamageModifier;
                totalEnemiDamage += comboCount * targetCombo.EnemiDamageModifier;
                totalDefModifier += comboCount * (int)targetCombo.EnemiDamageModifier;
            }

        }

        UpdateComboInfo(totalAtkModifier, totalDefModifier);
        //Rock Paper Scissors Logic
        if (EnemyLoadedCombo.ComboProfile.Count <= ctx.LoadedCombo.Count)
        {
            int dif = ctx.LoadedCombo.Count - EnemyLoadedCombo.ComboProfile.Count;
            for(int i = 0; i < ctx.LoadedCombo.Count - dif; i++)
            {
                switch (ctx.LoadedCombo[i])
                {
                    case ArgCardType.offensive:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.defensive)
                        {
                            totalDamage *= 0.8f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.neutral)
                        {
                            totalDamage *= 1.2f;
                        }
                        break;
                    case ArgCardType.defensive:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.neutral)
                        {
                            totalEnemiDamage *= 1.1f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.offensive)
                        {
                            totalEnemiDamage *= 0.8f;
                        }
                        break;
                    case ArgCardType.neutral:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.defensive)
                        {
                            totalDamage *= 1.1f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.offensive)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        break;
                }
            }
            if (dif != 0)
            {
                totalDamage *= 1 + (dif * .05f);
                totalEnemiDamage *= 1 + (dif * -.05f);
            }
        }
        else
        {
            int dif =  EnemyLoadedCombo.ComboProfile.Count - ctx.LoadedCombo.Count;
            for (int i = 0; i < EnemyLoadedCombo.ComboProfile.Count - dif; i++)
            {
                switch (EnemyLoadedCombo.ComboProfile[i])
                {
                    case ArgCardType.offensive:
                        if (ctx.LoadedCombo[i] == ArgCardType.defensive)
                        {
                            totalEnemiDamage *= 0.9f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.neutral)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        break;
                    case ArgCardType.defensive:
                        if (ctx.LoadedCombo[i] == ArgCardType.neutral)
                        {
                            totalDamage *= 1.1f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.offensive)
                        {
                            totalDamage *= 0.8f;
                        }
                        break;
                    case ArgCardType.neutral:
                        if (ctx.LoadedCombo[i] == ArgCardType.defensive)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.offensive)
                        {
                            totalDamage *= 1.2f;
                        }
                        break;
                }
            }
            totalEnemiDamage *= 1 + (dif * .05f);
            totalDamage *= 1 + (dif * -.05f);
        }
        _battleUIManager.BattleUIEventBus.UpdateEnemyIncomingDamage(new() { IncomingPlayerDamage = totalDamage,StackDamage =false });
        _battleUIManager.BattleUIEventBus.UpdatePlayerIncomingDamage(new() { IncomingEnemyDamage = totalEnemiDamage,StackDamage =false });
        SetEnemyText(totalDamage);
        SetPlayerText(totalEnemiDamage);
    }

    void ComboAchieved(BattleManagerEventContext ctx)
    {
        float totalDamage = (_damage + (_battleManager.GetActivePlayerStatus().Level * 4))*(1 + 0.1f * _battleManager.GetActivePlayerStatus().Stats[StatType.Argumentative]);
        float totalEnemiDamage = EnemyDamage * EnemyLoadedCombo.EnemyComboDamage;
        foreach (var targetCombo in _comboDatas)
        {
            int comboCount = 0;

            for (int i = 0; i <= ctx.LoadedCombo.Count - targetCombo.ComboProfile.Count; i++)
            {
                // Take a slice of 'combo' starting at i with the same length as the profile
                var slice = ctx.LoadedCombo.Skip(i).Take(targetCombo.ComboProfile.Count);

                // Check if this slice matches the combo profile
                if (slice.SequenceEqual(targetCombo.ComboProfile))
                {
                    comboCount++;
                }
            }

            if (comboCount > 0)
            {
                totalDamage += comboCount * targetCombo.DamageModifier;
                totalEnemiDamage += comboCount * targetCombo.EnemiDamageModifier;
            }
        }
        //Rock Paper Scissors Logic
        if (EnemyLoadedCombo.ComboProfile.Count <= ctx.LoadedCombo.Count)
        {
            int dif = ctx.LoadedCombo.Count - EnemyLoadedCombo.ComboProfile.Count;
            for (int i = 0; i < ctx.LoadedCombo.Count - dif; i++)
            {
                switch (ctx.LoadedCombo[i])
                {
                    case ArgCardType.offensive:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.defensive)
                        {
                            totalDamage *= 0.8f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.neutral)
                        {
                            totalDamage *= 1.2f;
                        }
                        break;
                    case ArgCardType.defensive:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.neutral)
                        {
                            totalEnemiDamage *= 1.1f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.offensive)
                        {
                            totalEnemiDamage *= 0.8f;
                        }
                        break;
                    case ArgCardType.neutral:
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.defensive)
                        {
                            totalDamage *= 1.1f;
                        }
                        if (EnemyLoadedCombo.ComboProfile[i] == ArgCardType.offensive)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        break;
                }
            }
            if (dif != 0)
            {
                totalDamage *= 1 + (dif * .05f);
                totalEnemiDamage *= 1 + (dif * -.05f);
            }
        }
        else
        {
            int dif = EnemyLoadedCombo.ComboProfile.Count - ctx.LoadedCombo.Count;
            for (int i = 0; i < EnemyLoadedCombo.ComboProfile.Count - dif; i++)
            {
                switch (EnemyLoadedCombo.ComboProfile[i])
                {
                    case ArgCardType.offensive:
                        if (ctx.LoadedCombo[i] == ArgCardType.defensive)
                        {
                            totalEnemiDamage *= 0.9f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.neutral)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        break;
                    case ArgCardType.defensive:
                        if (ctx.LoadedCombo[i] == ArgCardType.neutral)
                        {
                            totalDamage *= 1.1f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.offensive)
                        {
                            totalDamage *= 0.8f;
                        }
                        break;
                    case ArgCardType.neutral:
                        if (ctx.LoadedCombo[i] == ArgCardType.defensive)
                        {
                            totalEnemiDamage *= 1.2f;
                        }
                        if (ctx.LoadedCombo[i] == ArgCardType.offensive)
                        {
                            totalDamage *= 1.2f;
                        }
                        break;
                }
            }
            totalEnemiDamage *= 1 + (dif * .05f);
            totalDamage *= 1 + (dif * -.05f);
        }
        StartCoroutine(LaunchCombo(totalDamage,totalEnemiDamage));
        
    }

    private void UpdateComboInfo(int atkModifier,int defModifier)
    {
        _comboInfo.GetComponent<ComboInfoDisplay>().UpdateDisplay(atkModifier, defModifier);
    }
    IEnumerator LaunchCombo(float playerDamage,float enemyDamage)
    {
        bool first = true;

        foreach(var card in _selectionGrid.GetComponent<SelectionGridLogic>().GetArgCardList())
        {
            _word.GetComponent<TMP_Text>().text += FetchWordFromList(card.GetArgCardType());
            if (first)
            {
                _word.GetComponent<TMP_Text>().text = char.ToUpper(_word.GetComponent<TMP_Text>().text[0]) + _word.GetComponent<TMP_Text>().text.Substring(1);
                first = false;
            }
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, card.transform.GetComponent<RectTransform>().position);
            Vector3 _cardPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane + 10f)
            );
            Instantiate(_playerFeedbackSquareBurst, _cardPosition, Camera.main.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform card in _enemyComboGrid)
        {
            _enemyWord.GetComponent<TMP_Text>().text += FetchWordFromList(card.GetComponent<ArgWordCardLogic>().GetArgCardType());
            if (!first)
            {
                _enemyWord.GetComponent<TMP_Text>().text = char.ToUpper(_enemyWord.GetComponent<TMP_Text>().text[0]) + _enemyWord.GetComponent<TMP_Text>().text.Substring(1);
                first = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.75f);

        BattleManagerEventContext ctx = new() { DamageDone = playerDamage };
        _battleManager.BattleManagerEventBus.AttackEnemy(ctx);

        BattleManagerEventContext enemiCtx = new() { DamageDone = enemyDamage };
        _battleManager.BattleManagerEventBus.AttackPlayer(enemiCtx);

        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
    }

    private string FetchWordFromList(ArgCardType type)
    {
        //An upcoming function to get a word from a defensive/offensive list
        var random = new System.Random();
        return type switch
        {
            ArgCardType.offensive => _offensiveWords[random.Next(_offensiveWords.Count)],
            ArgCardType.defensive => _defensiveWords[random.Next(_defensiveWords.Count)],
            _ => _neutralWords[random.Next(_neutralWords.Count)],
        };
    }

    int[] GenerateRandomTernaryArray(int i, int j, int k)
    {
        int size = i + j + k;
        int[] arr = new int[size];

        int index = 0;

        // i zéros
        for (int x = 0; x < i; x++)
            arr[index++] = 0;

        // j uns
        for (int x = 0; x < j; x++)
            arr[index++] = 1;

        // k deux
        for (int x = 0; x < k; x++)
            arr[index++] = 2;

        // Mélanger (Fisher-Yates)
        System.Random rng = new System.Random();
        for (int x = size - 1; x > 0; x--)
        {
            int r = rng.Next(x + 1);
            (arr[x], arr[r]) = (arr[r], arr[x]);
        }

        return arr;
    }

    private void PauseActivate(FlowStateMachineEventContext ctx)
    {
        GameObject game = transform.GetChild(0).gameObject;
        game.SetActive(!game.activeSelf);
    }

    void OnDisable()
    {
        _argSlots.Clear();
        _argCards.Clear();
        for(int i = 0; i < _cardGrid.childCount; i++)
        {
            Destroy(_cardGrid.GetChild(i).gameObject);
        }
        for (int j = 0; j < _selectionGrid.transform.childCount; j++)
        {
            Destroy(_selectionGrid.transform.GetChild(j).gameObject);
        }
        for(int k = 0; k < _enemyHintGrid.childCount; k++)
        {
            Destroy(_enemyHintGrid.GetChild(k).gameObject);
        }
        EnemyHintSentences.Clear();
        foreach (Transform child in _enemyComboGrid)
        {
            Destroy(child.gameObject);
        }
        _battleUIManager.BattleUIEventBus.OnArgWordCardClicked -= ArgCardClicked;
        _battleManager.BattleManagerEventBus.OnComboUpdate -= ComboUpdate;
        _battleManager.BattleManagerEventBus.OnComboAchieved -= ComboAchieved;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingArgMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleLose -= ExitingArgMinigame;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin -= ExitingArgMinigame;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<ArgumentativeManager>();
    }

    void OnDestroy()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn -= ExitingArgMinigame;
        _fsm.FlowStateMachineEventBus.Pause -= PauseActivate;
        ServiceProvider.Instance.ServiceLocator.Unregister<ArgumentativeManager>();
    }

    private void ExitingArgMinigame(BattleFlowStateMachineEventContext obj)
    {
        transform.gameObject.SetActive(false);
    }
}
