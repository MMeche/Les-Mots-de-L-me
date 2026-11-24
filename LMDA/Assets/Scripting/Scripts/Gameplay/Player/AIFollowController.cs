using UnityEngine;
using UnityEngine.AI;

public class AIFollowController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Transform _navTarget;
    [SerializeField] PlayerStatus _followerStatus;
    [SerializeField] ParticleSystem _rippleParticles;
    [SerializeField] ParticleSystem _stationaryRipple;

    TrailRenderer[] _trails;
    ParticleSystem _trailParticle;
    
    NavMeshAgent _navAgent;
    bool _tooFar = false;

    float _updateRate = 0.1f;
    float _nextUpdateTime = 0f;
    float _targetSpeed=5f;
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _trails = GetComponentsInChildren<TrailRenderer>();
        _trailParticle = GetComponentInChildren<ParticleSystem>();
        _navAgent.Warp(_navTarget.position);
        _animator.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        


        float dist = Vector3.Distance(transform.position, _navTarget.position);
        if (!_tooFar && Vector3.Distance(transform.position, _navTarget.position) > 5)
        {
            _tooFar = true;
            _targetSpeed = 14f;
            _animator.SetBool("Sprinting", true);
            foreach(var trail in _trails)
            {
                trail.emitting = true;
            }
            _trailParticle.Play();
            
        }

        if(_tooFar && Vector3.Distance(transform.position, _navTarget.position) < 5)
        {
            _tooFar = false;
            _targetSpeed = 7f;
            _animator.SetBool("Sprinting", false);
            foreach (var trail in _trails)
            {
                trail.emitting = false;
            }
            _trailParticle.Stop(true,ParticleSystemStopBehavior.StopEmitting);

        }
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, _targetSpeed, Time.deltaTime * 5f);

    }

    private void FixedUpdate()
    {
        if (Time.time >= _nextUpdateTime)
        {
            _navAgent.SetDestination(_navTarget.position);
            _nextUpdateTime = Time.time + _updateRate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("Walking", false);
            _animator.SetBool("Sprinting", false);
            ToggleRipple();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("Walking", true);
            ToggleRipple();
        }
    }

    private void ToggleRipple()
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
