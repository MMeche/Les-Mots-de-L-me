using UnityEngine;

public abstract class IServiceClient : MonoBehaviour
{
    /// <summary>
    /// Force the use of OnDisable on a manager class to unregister from the ServiceLocator
    /// </summary>
    public abstract void OnDisable();

    /// <summary>
    /// Force the use of OnEnable on a manager class to register to the ServiceLocator
    /// </summary>
    public abstract void OnEnable();

}
