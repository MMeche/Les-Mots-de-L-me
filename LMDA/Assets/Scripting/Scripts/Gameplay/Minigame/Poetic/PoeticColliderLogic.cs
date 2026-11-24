using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PoeticColliderLogic : MonoBehaviour
{
    BattleManager _battleManager;
    Collider _collider;
    void Start()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _collider = GetComponent<Collider>();
    }


    void OnTriggerEnter(Collider other)
    {
        _battleManager.BattleManagerEventBus.PoeticCollisionOccured(new() { });
        //SendMessageUpwards("PoeticCollisionOccured");
    }
}
