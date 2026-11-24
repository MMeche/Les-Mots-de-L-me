using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ServiceProvider : MonoBehaviour
{
    public ServiceLocator ServiceLocator { get; private set; }
    public static ServiceProvider Instance { get; internal set; }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;

        ServiceLocator = new ServiceLocator();
    }
}