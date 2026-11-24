using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleBaseState
{
    public string Name;
    protected BattleStateMachine StateMachine;

    public BattleBaseState(string name, BattleStateMachine stateMachine) 
    {
        this.Name = name;
        this.StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void Exit() { }
}
