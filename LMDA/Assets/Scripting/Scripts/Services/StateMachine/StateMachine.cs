using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;

	private void Start()
    {
        _currentState = GetInitialState();

        _currentState?.Enter();
    }

    private void Update()
    {
        _currentState?.UpdateLogic();
    }

    public void ChangeState(BaseState newState)
    {
        if(_currentState!=null)_currentState.Exit();

        _currentState = newState;

        _currentState.Enter();
	}

    public BaseState GetCurrentState()
    {
        return _currentState;
    }

	protected virtual BaseState GetInitialState()
    {
        return null;
    }
}
