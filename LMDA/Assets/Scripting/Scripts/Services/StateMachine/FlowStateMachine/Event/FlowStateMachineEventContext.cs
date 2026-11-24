using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class FlowStateMachineEventContext
{
    public FlowStateMachineEventContext() : base() { }

    public string[] UIElements;
    public bool ReturnToRoot;
    public Texture2D VisuelStele;
    public Texture2D VisuelJournal;
    public FMOD.Studio.EventInstance InstanceVoiceStele;
    public bool Ministele;
    public int SteleIndex;
    public bool Replay;
    public GameObject TutorialEnemy;

    public string NewActionMapName;

    public PlayerInputs PlayerInputs;

}
