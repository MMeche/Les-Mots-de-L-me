using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceProvider.Instance.ServiceLocator.Unregister<SceneLoader>();
    }

    public void LoadLevel(string levelName)
    {
       
        SceneManager.LoadScene(levelName);
    }
}
