using System;
using UnityEngine;

public class BattleManagerEventBus
{
    public event Action<BattleManagerEventContext> OnPlayerAttacked;
    public void AttackPlayer(BattleManagerEventContext context) => OnPlayerAttacked?.Invoke(context);

    public event Action<BattleManagerEventContext> OnEnemyAttacked;
    public void AttackEnemy(BattleManagerEventContext context) => OnEnemyAttacked?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPlayerDefence;
    public void Defend(BattleManagerEventContext context) => OnPlayerDefence?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPlayerPoetic;
    public void PoeticMinigame(BattleManagerEventContext context) => OnPlayerPoetic?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPlayerLyric;
    public void LyricMinigame(BattleManagerEventContext context) => OnPlayerLyric?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPlayerNarrative;
    public void NarrativeMinigame(BattleManagerEventContext context) => OnPlayerNarrative?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPlayerArgumentative;
    public void ArgumentativeMinigame(BattleManagerEventContext context) => OnPlayerArgumentative?.Invoke(context);

    public event Action<BattleManagerEventContext> OnCamPlaced;
    public void CamPlaced(BattleManagerEventContext context) => OnCamPlaced?.Invoke(context);


    //Lyric Minigame
    public event Action<BattleManagerEventContext> OnKnobEnabled;
    public void KnobEnabled(BattleManagerEventContext context) => OnKnobEnabled?.Invoke(context);

    public event Action<BattleManagerEventContext> OnCursorTriggerEnter;
    public void CursorTriggerEnter(BattleManagerEventContext context) => OnCursorTriggerEnter?.Invoke(context);

    public event Action<BattleManagerEventContext> OnCursorTriggerExit;
    public void CursorTriggerExit(BattleManagerEventContext context) => OnCursorTriggerExit?.Invoke(context);

    public event Action<BattleManagerEventContext> OnKnobEndReached;
    public void KnobEndReached(BattleManagerEventContext context) => OnKnobEndReached?.Invoke(context);

    public event Action<BattleManagerEventContext> OnLyricSplineEnabled;
    public void LyricSplineEnabled(BattleManagerEventContext context) => OnLyricSplineEnabled?.Invoke(context);

    //Poetic Minigame
    public event Action<BattleManagerEventContext> OnPoeticCollisionOccured;
    public void PoeticCollisionOccured(BattleManagerEventContext context) => OnPoeticCollisionOccured?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPoeticEndTriggered;
    public void PoeticEndTriggered(BattleManagerEventContext context) => OnPoeticEndTriggered?.Invoke(context);

    public event Action<BattleManagerEventContext> OnPoeticOrbCaught;
    public void PoeticOrbCaught(BattleManagerEventContext context) => OnPoeticOrbCaught?.Invoke(context);

    //Arg Miniigame
    public event Action<BattleManagerEventContext> OnComboUpdate;
    public void ComboUpdate(BattleManagerEventContext context) => OnComboUpdate?.Invoke(context);

    public event Action<BattleManagerEventContext> OnComboAchieved;
    public void ComboAchieved(BattleManagerEventContext context) => OnComboAchieved?.Invoke(context);

    //Narrative Minigame
    public event Action<BattleManagerEventContext> OnOffensiveQTE;
    public void OffensiveQTE(BattleManagerEventContext context) => OnOffensiveQTE?.Invoke(context);

    public event Action<BattleManagerEventContext> OnDefensiveQTE;
    public void DefensiveQTE(BattleManagerEventContext context) => OnDefensiveQTE?.Invoke(context);
}
