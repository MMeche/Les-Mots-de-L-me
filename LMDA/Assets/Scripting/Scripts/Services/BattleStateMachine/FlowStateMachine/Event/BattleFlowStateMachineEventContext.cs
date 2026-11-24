using System.Collections.Generic;

public class BattleFlowStateMachineEventContext
{
    public BattleFlowStateMachineEventContext() : base() { }
    public string NewActionMapName;
    public string[] UINames;
    public EnemyStatus EnemyStatus;
    public PlayerStatus PlayerStatus;
    public int SelectionStatusIndex;
    public float ExpToAdd;
}
