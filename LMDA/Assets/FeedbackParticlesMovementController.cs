using UnityEngine;

public class FeedbackParticlesMovementController : MonoBehaviour
{
    [SerializeField] string _uiComponentName;
    RectTransform _uiTarget;

    public Camera cam;
    public float speed = 5f;
    Vector3 _targetWorldPos;
    
    
    private void Start()
    {
        _uiTarget = GameObject.Find(_uiComponentName).GetComponent<RectTransform>();
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _uiTarget.position);
        
        _targetWorldPos =  Camera.main.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane + 10f)
        );

    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetWorldPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetWorldPos) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
