using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Ink.Parsed;
using System.Collections;

public class BattleWords : MonoBehaviour
{
    BattleFlowStateMachine _bsm;
    BattleManager _bm;
    [SerializeField] TMP_Text _goalText;
    [SerializeField] TMP_Text _targetText;


    [SerializeField] PoeticMiniGameLayoutGenerator _poeticManager;
    [SerializeField] LyricMinigameLayoutGenerator _lyricGenerator;
    [SerializeField] LyricMinigameManager _lyricManager;
    bool _splineEnabled;
    bool _cursorTrigger;
    [SerializeField] QTEManager _narManager;
    int _targetWordsCompletion = 0;
    [SerializeField] BattleWordsData _wordsData;
    List<string> _words;

    bool _argIsLastState;
    void Start()
    {
        //_battleUiManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _bsm = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _bm = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();

        _bsm.BattleFlowStateMachineEventBus.OnPoeticMinigame += WordsPistActivate;
        _bsm.BattleFlowStateMachineEventBus.OnLyricMinigame += WordsPistActivate;
        _bsm.BattleFlowStateMachineEventBus.OnNarrativeMiniGame += WordsPistActivate;
        _bm.BattleManagerEventBus.OnLyricSplineEnabled += SplineEnabled;
        _bm.BattleManagerEventBus.OnPoeticOrbCaught += AddTargetWord;
        _bm.BattleManagerEventBus.OnDefensiveQTE += AddTargetWord;
        _bm.BattleManagerEventBus.OnOffensiveQTE += AddTargetWord;
        _bm.BattleManagerEventBus.OnCursorTriggerEnter += LyricTrigger;
        _bm.BattleManagerEventBus.OnCursorTriggerExit += LyricTrigger;
        
        _bsm.BattleFlowStateMachineEventBus.OnArgumentativeMinigame += WordsPistDeActivate;
        _bsm.BattleFlowStateMachineEventBus.OnPlayersTurn += WordsPistActivate;

    }

    private void WordsPistActivate(BattleFlowStateMachineEventContext ctx)
    {
        _goalText.text = "";
        _splineEnabled = false;
        _targetWordsCompletion = 0;
        int maxIndex = 0;
        _words = new();
        switch (_bsm.GetCurrentState().Name)
        {
            //case "BattleFlowState_Argumentative":
            //    _argManager.
            //    return;
            
            case "BattleFlowState_Narrative":
                maxIndex = _narManager.NumberOfDefensiveQTE + _narManager.NumberOfOffensiveQTE;             
                break;
            case "BattleFlowState_Poetic":
                maxIndex = _poeticManager.OrbNumber;
                break;
            case "BattleFlowState_Lyric":
                StartCoroutine(WaitForSplineEnable());               
                return;
        }
        if (_bsm.GetCurrentState() != _bsm.BattleStates.BattleFlowStatePlayerTurn)
        {
            _words = _wordsData.GetAlistWithLength(maxIndex);
            foreach (var w in _words)
            {
                AddGoalWord(w);
            }
        }
        if (!_argIsLastState)
        {
            StartCoroutine(NextFrame());
        }
        else _argIsLastState = false;
        
        
    }

    private void WordsPistDeActivate(BattleFlowStateMachineEventContext ctx)
    {
        _argIsLastState = true;
    }

    private void SplineEnabled(BattleManagerEventContext ctx)
    {
        _splineEnabled = true;
    }
    IEnumerator WaitForSplineEnable()
    {
        while (_splineEnabled == false)
        {
            yield return null;
        }
        int maxIndex = (int)(_lyricGenerator._splineContainer.CalculateLength() / _lyricManager.AnimationSpeed);
        _words = _wordsData.GetAlistWithLength(maxIndex);
        for (int i = 0; i < maxIndex; i++)
        {
            AddGoalWord(_words[i]);
            
        }
        StartCoroutine(NextFrame());
    }

    IEnumerator NextFrame()
    {
        yield return new WaitForEndOfFrame();
        _bsm.BattleFlowStateMachineEventBus.OnUIActivated(new()
        {
            UINames = new string[] { "WordsProgression" }
        });
    }
    private void AddGoalWord(string word)
    {
        _goalText.text += " " + word;
    }

    private void AddTargetWord(BattleManagerEventContext ctx)
    {
        if (ctx.NarrativeDamageModifier==-1 && _targetWordsCompletion<_words.Count) //Aka Ce n'est pas un event du narratif
        {
            _targetText.text += " " + _words[_targetWordsCompletion];
            
        }
        else
        {
            if((ctx.NarrativeOffensive && ctx.NarrativeDamageModifier>1) || (!ctx.NarrativeOffensive && ctx.NarrativeDamageModifier == 0)) _targetText.text += " " + _words[_targetWordsCompletion];
        }
        _targetWordsCompletion++;
    }

    private void LyricTrigger(BattleManagerEventContext ctx)
    {
        _cursorTrigger = !_cursorTrigger;
        if (_cursorTrigger && !IsInvoking(nameof(LyricStack)))
        {
            InvokeRepeating(nameof(LyricStack), 0, 1f);
        }
        //else CancelInvoke(nameof(LyricStack));
    }

    private void LyricStack()
    {
        if (_cursorTrigger)
        {
            AddTargetWord(new());
        }
    }

    public void CancelStack()
    {
        if (IsInvoking(nameof(LyricStack))) CancelInvoke(nameof(LyricStack));
    }
}
