using System;

public class BattleFlowStateMachineEventBus
{
    public event Action<BattleFlowStateMachineEventContext> UIActivated;
    public void OnUIActivated(BattleFlowStateMachineEventContext context) => UIActivated?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> StartBattle;
    public void OnBattleStart(BattleFlowStateMachineEventContext context) => StartBattle?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> SelectionChanged;
    public void OnSelectionChange(BattleFlowStateMachineEventContext context) => SelectionChanged?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> SelectionDone;
    public void OnSelectionDone(BattleFlowStateMachineEventContext context) => SelectionDone?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> SelectionCancel;
    public void OnSelectionCancel(BattleFlowStateMachineEventContext context) => SelectionCancel?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> UISetup;
    public void OnUISetup(BattleFlowStateMachineEventContext context) => UISetup?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnPlayersTurn;
    public void PlayersTurn(BattleFlowStateMachineEventContext context) => OnPlayersTurn?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnPlayersExit;
    public void ExitPlayersTurn(BattleFlowStateMachineEventContext context) => OnPlayersExit?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnEnemysTurn;
    public void EnemysTurn(BattleFlowStateMachineEventContext context) => OnEnemysTurn?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnPoeticMinigame;
    public void PoeticMinigameStart(BattleFlowStateMachineEventContext context) => OnPoeticMinigame?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnLyricMinigame;
    public void LyricMinigameStart(BattleFlowStateMachineEventContext context) => OnLyricMinigame?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnNarrativeMiniGame;
    public void NarrativeMiniGameStart(BattleFlowStateMachineEventContext context) => OnNarrativeMiniGame?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnArgumentativeMinigame;
    public void ArgumentativeMinigameStart(BattleFlowStateMachineEventContext context) => OnArgumentativeMinigame?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> PoeticMinigameFail;
    public void OnPoeticMinigameFail(BattleFlowStateMachineEventContext context) => PoeticMinigameFail?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnInputMapChange;
    public void ChangeInputMap(BattleFlowStateMachineEventContext context) => OnInputMapChange?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnBattleLose;
    public void BattleLost(BattleFlowStateMachineEventContext context) => OnBattleLose?.Invoke(context);
    
    public event Action<BattleFlowStateMachineEventContext> OnPlayerDead;
    public void PlayerDied(BattleFlowStateMachineEventContext context) => OnPlayerDead?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnBattleWin;
    public void BattleWin(BattleFlowStateMachineEventContext context) => OnBattleWin?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnPlayerAttackStrike;
    public void PlayerAttackStrike(BattleFlowStateMachineEventContext context) => OnPlayerAttackStrike?.Invoke(context);

    public event Action<BattleFlowStateMachineEventContext> OnEnemyAttackStrike;
    public void EnemyAttackStrike(BattleFlowStateMachineEventContext context) => OnEnemyAttackStrike?.Invoke(context);
}
