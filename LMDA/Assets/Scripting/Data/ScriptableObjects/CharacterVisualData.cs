using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterUIData", menuName = "Scriptable Objects/CharacterVisualData")]
public class CharacterUIData : ScriptableObject
{
    public string CharacterName;
    public Sprite CharacterPicture;
}
