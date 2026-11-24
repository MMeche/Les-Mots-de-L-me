using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;
using MemoryPack;
using Unity.VisualScripting;

public static class SaveSystem
{
    public const string FILENAME_SAVEDATA = "/savedata.json";

    public static void SaveGameState()
    {
        
        string filePathSaveData = Application.persistentDataPath + FILENAME_SAVEDATA;
        Debug.Log("Saving Game in " + filePathSaveData);

        List<PlayerStatus> allPlayerStatus = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
        List<PlayerSaveData> allPlayerSaveData = new();
        foreach (var status in allPlayerStatus) allPlayerSaveData.Add(new PlayerSaveData(status));
        InteractedSteleData journalData = Resources.FindObjectsOfTypeAll<InteractedSteleData>()[0];
        List<SteleSave> journalStelesSave = new();
        foreach (Stele stele in journalData.Steles)
        {
            journalStelesSave.Add(new(stele));
        }
        JournalSteleSaveData journalSteleSaveData = new(journalStelesSave);
        TutorialSaveData tutorialSaveData = new(Resources.FindObjectsOfTypeAll<TutorialState>()[0]);
        SaveData saveData = new SaveData(allPlayerSaveData, tutorialSaveData, journalSteleSaveData);
        string txt = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathSaveData, txt);
    }

    public static void SaveGameState(string fileName)
    {

        string filePathSaveData = Application.persistentDataPath + fileName;
        Debug.Log("Saving Game in " + filePathSaveData);

        List<PlayerStatus> allPlayerStatus = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
        List<PlayerSaveData> allPlayerSaveData = new();
        foreach (PlayerStatus status in allPlayerStatus)
        {
            allPlayerSaveData.Add(new PlayerSaveData(status));
        }
        InteractedSteleData journalData = Resources.FindObjectsOfTypeAll<InteractedSteleData>()[0];
        List<SteleSave> journalStelesSave = new();
        foreach(Stele stele in journalData.Steles)
        {
            journalStelesSave.Add(new(stele));
        }
        JournalSteleSaveData journalSteleSaveData = new(journalStelesSave);
        TutorialSaveData tutorialSaveData = new(Resources.FindObjectsOfTypeAll<TutorialState>()[0]);
        SaveData saveData = new SaveData(allPlayerSaveData,tutorialSaveData,journalSteleSaveData);
        string txt = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathSaveData, txt);
    }
}

[Serializable]
public partial class SaveData
{
    [SerializeField] public List<PlayerSaveData> _allPlayerSaveData;
    [SerializeField] public TutorialSaveData _tutorialSaveData;
    [SerializeField] public JournalSteleSaveData _journalSteleSaveData;

    public SaveData(List<PlayerSaveData> allPlayerSaveData, TutorialSaveData tutorialSaveData, JournalSteleSaveData journalSteleSaveData)
    {
        _allPlayerSaveData = allPlayerSaveData;
        _tutorialSaveData = tutorialSaveData;
        _journalSteleSaveData = journalSteleSaveData;
    }
}

[Serializable]
public partial class PlayerSaveData
{
    [SerializeField] public int PlayerIndex;
    [SerializeField] public string CharName;
    [SerializeField] public Vector3 Position;
    [SerializeField] public int Level;
    [SerializeField] public float Experience;
    [SerializeField] public float MaxHealth;
    [SerializeField] public float Health;
    [SerializeField] public int AvailableSkillPoint;
    [SerializeField] public List<string> EnemiesDefeated;
    [SerializeField] public int PoeticStats;
    [SerializeField] public int LyricStats;
    [SerializeField] public int ArgStats;
    [SerializeField] public int NarrativeStats;

    public PlayerSaveData(PlayerStatus player)
    {
        PlayerIndex = player.PlayerIndex;
        CharName = player.CharName;
        Position = player.Position;
        Level = player.Level;
        Experience = player.Experience;
        MaxHealth = player.MaxHealth;
        Health = player.Health;
        AvailableSkillPoint = player.AvailableSkillPoints;
        EnemiesDefeated = player.EnemiesDefeated;
        PoeticStats = player.Stats[StatType.Poetic];
        LyricStats = player.Stats[StatType.Lyrical];
        ArgStats = player.Stats[StatType.Argumentative];
        NarrativeStats = player.Stats[StatType.Narrative];
    }
}

[Serializable]
public partial class TutorialSaveData
{
    
    [SerializeField] public int DoorPartsOpened;
    [SerializeField] public bool DoorOpened;
    [SerializeField] public bool TutoSteleActivated;
    [SerializeField] public bool TutoMoveDone;
    [SerializeField] public bool InvisibleWall;
    [SerializeField] public bool TutoPoetic;
    [SerializeField] public bool TutoLyric;
    [SerializeField] public bool TutoNarrative;
    [SerializeField] public bool TutoArgumentative;
    [SerializeField] public bool TutoStatDone;
    [SerializeField] public bool TutoJournalDone;
    [SerializeField] public bool IntroCamDone;

    public List<string> TutorialDefeated;

    public TutorialSaveData(TutorialState t)
    {
        DoorPartsOpened = t.DoorPartsOpened;
        DoorOpened = t.DoorOpened;
        TutoSteleActivated = t.TutoSteleActivated;
        TutoMoveDone = t.TutoMoveDone;
        TutoStatDone = t.TutoStatDone;
        InvisibleWall = t.InvisibleWall;
        TutoPoetic = t.TutoPoetic;
        TutoLyric = t.TutoLyric;
        TutoNarrative = t.TutoNarrative;
        TutoArgumentative = t.TutoArgumentative;
        TutorialDefeated = t.TutorialDefeated;
        TutoJournalDone= t.TutoJournalDone;
        IntroCamDone = t.IntroCamDone;
    }
    
    
}
[Serializable]
public partial class JournalSteleSaveData
{
    [SerializeField] public List<SteleSave> Steles;

    public JournalSteleSaveData(List<SteleSave> steles)
    {
        Steles = steles;
    }
}

[Serializable]
public partial class SteleSave
{
    [SerializeField] public Texture2D VisuelStele;
    [SerializeField] public Texture2D VisuelJournal;
    [SerializeField] public FMOD.Studio.EventInstance InstanceVoiceStele;
    [SerializeField] public bool Ministele;
    [SerializeField] public int SteleIndex;

    public SteleSave(Stele s)
    {
        VisuelStele = s.VisuelStele;
        VisuelJournal = s.VisuelJournal;
        InstanceVoiceStele = s.InstanceVoiceStele;
        Ministele = s.Ministele;
        SteleIndex = s.SteleIndex;
    }
}