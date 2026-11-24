using UnityEngine;

public class BattleUIEventContext
{
    public BattleUIEventContext() : base() { }

    public float LargeurCulMatthieu;
    public PlayerStatus PlayerStatus;
    public EnemyStatus EnemyStatus;
    public float NewHealth;
    public string TutoPanelName;

    //For the damage info bar.
    public bool StackDamage; 
    public float IncomingPlayerDamage;
    public float IncomingEnemyDamage;

    //For the arg card thing
    public Transform Card;
}
