using UnityEngine;
using UnityEngine.Rendering;

public class LyricSplineLogic : MonoBehaviour
{
    BattleManager _battleManager;



private void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleManager.BattleManagerEventBus.LyricSplineEnabled(new() { });
        //SendMessageUpwards("OnLyricSplineEnabled");
    }
}
