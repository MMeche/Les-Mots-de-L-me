using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyWordList", menuName = "Scriptable Objects/EnemyWordList")]
public class EnemyWordList : ScriptableObject
{
    public List<string> WeaknessWords;
    public List<string> NeutralWords;
    public List<string> CounterWords;

    public List<string> DescriptionSentences;
}
