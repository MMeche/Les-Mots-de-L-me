using UnityEngine;

public class SingleTutoToggle : MonoBehaviour
{
    [SerializeField] GameObject _tutoToEnable;
    SphereCollider _collider;

    private void OnEnable()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _tutoToEnable.SetActive(true);
        }
    }
}
