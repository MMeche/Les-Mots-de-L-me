using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{

    
    public static AudioManager Instance { get; private set; }

    List<EventInstance> _currentInstances;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Several Audio managers found");
        }
        Instance = this;
        ServiceProvider.Instance.ServiceLocator.Register(this);
        _currentInstances = new();
    }

    public void PlayOneShotAttached(EventReference sound, GameObject gm)
    {
        RuntimeManager.PlayOneShotAttached(sound, gm);
    }


    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance AttachToGameObject(EventReference sound, GameObject gm)
    {
        EventInstance newInstance = RuntimeManager.CreateInstance(sound);
        _currentInstances.Add(newInstance);
        RuntimeManager.AttachInstanceToGameObject(newInstance, gm);
        return newInstance;
    }

    public void RemoveFromGameObject(EventInstance obsoleteInstance,FMOD.Studio.STOP_MODE stopMode)
    {
        obsoleteInstance.stop(stopMode);              // 1. Stop playback
        obsoleteInstance.release();                   // 2. Tell FMOD we’re done
        obsoleteInstance.clearHandle();               // 3. Clear local handle

        _currentInstances.RemoveAll(ps => ps.handle == obsoleteInstance.handle);
    }

    public EventInstance SwapInstances(EventInstance obsoleteInstance,EventReference newSound,GameObject gm)
    {
        
        obsoleteInstance.getTimelinePosition(out int position);
        obsoleteInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);     // 1. Stop playback
        obsoleteInstance.release();                                 // 2. Tell FMOD we’re done
        obsoleteInstance.clearHandle();                             // 3. Clear local handle

        _currentInstances.RemoveAll(ps => ps.handle == obsoleteInstance.handle);
        EventInstance newInstance = RuntimeManager.CreateInstance(newSound);
        newInstance.setTimelinePosition(position);
        _currentInstances.Add(newInstance);
        RuntimeManager.AttachInstanceToGameObject(newInstance, gm);
        return newInstance;
    }


    public static void ChangeVolume(string vcaName, float newVolume)
    {
        RuntimeManager.GetVCA(vcaName).setVolume(newVolume);
    }
   
}
