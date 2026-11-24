using System.Collections;
using UnityEngine;

public class InteractionDetectionManager : MonoBehaviour
{
    SphereCollider _collider;
    GameObject _currentInteractionTarget = null;
    Coroutine _coroutine;

    [SerializeField] GameObject _popUp;
    [SerializeField] Transform _currentCam;
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (_currentInteractionTarget == null)
            {
                _currentInteractionTarget = other.gameObject;
                _popUp.SetActive(true);
                _coroutine = StartCoroutine(nameof(PopUpFaceCamera));
            }

            else if (Vector3.Distance(_currentInteractionTarget.transform.position, transform.position) > Vector3.Distance(other.transform.position, transform.position))
            {
                _currentInteractionTarget.GetComponent<IInteractable>().HideOutline();
                _currentInteractionTarget = other.gameObject;
            }
            _currentInteractionTarget.GetComponent<IInteractable>().ShowOutline();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            other.GetComponent<IInteractable>().HideOutline();
            if (other.gameObject == _currentInteractionTarget)
            {
                _currentInteractionTarget = null;
                StopCoroutine(_coroutine);
                _popUp.SetActive(false);
            }
        }
    }

    IEnumerator PopUpFaceCamera()
    {
        while (true)
        {
            transform.rotation = _currentCam.rotation;
            yield return null;
        }
    }

    public IInteractable GetInteractionTarget()
    {   if (_currentInteractionTarget) return _currentInteractionTarget.GetComponent<IInteractable>();
        else return null;
    }
}
