using UnityEngine;
using UnityEngine.VFX;

public class PlayerFeedbackBurstController : MonoBehaviour
{

    VisualEffect _vfx;
    
    void Start()
    {
        _vfx = GetComponentInChildren<VisualEffect>();
    }

    
    void Update()
    {
        if (_vfx.aliveParticleCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
