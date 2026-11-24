#if UNITY_EDITOR
using Mono.Cecil;
#endif
using UnityEngine;

public class EnemyDataHolder : MonoBehaviour
{
    public EnemyStatus EnemyStatusData;
    public EnemyStatus BattleEnemyStatus;
    

    protected bool _defeated = false;
    protected PlayerStatus[] _playerStatuses;

    private void OnEnable()
    {
        _playerStatuses = Resources.FindObjectsOfTypeAll<PlayerStatus>();
        if (BattleEnemyStatus.CharName == EnemyStatusData.CharName && BattleEnemyStatus.Defeated)
        {
            _defeated = true;
            BattleEnemyStatus.Defeated = false;
            EnemyStatusData.Defeated = true;
            foreach (var player in _playerStatuses)
            {
                player.EnemiesDefeated.Add(EnemyStatusData.CharName);
            }
        }

        foreach (var player in _playerStatuses)
        {
            if (player.EnemiesDefeated.Find(e => e == EnemyStatusData.CharName) != null)
            {
                _defeated = true;
            }
        }

        if (_defeated)
        {
            if (transform.parent.GetComponent<EnemyOverworldAI>()) transform.parent.gameObject.SetActive(false);
            else gameObject.SetActive(false);
        }
    }
}
