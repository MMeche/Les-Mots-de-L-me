using UnityEngine;

public class DamageDisplayOnHealthBar : MonoBehaviour
{
    public void AnimationEnded()
    {
        Destroy(gameObject);
    }
    
}
