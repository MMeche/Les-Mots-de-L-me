using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "StatusObjects/PlayerData", order = 1)]
public class PlayerStatus : ScriptableObject
{
    public Vector3 InitialSpawn = new Vector3(-29.2700005f, 17.3099995f, 141.480743f);

    public int PlayerIndex;
    public string CharName = "name";
    public Vector3 Position;
    public GameObject CharacterGameObject;
    public int Level = 1;
    public float Experience;
    public float MaxHealth = 100;
    public float Health = 100;
    public int AvailableSkillPoints;
    public List<ArgComboData> ComboList;
    public List<string> EnemiesDefeated;

    public Dictionary<StatType, int> Stats = new() 
    { 
        [StatType.Narrative] = 0, 
        [StatType.Poetic] = 0 , 
        [StatType.Lyrical] = 0 , 
        [StatType.Argumentative] = 0 
    };

    public void Reset()
    {
        Level = 1;
        Experience = 0;
        MaxHealth = 100;
        Health = 100;
        AvailableSkillPoints = 0;
        EnemiesDefeated = new();
        Position = InitialSpawn;
        Stats = new()
        {
            [StatType.Narrative] = 0,
            [StatType.Poetic] = 0,
            [StatType.Lyrical] = 0,
            [StatType.Argumentative] = 0
        };
    }
}

public enum StatType
{
    Narrative,
    Poetic,
    Lyrical,
    Argumentative
}
