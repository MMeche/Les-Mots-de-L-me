using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStatManager : MonoBehaviour
{
    public PlayerStatus[] PlayerStatuses;
    public PlayerEventBus PlayerEventBus;
    public PlayerEventContext PlayerEventContext;
    UIManager _uiManager;
    FlowStateMachine _fsm;
    BattleFlowStateMachine _bfsm;

    int _currentSelectedCharacter;

    private int _level = 1;

    private float _exp = 0f;

    private float _expToNextLevel => _expCurve(_level);

    private Func<int, float> _expCurve = (lvl) => 50f * lvl * lvl + 50f; // Example: quadratic progression

    private void Start()
    {
        _level = 1;
        _exp = 0;

        PlayerEventBus.OnSetExp += SetExp;
        PlayerEventBus.OnSetLvl += SetLevel;
        _uiManager = ServiceProvider.Instance.ServiceLocator.TryGet<UIManager>();
        _fsm = ServiceProvider.Instance.ServiceLocator.TryGet<FlowStateMachine>();
        _bfsm = ServiceProvider.Instance.ServiceLocator.TryGet<BattleFlowStateMachine>();

        if(_bfsm!=null) _bfsm.BattleFlowStateMachineEventBus.OnBattleWin += AddExp;
        
        if (_fsm != null)
        {
            _fsm.FlowStateMachineEventBus.Pause += SendStatsToUI;

        }
    }

    private void SendStatsToUI(FlowStateMachineEventContext obj)
    {
        _uiManager.UIEventBus.SetupUI(new()
        {
            CurrentExp = _exp,
            Level = _level,
            ExpToNextLvl = _expToNextLevel,
            CharacteIndex = _currentSelectedCharacter,
            SkillPoints = PlayerStatuses[_currentSelectedCharacter].AvailableSkillPoints,
            Narrative = PlayerStatuses[_currentSelectedCharacter].Stats[StatType.Narrative],
            Poetic = PlayerStatuses[_currentSelectedCharacter].Stats[StatType.Poetic],
            Lyric = PlayerStatuses[_currentSelectedCharacter].Stats[StatType.Lyrical],
            Argumentative = PlayerStatuses[_currentSelectedCharacter].Stats[StatType.Argumentative]
        });
    }

   

    void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
        PlayerEventBus = new();
        PlayerEventContext = new();
    }

    public void SwapChar(int increment)
    {
        _currentSelectedCharacter += increment;

        if (_currentSelectedCharacter >= PlayerStatuses.Length)
            _currentSelectedCharacter = 0;

        if (_currentSelectedCharacter < 0)
            _currentSelectedCharacter = PlayerStatuses.Length - 1;

        _exp = PlayerStatuses[_currentSelectedCharacter].Experience;
        _level = PlayerStatuses[_currentSelectedCharacter].Level;

        SendStatsToUI(new());
    }

    public void ChangeChar(int playerIndex)
    {
        _currentSelectedCharacter = playerIndex;

        _exp = PlayerStatuses[_currentSelectedCharacter].Experience;
        _level = PlayerStatuses[_currentSelectedCharacter].Level;

        SendStatsToUI(new());
    }

    public void AddPointToSkill(int statTypeIndex)
    {
        StatType currentStatType = (StatType)statTypeIndex;
        PlayerStatuses[_currentSelectedCharacter].Stats.TryGetValue(currentStatType, out int currentStatValue);

        int newStatValue = currentStatValue + 1;

        if (newStatValue <= 99 && PlayerStatuses[_currentSelectedCharacter].AvailableSkillPoints > 0)
        {
            
            int skillpoints = PlayerStatuses[_currentSelectedCharacter].AvailableSkillPoints -= 1;
            PlayerStatuses[_currentSelectedCharacter].Stats[currentStatType] = newStatValue;
            UpdateStatUI(newStatValue, statTypeIndex, skillpoints, currentStatType);
        }
    }

    public void RemovePointFromSkill(int statTypeIndex)
    {
        StatType currentStatType = (StatType)statTypeIndex;
        PlayerStatuses[_currentSelectedCharacter].Stats.TryGetValue(currentStatType, out int currentStatValue);

        int newStatValue = currentStatValue - 1;

        if (newStatValue >= 0)
        {
            int skillpoints = PlayerStatuses[_currentSelectedCharacter].AvailableSkillPoints += 1;
            PlayerStatuses[_currentSelectedCharacter].Stats[currentStatType] = newStatValue;
            UpdateStatUI(newStatValue, statTypeIndex, skillpoints, currentStatType);
        }
    }

    private void UpdateStatUI(int statValue, int statTypeIndex, int skillPoints, StatType statType)
    {
        PlayerStatuses[_currentSelectedCharacter].Stats[statType] = statValue;
        _uiManager.UIEventBus.UpdateStatDisplay(new() { StatValue = statValue, StatTypeIndex = statTypeIndex });
        _uiManager.UIEventBus.UpdateSkillPointDisplay(new() { SkillPoints = skillPoints });
    }

    private void SetLevel(PlayerEventContext context)
    {
        if (_level == context.NewLvl) return;
        int prev = _level;
        _level = context.NewLvl;
        _uiManager.UIEventBus.UpdateLvlDisplay(new() { Level = _level });
    }

    /// <summary>
    /// Adds experience points and handles multiple level-ups.
    /// </summary>
    private void AddExp(BattleFlowStateMachineEventContext context)
    {
        for (int x = 0; x < PlayerStatuses.Length; x++)
        {
            float exp = PlayerStatuses[x].Experience;
            _level = PlayerStatuses[x].Level;
            exp += context.ExpToAdd;

            while (exp >= _expToNextLevel)
            {
                exp -= _expToNextLevel;
                PlayerStatuses[x].Level += 1;
                _level = PlayerStatuses[x].Level;
                LevelUp(_level, x);
            }

            PlayerStatuses[x].Experience = exp;
        }

        _exp = PlayerStatuses[_currentSelectedCharacter].Experience;

        _uiManager.UIEventBus.UpdateExpDisplay(new() { CurrentExp = _exp, ExpToNextLvl = _expToNextLevel });
    }

    /// <summary>
    /// Sets the experience points directly (for loading/sync).
    /// </summary>
    private void SetExp(PlayerEventContext context)
    {
        _exp = context.NewExp;
        _uiManager.UIEventBus.UpdateExpDisplay(new() { CurrentExp = _exp, ExpToNextLvl = _expToNextLevel });
    }

    private void LevelUp(int level, int playerIndex)
    {
        _uiManager.UIEventBus.UpdateLvlDisplay(new() { Level = level });
        PlayerStatuses[playerIndex].AvailableSkillPoints += 3;
        PlayerStatuses[playerIndex].Level = level;
        _uiManager.UIEventBus.UpdateSkillPointDisplay(new() { SkillPoints = PlayerStatuses[playerIndex].AvailableSkillPoints });
    }
}
