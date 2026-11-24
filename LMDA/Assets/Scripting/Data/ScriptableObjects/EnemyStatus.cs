using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatusData", menuName = "StatusObjects/EnemyData", order = 2)]
[System.Serializable]
public class EnemyStatus : ScriptableObject
{
    public string CharName = "name";
    public GameObject CharacterGameObject;
    public float MaxHealth = 100f;
    public bool Defeated = false;
    public float ExpGiven = 20f;
    [Header("Poetic parameters")]
    public bool PoeticEnabled = true;
    public float PoeticObstacleSpeed = 0.05f;
    public int PoeticObstacleNumber = 10;
    public float PoeticObstacleChance = 0.3f;
    public int PoeticGapBetweenObstacles = 3;
    public int PoeticEnemyDamage = 10;
    [Header("Lyric parameters")]
    public bool LyricEnabled = true;
    public int LyricNbOfPoints = 20;
    public int LyricMaxLengthOfAlternativePaths = 0;
    public int LyricNbOfAlternativePaths = 0;
    public float LyricAnimationSpeed = 6;
    public float LyricEnemyDamage = 1f;
    [Header("Arg parameters")]
    public bool ArgEnabled = true;
    public int ArgNbOfDefensiveCard = 2;
    public int ArgNbOfOffensiveCard = 2;
    public int ArgNbOfNeutralCard = 5;
    public int ArgNbOfSelectionSlot = 3;
    public float ArgEnemyDamage = 10;
    public EnemyWordList WordList;
    public List<ArgComboData> AvailableEnemyCombos;
    [Header("Narrative parameters")]
    public bool NarrativeEnabled = true;
    public int NarrativeNumberOfDefensiveQTE = 1;
    public int NarrativeNumberOfOffensiveQTE = 3;
    public int NarrativeEnemyDamage = 15;
    public float MinSpawnTime = 1.5f;
    public float MaxSpawnTime = 0.85f;
    public float IndicatorShrinkSpeedModifier = 1f;
    public float MinWidthSpawnDistance = 0.4f;
    public float MaxWidthSpawnDistance = 0.65f;
    public float MinHeightSpawnDistance = 0.4f;
    public float MaxHeightSpawnDistance = 0.68f;

    
}
