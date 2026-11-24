#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationManager : MonoBehaviour
{

    [SerializeField] GameObject rippleCamera;
    [SerializeField] Animator _animator;

    private TrailRenderer[] _trails;
    [SerializeField] ParticleSystem _trailParticles;
    [SerializeField] ParticleSystem _rippleParticles;
    [SerializeField] ParticleSystem _stationaryRipple;

    private void OnEnable()
    {
        
        _trails = GetComponentsInChildren<TrailRenderer>();
        _trailParticles = GetComponentInChildren<ParticleSystem>();
        
    }

    private void Update()
    {
        if (rippleCamera!=null)
        {
            rippleCamera.transform.position = transform.position + Vector3.up * 10;
            Shader.SetGlobalVector("_PlayerPos", rippleCamera.transform.position);
        }
        
        //if (_animator.GetBool("Walking") || _animator.GetBool("Sprinting"))
        //{
        //    _rippleParticles.Play(true);
        //    Debug.Log("rippleplying");
        //}
        //else _rippleParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    private void MoveStarted()
    {
        _animator.SetBool("Iddle", false);
        _animator.SetBool("Walking", true);
        ToggleRipples();
    }

    private void MoveStopped()
    {
        _animator.SetBool("Walking", false);
        _animator.SetBool("Iddle", true);
        ToggleRipples();
    }

    private void BeginSprint()
    {
        _animator.SetBool("Sprinting", true);
        ToggleTrails();
    }

    private void EndSprint()
    {
        _animator.SetBool("Sprinting", false);
        ToggleTrails();
    }

    private void BeginJump()
    {
        _animator.SetBool("JumpEnd", false);
        _animator.SetBool("JumpStart", true);
    }
    
    private void EndJump()
    {
        _animator.SetBool("JumpStart", false);
        _animator.SetBool("Falling", false);
        _animator.SetBool("JumpEnd", true);
    }

    private void Falling()
    {
        _animator.SetBool("JumpEnd", false);
        _animator.SetBool("Falling", true);
    }

    public void Dead()
    {
        _animator.SetBool("Dead", true);
    }
    
    public void Attack()
    {
        _animator.Play("PlayerAttackWithEvent");
    }

    private void ToggleTrails()
    {
        foreach(TrailRenderer trailRend in _trails)
        {
            trailRend.emitting = !trailRend.emitting;
        }
        if (_trailParticles.isEmitting)
        {
            _trailParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            _trailParticles.Play(true);
        }
        
    }

    private void ToggleRipples()
    {
        if(_rippleParticles != null && _stationaryRipple != null)
        {
            if (_rippleParticles.isEmitting)
            {
                _rippleParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _stationaryRipple.Play(true);
            }
            else
            {
                _stationaryRipple.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _rippleParticles.Play(true);
            }
        }
        
    }
}
