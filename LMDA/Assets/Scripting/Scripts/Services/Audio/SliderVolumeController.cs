using FMODUnity;
using UnityEngine;

public class SliderVolumeController : MonoBehaviour
{
    [SerializeField] string _vcaName;

    public void SetVolume(float newVolume)
    {
        AudioManager.ChangeVolume("vca:/" + _vcaName, newVolume);
    }
}
