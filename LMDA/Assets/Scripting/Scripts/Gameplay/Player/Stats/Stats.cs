//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Stats : MonoBehaviour
//{
//    private PlayerStatus[] statConfig;
//    private PlayerStatManager _statManager;

//    private void Start()
//    {
//        _statManager = ServiceProvider.Instance.ServiceLocator.Get<PlayerStatManager>();

//        _statManager.PlayerEventBus.OnStatChanged += LevelStat;
//        _statManager.PlayerEventBus.OnModifierAdded += AddModifier;
//        _statManager.PlayerEventBus.OnModifierRemoved += RemoveModifier;
//        _statManager.PlayerEventBus.OnLevelUp += AddSkillPoints;
//    }

//    private void AddSkillPoints(PlayerEventContext context)
//    {
        
//    }

//    private void LevelStat(PlayerEventContext context)
//    {

//    }

//    private void AddModifier(PlayerEventContext context)
//    {
        
//    }

//    private void RemoveModifier(PlayerEventContext context)
//    {
        
//    }
//}

//public enum StatModifierType
//{
//    Base,
//    Flat,
//    Increased,
//    Decreased,
//    More,
//    Less
//}