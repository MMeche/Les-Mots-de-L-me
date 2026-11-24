using System;
using System.Diagnostics;
using UnityEngine;

public class FlowProgressTracker
{
    public GamePhase CurrentGamePhase;
    private int _phaseIndex;
	private int _previousPhaseIndex = 1;

	public FlowProgressTracker() 
    {
        CurrentGamePhase = GamePhase.None;
        _phaseIndex = (int)GamePhase.None;
    }

    public void ChangeGamePhase(int index)
    {
        if (Enum.IsDefined(typeof(GamePhase), index))
        {
            CurrentGamePhase = (GamePhase)index;
            _phaseIndex = index;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Invalid {nameof(GamePhase)} enum value");
        }
    }

    public void ChangeGamePhase(GamePhase newPhase)
    {
		if (Enum.IsDefined(typeof(GamePhase), newPhase))
        {
            CurrentGamePhase = newPhase;
            _phaseIndex = (int)newPhase;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(newPhase), $"Invalid {nameof(GamePhase)} enum value");
        }
    }

    public void IncrementPhaseIndex()
    {
        int newIndex = _phaseIndex + 1;
        if (Enum.IsDefined(typeof(GamePhase), newIndex))
        {
            _phaseIndex = newIndex;
            CurrentGamePhase = (GamePhase)_phaseIndex;
        }
        else
        {
            // Handle the case where the new index is out of bounds
            throw new InvalidOperationException("PhaseIndex is out of bounds");
        }
    }

    public void SetFormerPhaseIndex(int index)
    { 
        _previousPhaseIndex = index;
    }

	public int GetFormerPhaseIndex()
	{
		return _previousPhaseIndex;
	}

	public int GetEnumLastValue()
    {
        return Enum.GetValues(typeof(GamePhase)).Length;
    }

    public int GetCurrentPhaseIndex()
    {
        return _phaseIndex;
    }

	private void Debug_PhaseIndex()
    {
        UnityEngine.Debug.Log($"DEBUG: {nameof(_phaseIndex)} : {_phaseIndex}, {CurrentGamePhase}");
    }

    public enum GamePhase
    {
        None = -1,
        Overworld = 0,
    }
}
