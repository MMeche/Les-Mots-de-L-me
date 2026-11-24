using UnityEngine;

public class OrbAnimation : MonoBehaviour
{
    private void AnimationEnd()
    {
        Vector3 newAngles = new Vector3(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
        transform.rotation = Quaternion.Euler(newAngles);
    }
}
