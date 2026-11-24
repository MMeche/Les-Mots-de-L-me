using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InteractedSteleData", menuName = "Scriptable Objects/InteractedSteleData")]
public class InteractedSteleData : ScriptableObject
{
    public List<Stele> Steles = new();

    public void Reset()
    {
        Steles = new();
    }

    public void Assign(JournalSteleSaveData saveData)
    {
        Steles = new();
        foreach(SteleSave stele in saveData.Steles)
        {
            Stele newS = new();
            newS.VisuelStele = stele.VisuelStele;
            newS.VisuelJournal = stele.VisuelJournal;
            newS.Ministele = stele.Ministele;
            newS.InstanceVoiceStele = stele.InstanceVoiceStele;
            newS.SteleIndex = stele.SteleIndex;
            Steles.Add(newS);
        }
    }
}

public class Stele
{
    public Texture2D VisuelStele;
    public Texture2D VisuelJournal;
    public FMOD.Studio.EventInstance InstanceVoiceStele;
    public bool Ministele;
    public int SteleIndex;
}
