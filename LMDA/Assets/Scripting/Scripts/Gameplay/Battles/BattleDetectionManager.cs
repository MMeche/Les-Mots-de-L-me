using UnityEngine;
using UnityEngine.Rendering;

public class BattleDetectionManager : MonoBehaviour
{
    [SerializeField] PlayerStatus _playerStatus;
    [SerializeField] EnemyStatus _enemyStatus;
    SceneLoader _sceneLoader;
    bool _isAttacked = false;
    PauseManager _pauseManager;

    private void Start()
    {
        _sceneLoader = ServiceProvider.Instance.ServiceLocator.Get<SceneLoader>();
        _pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "Enemy")
        {
            if (!_isAttacked)
            {
                _isAttacked = true;
                SetBattleData(other);
                SetOverworldPersistentData();

                Destroy(other.gameObject);

                _pauseManager.GetLoadingScreen().LoadScene("TestBattleArena_Matth");
            }
        }
    }

    private void SetOverworldPersistentData()
    {
        _playerStatus.Position = transform.position;
    }

    private void SetBattleData(Collider collider)
    {
        EnemyStatus status = collider.GetComponent<EnemyDataHolder>().EnemyStatusData;
        
        _enemyStatus.CharName = status.CharName;
        _enemyStatus.CharacterGameObject = status.CharacterGameObject;
        _enemyStatus.MaxHealth = status.MaxHealth;
        _enemyStatus.ExpGiven = status.ExpGiven;

        _enemyStatus.PoeticEnabled = status.PoeticEnabled;
        _enemyStatus.PoeticObstacleSpeed = status.PoeticObstacleSpeed;
        _enemyStatus.PoeticObstacleNumber = status.PoeticObstacleNumber;
        _enemyStatus.PoeticObstacleChance = status.PoeticObstacleChance;
        _enemyStatus.PoeticGapBetweenObstacles = status.PoeticGapBetweenObstacles;
        _enemyStatus.PoeticEnemyDamage = status.PoeticEnemyDamage;

        _enemyStatus.LyricEnabled = status.LyricEnabled;
        _enemyStatus.LyricNbOfPoints = status.LyricNbOfPoints;
        _enemyStatus.LyricMaxLengthOfAlternativePaths = status.LyricMaxLengthOfAlternativePaths;
        _enemyStatus.LyricNbOfAlternativePaths = status.LyricNbOfAlternativePaths;
        _enemyStatus.LyricAnimationSpeed = status.LyricAnimationSpeed;
        _enemyStatus.LyricEnemyDamage = status.LyricEnemyDamage;

        _enemyStatus.ArgEnabled = status.ArgEnabled;
        _enemyStatus.ArgNbOfDefensiveCard = status.ArgNbOfDefensiveCard;
        _enemyStatus.ArgNbOfOffensiveCard = status.ArgNbOfOffensiveCard;
        _enemyStatus.ArgNbOfSelectionSlot = status.ArgNbOfSelectionSlot;
        _enemyStatus.ArgNbOfNeutralCard = status.ArgNbOfNeutralCard;
        _enemyStatus.ArgEnemyDamage = status.ArgEnemyDamage;
        _enemyStatus.WordList = status.WordList;
        _enemyStatus.AvailableEnemyCombos = status.AvailableEnemyCombos;

        _enemyStatus.NarrativeEnabled = status.NarrativeEnabled;
        _enemyStatus.NarrativeNumberOfDefensiveQTE = status.NarrativeNumberOfDefensiveQTE;
        _enemyStatus.NarrativeNumberOfOffensiveQTE = status.NarrativeNumberOfOffensiveQTE;
        _enemyStatus.NarrativeEnemyDamage = status.NarrativeEnemyDamage;
        _enemyStatus.MinSpawnTime = status.MinSpawnTime;
        _enemyStatus.MaxSpawnTime = status.MaxSpawnTime;
        _enemyStatus.MinHeightSpawnDistance = status.MinHeightSpawnDistance;
        _enemyStatus.MaxHeightSpawnDistance = status.MaxHeightSpawnDistance;
        _enemyStatus.MinWidthSpawnDistance = status.MinWidthSpawnDistance;
        _enemyStatus.MaxWidthSpawnDistance = status.MaxWidthSpawnDistance;
        _enemyStatus.IndicatorShrinkSpeedModifier = status.IndicatorShrinkSpeedModifier;

        _enemyStatus.WordList = status.WordList;
}
}
