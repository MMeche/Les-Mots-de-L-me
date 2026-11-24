using Unity.VisualScripting;
using UnityEngine;

public class SpriteCameraOrient : MonoBehaviour
{

    Camera _mainCamera;
    private void OnEnable()
    {
        _mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position.Set(transform.position.x, _mainCamera.transform.position.y - transform.position.y, transform.position.z);
    }
}
