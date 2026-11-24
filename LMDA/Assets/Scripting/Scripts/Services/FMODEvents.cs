using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Stele Voices")]
    [field: SerializeField] public List<EventReference> SteleVoice { get; private set; }
    [field: SerializeField] public List<EventReference> LevelTheme { get; private set; }
    [field: SerializeField] public EventReference WalkSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerDamageSFX { get; private set; }
    [field: SerializeField] public EventReference EnemyDamageSFX { get; private set; }
    [field: SerializeField] public EventReference UIHover { get; private set; }
    [field: SerializeField] public EventReference UIClick { get; private set; }
    [field: SerializeField] public EventReference UICancel { get; private set; }
    [field: SerializeField] public EventReference UIError { get; private set; }
    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Several FMODEvents found");
        }
        Instance = this;
        ServiceProvider.Instance.ServiceLocator.Register(this);
    }

}
