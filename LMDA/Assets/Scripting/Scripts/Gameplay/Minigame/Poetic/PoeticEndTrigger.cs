using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PoeticEndTrigger : MonoBehaviour
{
    BattleManager _battleManager;
    Collider _collider;
    void Start()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _collider = GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _battleManager.BattleManagerEventBus.PoeticEndTriggered(new() { });
            //SendMessageUpwards("PoeticEndTriggered");
        }
    }
    
}
