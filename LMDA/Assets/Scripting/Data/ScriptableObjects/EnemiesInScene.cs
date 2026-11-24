using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesInScene", menuName = "EnemiesInScene", order = 1)]
public class EnemiesInScene : ScriptableObject
{
    public List<EnemyStatus> Enemies = new();

    
}
