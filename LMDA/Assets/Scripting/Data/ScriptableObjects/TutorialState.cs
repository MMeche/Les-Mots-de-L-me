using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TutorialState", menuName = "Scriptable Objects/TutorialState")]
public class TutorialState : ScriptableObject
{
    public int DoorPartsOpened = 0;
    public bool DoorOpened = false;
    public bool TutoSteleActivated = false;
    public bool TutoMoveDone = false;
    public bool InvisibleWall = false;
    public bool TutoPoetic = false;
    public bool TutoLyric = false;
    public bool TutoNarrative = false;
    public bool TutoArgumentative = false;
    public bool TutoStatDone = false;
    public bool TutoJournalDone = false;
    public bool IntroCamDone = false;

    public List<string> TutorialDefeated = new();

    public void Reset()
    {
        TutoMoveDone = false;
        DoorPartsOpened = 0;
        DoorOpened = false;
        TutoSteleActivated = false;
        InvisibleWall = false;
        TutoStatDone = false;
        TutorialDefeated = new();
        TutoLyric = false;
        TutoNarrative = false;
        TutoArgumentative = false;
        TutoPoetic = false;
        TutoJournalDone = false;
        IntroCamDone = false;
    }

    public TutorialState(TutorialSaveData tutoSave)
    {
        TutoMoveDone = tutoSave.TutoMoveDone;
        DoorPartsOpened = tutoSave.DoorPartsOpened;
        DoorOpened = tutoSave.DoorOpened;
        TutoSteleActivated = tutoSave.TutoSteleActivated;
        InvisibleWall = tutoSave.InvisibleWall;
        TutoStatDone = tutoSave.TutoStatDone;
        TutorialDefeated = tutoSave.TutorialDefeated;
        TutoLyric = tutoSave.TutoLyric;
        TutoPoetic = tutoSave.TutoPoetic;
        TutoNarrative = tutoSave.TutoNarrative;
        TutoArgumentative = tutoSave.TutoArgumentative;
        TutoJournalDone = tutoSave.TutoJournalDone;
        IntroCamDone = tutoSave.IntroCamDone;
    }

    public void Assign(TutorialSaveData tutoSave)
    {
        TutoMoveDone = tutoSave.TutoMoveDone;
        DoorPartsOpened = tutoSave.DoorPartsOpened;
        DoorOpened = tutoSave.DoorOpened;
        TutoSteleActivated = tutoSave.TutoSteleActivated;
        InvisibleWall = tutoSave.InvisibleWall;
        TutoStatDone = tutoSave.TutoStatDone;
        TutorialDefeated = tutoSave.TutorialDefeated;
        TutoLyric = tutoSave.TutoLyric;
        TutoPoetic = tutoSave.TutoPoetic;
        TutoNarrative = tutoSave.TutoNarrative;
        TutoArgumentative = tutoSave.TutoArgumentative;
        TutoJournalDone = tutoSave.TutoJournalDone;
        IntroCamDone = tutoSave.IntroCamDone;
    }
    
}
