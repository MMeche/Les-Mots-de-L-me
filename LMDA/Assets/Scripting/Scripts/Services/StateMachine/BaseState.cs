using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseState
{
    public string Name;
    protected StateMachine StateMachine;

    public BaseState(string name, StateMachine stateMachine) 
    {
        this.Name = name;
        this.StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void Exit() { }
}
