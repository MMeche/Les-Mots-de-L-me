using UnityEngine;

public class TrailParticleAnimationController : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Falling() 
    {
        _animator.SetBool("Falling",true);
    }

    private void EndJump()
    {
        _animator.SetBool("Falling",false);
    }
}
