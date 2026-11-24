using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.UI.Image;

public class EnemyOverworldAI : MonoBehaviour
{
    [SerializeField] Transform _npcEyes;
    [SerializeField] Transform _playerHead;

    NavMeshAgent _agent;

    [SerializeField] bool _isBusy;
    [SerializeField] float _walkingSpeed = 1.5f;
    [SerializeField] float _runningSpeed = 2.5f;

    float _lastDetectionTime = 0f;

    [SerializeField] float _destinationSpread;
    [SerializeField] float _destinationRange;
    [SerializeField] float _playerDetectionRange = 15f;

    // Start is called before the first frame update
    void OnEnable()
    {
        _agent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Start()
    {
        if(_agent!=null) InvokeRepeating(("Detect"), 0.2f, 0.5f);
    }

    void Update()
    {
        // Si chu pô oqp, jfa ma run
        if (!_isBusy && _agent != null)
        {
            StartCoroutine(Work(SeekDestination(), _walkingSpeed));
        }
        if(_agent == null)
        {
            CancelInvoke("Detect");
        }
    }

    void Detect()
    {
        RaycastHit hit;
        Vector3 dir = (_playerHead.position - _npcEyes.position).normalized;
        // Cast une ligne entre les point, retourne des infos sur l'objet touché
        if (Physics.Raycast(_npcEyes.position, dir, out hit, _playerDetectionRange))
        {
            Debug.DrawLine(_npcEyes.position, _playerHead.position, Color.black);
            if (hit.transform.CompareTag("Player"))
            {
                // Arreter la patrouille
                StopAllCoroutines();

                // Se dirige vers la position du joueur en courant
                StartCoroutine(Work(hit.point, _runningSpeed));
            }
        }
    }

    Vector3 SeekDestination()
    {
        // Pick a random offset within the spread range
        //float offsetX = Random.Range(-_destinationSpread, _destinationSpread);
        //float offsetZ = Random.Range(-_destinationSpread, _destinationSpread);

        // Target point below the object with that offset
        Vector3 targetPoint = transform.position + Random.insideUnitSphere * _destinationSpread;

        // Direction from origin to target
        //Vector3 dir = (targetPoint - transform.position).normalized;

        NavMeshHit hit;

        // Raycast
        while (!NavMesh.SamplePosition(targetPoint, out  hit, 2f, NavMesh.AllAreas))
        {
            //offsetX = Random.Range(-_destinationSpread, _destinationSpread);
            //offsetZ = Random.Range(-_destinationSpread, _destinationSpread);

            // Target point below the object with that offset
            targetPoint = transform.position + Random.insideUnitSphere * _destinationSpread;

            // Direction from origin to target
            //dir = (targetPoint - transform.position).normalized;
            //Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        
        return hit.position;
    }

    IEnumerator Work(Vector3 _destination, float _speed)
    {
        _isBusy = true;

        _agent.speed = _speed;

        // Se déplacer vers la destination
        _agent.SetDestination(_destination);

        // Tant que je ne suis pas rendu
        while (_agent!= null && (_agent.pathPending || _agent.remainingDistance > 0.5f))
            yield return null;

        // Rendu à destination je prends un break
        yield return new WaitForSeconds(Random.Range(3f, 8f));

        // j'suis pu oqp
        _isBusy = false;
    }

    IEnumerator Detects()
    {
        yield return null;
    }
}
