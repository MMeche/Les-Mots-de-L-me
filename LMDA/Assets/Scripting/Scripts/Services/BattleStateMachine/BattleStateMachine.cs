using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    private BattleBaseState _currentState;

	private void Start()
    {
        _currentState = GetInitialState();

        _currentState?.Enter();
    }

    private void Update()
    {
        _currentState?.UpdateLogic();
    }

    public void ChangeState(BattleBaseState newState)
    {
        _currentState.Exit();

        _currentState = newState;

        _currentState.Enter();
	}

    public BattleBaseState GetCurrentState()
    {
        return _currentState;
    }

	protected virtual BattleBaseState GetInitialState()
    {
        return null;
    }
}
