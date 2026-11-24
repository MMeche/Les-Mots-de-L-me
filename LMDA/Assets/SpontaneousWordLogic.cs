using UnityEngine;
using Unity.Cinemachine;

public class SpontaneousWordLogic : MonoBehaviour
{
    public void TerminateWordEvent()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.rotation = FindFirstObjectByType<CinemachineBrain>().OutputCamera.transform.rotation;
    }
}
