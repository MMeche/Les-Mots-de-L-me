public class BattleStates
{
    public BattleFlowState_EnterCombat BattleFlowStateEnterCombat;
    public BattleFlowState_PlayerTurn BattleFlowStatePlayerTurn;
    public BattleFlowState_EnemyTurn BattleFlowStateEnemyTurn;
    public BattleFlowState_EndCombat BattleFlowStateEndCombat;
    public BattleFlowState_Poetic BattleFlowStatePoetic;
    public BattleFlowState_PoeticMinigameFail BattleFlowStatePoeticMinigameFail;
    public BattleFlowState_Narrative BattleFlowState_Narrative;
    public BattleFlowState_Lyric BattleFlowStateLyric;
    public BattleFlowState_Argumentative BattleFlowStateArgumentative;
    public BattleFlowState_Tutorial BattleFlowStateTutorial;

    public BattleStates(BattleFlowStateMachine stateMachine)
    {
        BattleFlowStateEnterCombat = new BattleFlowState_EnterCombat(stateMachine);
        BattleFlowStatePlayerTurn = new BattleFlowState_PlayerTurn(stateMachine);
        BattleFlowStateEnemyTurn = new BattleFlowState_EnemyTurn(stateMachine);
        BattleFlowStateEndCombat = new BattleFlowState_EndCombat(stateMachine);
        BattleFlowStatePoetic = new BattleFlowState_Poetic(stateMachine);
        BattleFlowStatePoeticMinigameFail = new BattleFlowState_PoeticMinigameFail(stateMachine);
        BattleFlowState_Narrative = new BattleFlowState_Narrative(stateMachine);
        BattleFlowStateLyric = new BattleFlowState_Lyric(stateMachine);
        BattleFlowStateArgumentative = new BattleFlowState_Argumentative(stateMachine);
        BattleFlowStateTutorial = new BattleFlowState_Tutorial(stateMachine);
    }
}