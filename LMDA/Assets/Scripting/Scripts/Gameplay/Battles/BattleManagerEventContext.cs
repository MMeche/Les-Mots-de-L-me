using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;

public class BattleManagerEventContext
{
    public float DamageDone;

    public KnobType Type;
    public float ElapsedTimeHoveringKnob;

    public List<ArgCardType> LoadedCombo;

    public float NarrativeDamageModifier = -1;
    public bool NarrativeOffensive;

    public string MinigameType;
}
