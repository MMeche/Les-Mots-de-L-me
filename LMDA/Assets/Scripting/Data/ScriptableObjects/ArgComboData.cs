using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[CreateAssetMenu(fileName = "ArgComboData", menuName = "Scriptable Objects/ArgComboData")]
public class ArgComboData : ScriptableObject
{
    public List<ArgCardType> ComboProfile;
    public float DamageModifier;
    public float EnemiDamageModifier;

    public float EnemyComboDamage=10;
}
