#if UNITY_EDITOR
using Mono.Cecil;
#endif
using UnityEngine;

public class BossDataHolder : EnemyDataHolder
{
    [SerializeField] GameObject _lvlCompletedScreen;
    FlowStateMachine _fsm;
    
    
    private void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _playerStatuses = Resources.FindObjectsOfTypeAll<PlayerStatus>();
        if (BattleEnemyStatus.CharName == EnemyStatusData.CharName && BattleEnemyStatus.Defeated)
        {
            _defeated = true;
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
            BattleEnemyStatus.Defeated = false;
            
        }
    }

    private void Start()
    {
        if (_defeated)
        {
            _fsm.FlowStateMachineEventBus.OnLevelFinish(new());
            _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "LevelCompleted" } });
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            gameObject.SetActive(false);
        }
        
    }
}
