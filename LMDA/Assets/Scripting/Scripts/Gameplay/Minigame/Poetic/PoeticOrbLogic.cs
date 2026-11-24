using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PoeticOrbLogic : MonoBehaviour
{
    BattleManager _battleManager;
    Collider _collider;
    [SerializeField] GameObject _feedbackParticles;
    [SerializeField] GameObject _feedBackBurst;
    void Start()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.TryGet<BattleManager>();
        _collider = GetComponent<SphereCollider>();
    }


    void OnTriggerEnter(Collider other)
    {
        _battleManager.BattleManagerEventBus.PoeticOrbCaught(new() { });
        Instantiate(_feedbackParticles,transform.position,Quaternion.identity);
        Instantiate(_feedBackBurst, transform.position, Camera.main.transform.rotation);
        //SendMessageUpwards("PoeticOrbCaught");
        Collected();
    }

    void Collected()
    {
        Destroy(this.gameObject);
    }
}
