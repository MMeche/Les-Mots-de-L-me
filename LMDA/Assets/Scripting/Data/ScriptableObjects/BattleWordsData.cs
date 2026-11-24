using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BattleWordsData", menuName = "Scriptable Objects/BattleWords")]
public class BattleWordsData : ScriptableObject
{
    [SerializeField] List<WordsData> WordsLists;

    public List<string> GetAlistWithLength(int nbOfWords)
    {
        List<List<string>> selectiveLists = new();
        foreach(var w in WordsLists.FindAll(l => l.Words.Count == nbOfWords))
        {
            selectiveLists.Add(w.Words);
        }
        return selectiveLists[Random.Range(0, selectiveLists.Count)];
    }
}