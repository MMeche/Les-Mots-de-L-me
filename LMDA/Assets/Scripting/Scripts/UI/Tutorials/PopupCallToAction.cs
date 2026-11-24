using UnityEngine;

public class PopupCallToAction : MonoBehaviour
{
    [SerializeField] GameObject _popUp;
    [SerializeField] Transform _currentCam;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))_popUp.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _popUp.SetActive(false);
    }

    private void FixedUpdate()
    {
        transform.rotation = _currentCam.rotation;

    }
}
