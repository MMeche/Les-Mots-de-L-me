//using System;
//using UnityEngine;

//public class Experience : MonoBehaviour
//{
//    PlayerStatManager _statManager;

//    private int _level = 1;

//    private float _exp = 0f;

//    private float _expToNextLevel => _expCurve(_level);

//    private Func<int, float> _expCurve = (lvl) => 50f * lvl * lvl + 50f; // Example: quadratic progression

//    private void Start()
//    {
//        _level = 1;
//        _exp = 0;

//        _statManager = ServiceProvider.Instance.ServiceLocator.Get<PlayerStatManager>();
//        _statManager.PlayerEventBus.OnAddExp += AddExp;
//        _statManager.PlayerEventBus.OnSetExp += SetExp;
//        _statManager.PlayerEventBus.OnSetLvl += SetLevel;
//    }

//    /// <summary>
//    /// Sets the entity's level directly. Triggers OnLevelUp if changed.
//    /// </summary>
//    private void SetLevel(PlayerEventContext context)
//    {
//        if (_level == context.NewLvl) return;
//        int prev = _level;
//        _level = context.NewLvl;
//    }

//    /// <summary>
//    /// Adds experience points and handles multiple level-ups.
//    /// </summary>
//    private void AddExp(PlayerEventContext context)
//    {
//        _exp += context.ExpToAdd;
//        while (_exp >= _expToNextLevel)
//        {
//            _exp -= _expToNextLevel;
//            int prev = _level;
//            _level++;
//            _statManager.PlayerEventBus.LevelUp(new() { CurrentLvl = _level, FormerLvl = prev });
//        }
//        _statManager.PlayerEventBus.ChangeExp(new() { CurrentExp = _exp, ExpToNextLvl = _expToNextLevel });
//    }

//    /// <summary>
//    /// Sets the experience points directly (for loading/sync).
//    /// </summary>
//    private void SetExp(PlayerEventContext context)
//    {
//        _exp = context.NewExp;
//        _statManager?.PlayerEventBus.ChangeExp(new() { CurrentExp = _exp, ExpToNextLvl = _expToNextLevel });
//    }
//}
