using FMOD.Studio;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class PlayerAudio : MonoBehaviour
{
    private EventInstance _walkSFXInstance;
    AudioManager _audioManager;
    FMODEvents _sfxEvents;
    bool _walking;

    private void Start()
    {
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();
        _walkSFXInstance = _audioManager.AttachToGameObject(_sfxEvents.WalkSFX, gameObject);
    }
    void MoveStarted()
    {
        
        if (_walkSFXInstance.isValid()&&!_walking)
        {
            _walking = true;
            _walkSFXInstance.start();
        }
        
    }

    void MoveStopped()
    {
        if (_walkSFXInstance.isValid()&&_walking)
        {
            _walking = false;
            _walkSFXInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }


    }

    private void OnDestroy()
    {
        _audioManager.RemoveFromGameObject(_walkSFXInstance, STOP_MODE.IMMEDIATE);
    }
}
