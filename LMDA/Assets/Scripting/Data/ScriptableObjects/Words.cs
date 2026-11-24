using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Words", menuName = "Scriptable Objects/Words")]
public class WordsData : ScriptableObject
{
    public List<string> Words;
}
