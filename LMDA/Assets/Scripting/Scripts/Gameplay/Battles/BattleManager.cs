using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleManagerEventBus BattleManagerEventBus;
    public BattleManagerEventContext BattleManagerEventContext;

    [SerializeField] BattlePlatforms _playerPlatform;
    [SerializeField] Transform _playerContainer;
    [SerializeField] List<Color> _outlineColors;
    [SerializeField] Transform _enemyPlatformTransform;
    [SerializeField] GameObject _selectionMark;

    [SerializeField] Transform _enemyDamageTarget;
    [SerializeField] EnemyStatus _enemyStatus;
    [SerializeField] Transform _playerDamageTarget;
    [SerializeField] private List<PlayerStatus> _playerStatuses;
    [SerializeField] TutorialState _tutorialState;

    [SerializeField] GameObject _damagePrefab;

    [SerializeField] PoeticMinigameManager _poeticMinigame;
    PoeticMiniGameLayoutGenerator _poeticLayout;
    [SerializeField] LyricMinigameManager _lyricMinigame;
    LyricMinigameLayoutGenerator _lyricLayout;
    [SerializeField] QTEManager _narrativeMinigame;
    [SerializeField] ArgumentativeManager _argumentativeMinigame;

    BattleUIManager _battleUIManager;
    BattleFlowStateMachine _battleFlowStateMachine;
    AudioManager _audioManager;
    FMODEvents _sfxEvents;

    SceneLoader _sceneLoader;

    EventInstance _mainBattleMusic;

    PlayerStatus _activePlayer;
    public bool[] PlayerStillAlive;
    public bool[] HasPlayed;

    float _playerCurrentHealth;
    float _enemyCurrentHealth;
    int _turnCount = 0;
    bool _camIsDoneMoving=false;
    bool _playerStrike=false;
    bool _enemyStrike = false;

    private void OnEnable()
    {
        BattleManagerEventBus = new();
        BattleManagerEventContext = new();

        ServiceProvider.Instance.ServiceLocator.Register(this);

        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _sceneLoader = ServiceProvider.Instance.ServiceLocator.Get<SceneLoader>();
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.StartBattle += BattleStartInit;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionDone += ValidateSelection;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionChanged += ChangeSelection;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayersTurn += NewTurn; //Generic name for this can be renamed for further purposes
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnBattleWin += AddExp;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnPlayerAttackStrike += PlayerStrike;
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnEnemyAttackStrike += EnemyStrike;
        BattleManagerEventBus.OnPlayerAttacked += UpdatePlayerHealth;
        BattleManagerEventBus.OnEnemyAttacked += UpdateEnemyHealth;
        BattleManagerEventBus.OnPlayerDefence += Defend;
        BattleManagerEventBus.OnPlayerPoetic += LaunchPoeticMinigame;
        BattleManagerEventBus.OnPlayerLyric += LaunchLyricMinigame;
        BattleManagerEventBus.OnPlayerNarrative += LaunchNarrativeMinigame;
        BattleManagerEventBus.OnPlayerArgumentative += LaunchArgumentativeMinigame;
        BattleManagerEventBus.OnCamPlaced += CamPlaced;

        _activePlayer = _playerStatuses[1];
        PlayerStillAlive = new bool[3];
        HasPlayed = new bool[3];
        for(int i = 0; i < PlayerStillAlive.Length; i++)
        {
            PlayerStillAlive[i] = true;
        }

        _mainBattleMusic = _audioManager.AttachToGameObject(_sfxEvents.LevelTheme[0],gameObject);
        _mainBattleMusic.start();
    }

 

    private void OnDisable()
    {
        BattleManagerEventBus.OnPlayerAttacked -= UpdatePlayerHealth;
        BattleManagerEventBus.OnEnemyAttacked -= UpdateEnemyHealth;
        BattleManagerEventBus.OnPlayerDefence -= Defend;
        BattleManagerEventBus.OnPlayerPoetic -= LaunchPoeticMinigame;
        BattleManagerEventBus.OnPlayerLyric -= LaunchLyricMinigame;
        BattleManagerEventBus.OnPlayerNarrative -= LaunchNarrativeMinigame;

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.StartBattle -= BattleStartInit;

        foreach(var status in _playerStatuses)
        {
            status.Health = status.MaxHealth;
        }
        _audioManager.RemoveFromGameObject(_mainBattleMusic,STOP_MODE.ALLOWFADEOUT);
        ServiceProvider.Instance.ServiceLocator.Unregister<BattleManager>();
    }

    private void Defend(BattleManagerEventContext obj)
    {
        _turnCount = 0;
        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateEnemyTurn);
    }

    private void UpdatePlayerHealth(BattleManagerEventContext obj)
    {
        float damage = obj.DamageDone;
        _playerStrike = false;
        _enemyStrike = false;
        //if (_turnCount < 3)
        //    damage = obj.DamageDone / 3f;

        _activePlayer.Health -= damage;

        float newHealth = _activePlayer.Health / _activePlayer.MaxHealth;
        _battleUIManager.BattleUIEventBus.UpdatePlayerStatus(new() { NewHealth = newHealth });
        if (_battleFlowStateMachine.GetCurrentState() == _battleFlowStateMachine.BattleStates.BattleFlowStateArgumentative)
        {
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAnimationManager>().Attack();
            StartCoroutine(DamagePopUpSpawn((int)damage, DamageType.EnemyDamage, _playerDamageTarget, _sfxEvents.PlayerDamageSFX));
        }
        else
        {
            //Spawning the Damage PopUp
            DamagePopUps popUp = Instantiate(_damagePrefab, _playerDamageTarget).GetComponentInChildren<DamagePopUps>();
            popUp.SetDamage(damage);
            popUp.SetType(DamageType.EnemyDamage);

            _audioManager.PlayOneShotAttached(_sfxEvents.PlayerDamageSFX, _playerDamageTarget.gameObject);
        }
        if (_activePlayer.Health > 0)
        {
            _turnCount++;
        }
        else
        {
            PlayerStillAlive[_playerStatuses.FindIndex(ps => ps == _activePlayer)] = false;
            //_playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps == _activePlayer)).gameObject.SetActive(false);
            _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps == _activePlayer)).GetComponent<PlayerAnimationManager>().Dead();
            _battleFlowStateMachine.BattleFlowStateMachineEventBus.PlayerDied(new() { });
            



            foreach (var b in PlayerStillAlive)
            {
                if (b == true) return;
            }
            _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateEndCombat);
        }
        
    }

    private void UpdateEnemyHealth(BattleManagerEventContext obj)
    {
        _enemyCurrentHealth -= obj.DamageDone;
        _playerStrike = false;
        _enemyStrike = false;
        float newHealth = _enemyCurrentHealth / _enemyStatus.MaxHealth;
        _battleUIManager.BattleUIEventBus.UpdateEnemyStatus(new() { NewHealth = newHealth });
        if (_battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStatePoetic && _battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowState_Narrative)
        {
            _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps == _activePlayer)).GetComponent<PlayerAnimationManager>().Attack();
            StartCoroutine(DamagePopUpSpawn((int)obj.DamageDone,DamageType.PlayerDamage,_enemyDamageTarget,_sfxEvents.EnemyDamageSFX));
        }
        else
        {
            
            DamagePopUps popUp = Instantiate(_damagePrefab,_enemyDamageTarget).GetComponentInChildren<DamagePopUps>();
            popUp.SetDamage((int)obj.DamageDone);
            popUp.SetType(DamageType.PlayerDamage);

            _audioManager.PlayOneShotAttached(_sfxEvents.EnemyDamageSFX, _enemyDamageTarget.gameObject);
        }
        if(_battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStateArgumentative)
        {
            StartCoroutine(WaitHit(DamageType.PlayerDamage,false));
        }

        if (_enemyCurrentHealth < 0)
        {
            EnemyAnimationManager animatorManager = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAnimationManager>();
            if (_battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowStatePoetic && _battleFlowStateMachine.GetCurrentState() != _battleFlowStateMachine.BattleStates.BattleFlowState_Narrative) StartCoroutine(WaitHit(DamageType.PlayerDamage, true));
            else animatorManager.Dead();
            _enemyStatus.Defeated = true;
            StartCoroutine(WaitForLoad(animatorManager));
        }
    }


    private void PlayerStrike(BattleFlowStateMachineEventContext ctx)
    {
        _playerStrike = true;
    }

    private void EnemyStrike(BattleFlowStateMachineEventContext ctx)
    {
        _enemyStrike = true;
    }

    IEnumerator DamagePopUpSpawn(int damage, DamageType damageType,Transform target,FMODUnity.EventReference sfx)
    {
        if(damageType == DamageType.EnemyDamage)
        {
            while (!_playerStrike) yield return null;
        }
        else
        {
            while (!_enemyStrike) yield return null;
        }
        _playerStrike = false;
        _enemyStrike = false;
        //Spawning the Damage PopUp
        yield return new WaitForFixedUpdate();
        DamagePopUps popUp = Instantiate(_damagePrefab, target).GetComponentInChildren<DamagePopUps>();
        popUp.SetDamage(damage);
        popUp.SetType(damageType);

        _audioManager.PlayOneShotAttached(sfx, target.gameObject);

    }

    IEnumerator WaitHit(DamageType damageType,bool lethal)
    {
        if(damageType == DamageType.PlayerDamage)
        {
            while (!_playerStrike)
            {
                yield return null;
            }
            if(!lethal)GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAnimationManager>().Hit();
            else GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAnimationManager>().Dead();
        }
    }

    IEnumerator WaitForLoad(EnemyAnimationManager animatorManager)
    {
        while (!animatorManager.Animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            yield return null;
        }

        while (animatorManager.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
        
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.BattleWin(new() { ExpToAdd = _enemyStatus.ExpGiven });

        StopCoroutine("WaitForLoad");
    }

    private void LaunchPoeticMinigame(BattleManagerEventContext obj)
    {
        _poeticLayout = _poeticMinigame.GetComponent<PoeticMiniGameLayoutGenerator>();
        //Set the poetic context parameters here TODO : must depend on the enemy and not be hard coded !!
        _poeticLayout.ObstacleSpeed = _enemyStatus.PoeticObstacleSpeed;
        _poeticLayout.ObstacleNumber = _enemyStatus.PoeticObstacleNumber;
        _poeticLayout.ObstacleChance = _enemyStatus.PoeticObstacleChance;
        _poeticLayout.GapBetweenObstacles = _enemyStatus.PoeticGapBetweenObstacles;
        _poeticMinigame.ContactAllowed = 0;
        _poeticMinigame.EnemyDamage = _enemyStatus.PoeticEnemyDamage;


        _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps.Equals(_activePlayer))).transform.position = _playerPlatform.transform.GetChild(1).position; // On place le joueur sélectionné au milieu.
        
        _playerDamageTarget = _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps.Equals(_activePlayer)));
        _enemyDamageTarget = GameObject.FindGameObjectWithTag("Enemy").transform;


        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePoetic);
        _mainBattleMusic = _audioManager.SwapInstances(_mainBattleMusic, _sfxEvents.LevelTheme[1], gameObject);
        _mainBattleMusic.start();
        StartCoroutine(PoeticWaitForCamPlacement());
    }

    private void LaunchLyricMinigame(BattleManagerEventContext obj)
    {
        _lyricLayout = _lyricMinigame.GetComponent<LyricMinigameLayoutGenerator>();
        _lyricLayout.NbOfPoints = _enemyStatus.LyricNbOfPoints;
        _lyricLayout.MaxLengthOfAlternativePaths = _enemyStatus.LyricMaxLengthOfAlternativePaths;
        _lyricLayout.NbOfAlternativePaths = _enemyStatus.LyricNbOfAlternativePaths;

        
        _lyricMinigame.AnimationSpeed = _enemyStatus.LyricAnimationSpeed;
        _lyricMinigame.EnemyDamage = _enemyStatus.LyricEnemyDamage;
        

        _playerDamageTarget = _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps.Equals(_activePlayer)));
        _playerDamageTarget.GetComponent<CapsuleCollider>().enabled = false;
        _enemyDamageTarget = GameObject.FindGameObjectWithTag("Enemy").transform;

        StartCoroutine(LyricWaitForCamPlacement());
        _mainBattleMusic = _audioManager.SwapInstances(_mainBattleMusic, _sfxEvents.LevelTheme[2], gameObject);
        _mainBattleMusic.start();
        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateLyric);
        
    }

    private void LaunchArgumentativeMinigame(BattleManagerEventContext obj)
    {
        ArgumentativeManager argContext = _argumentativeMinigame.GetComponent<ArgumentativeManager>();
        argContext.MaxNbOfDefensiveCard = _enemyStatus.ArgNbOfDefensiveCard;
        argContext.MaxNbOfOffensiveCard = _enemyStatus.ArgNbOfOffensiveCard;
        argContext.NbOfSelectionSlot = _enemyStatus.ArgNbOfSelectionSlot;
        argContext.NbOfNeutralCard = _enemyStatus.ArgNbOfNeutralCard;
        argContext.EnemyDamage = _enemyStatus.ArgEnemyDamage;
        argContext.EnemyLoadedCombo = _enemyStatus.AvailableEnemyCombos[UnityEngine.Random.Range(0, _enemyStatus.AvailableEnemyCombos.Count)];


        foreach (var sentence in _enemyStatus.WordList.DescriptionSentences)
        {
            argContext.EnemyHintSentences.Add(sentence);
        }

        _playerDamageTarget = _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps.Equals(_activePlayer)));
        _enemyDamageTarget = GameObject.FindGameObjectWithTag("Enemy").transform;


        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStateArgumentative);
        _mainBattleMusic = _audioManager.SwapInstances(_mainBattleMusic, _sfxEvents.LevelTheme[3], gameObject);
        _mainBattleMusic.start();
        StartCoroutine(ArgumentativeWaitForCamPlacement());
    }

    private void LaunchNarrativeMinigame(BattleManagerEventContext obj)
    {
        _narrativeMinigame.NumberOfDefensiveQTE = _enemyStatus.NarrativeNumberOfDefensiveQTE;
        _narrativeMinigame.NumberOfOffensiveQTE = _enemyStatus.NarrativeNumberOfOffensiveQTE;
        _narrativeMinigame.EnemyDamage = _enemyStatus.NarrativeEnemyDamage;
        _narrativeMinigame._maxHeightSpawnDistance = _enemyStatus.MaxHeightSpawnDistance;
        _narrativeMinigame._minHeightSpawnDistance = _enemyStatus.MinHeightSpawnDistance;
        _narrativeMinigame._maxWidthSpawnDistance = _enemyStatus.MaxWidthSpawnDistance;
        _narrativeMinigame._minWidthSpawnDistance = _enemyStatus.MinWidthSpawnDistance;
        _narrativeMinigame._maxSpawnTimeInterval = _enemyStatus.MaxSpawnTime;
        _narrativeMinigame._minSpawnTimeInterval = _enemyStatus.MinSpawnTime;

        _playerDamageTarget = _playerContainer.GetChild(_playerStatuses.FindIndex(ps => ps.Equals(_activePlayer)));
        _enemyDamageTarget = GameObject.FindGameObjectWithTag("Enemy").transform;


        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowState_Narrative);
        _mainBattleMusic = _audioManager.SwapInstances(_mainBattleMusic, _sfxEvents.LevelTheme[4], gameObject);
        _mainBattleMusic.start();
        StartCoroutine(NarrativeWaitForCamPlacement());
    }

    IEnumerator PoeticWaitForCamPlacement()
    {
        while (!_camIsDoneMoving)
        {
            yield return null;
        }

        if (_enemyStatus != null)
        {
            //Paramètre en fonction de l'ennemi
        }

        _camIsDoneMoving = false;
        _poeticMinigame.gameObject.SetActive(true);
    }

    IEnumerator ArgumentativeWaitForCamPlacement()
    {
        while (!_camIsDoneMoving)
        {
            yield return null;
        }

        if (_enemyStatus != null)
        {
            //Paramètre en fonction de l'ennemi
        }

        _camIsDoneMoving = false;
        _argumentativeMinigame.gameObject.SetActive(true);
    }

    // J'ai mis ça dans l'optique d'avoir des travelling de cam
    // pour le placement des cams pendant le combat
    IEnumerator NarrativeWaitForCamPlacement()
    {
        while (!_camIsDoneMoving)
        {
            yield return null;
        }

        if (_enemyStatus != null)
        {
            //Paramètre en fonction de l'ennemi
        }

        _camIsDoneMoving = false;
        _narrativeMinigame.gameObject.SetActive(true);
    }

    IEnumerator LyricWaitForCamPlacement()
    {
        while (!_camIsDoneMoving)
        {
            yield return null;
        }

        if (_enemyStatus != null)
        {
            //Paramètre en fonction de l'ennemi
        }

        _camIsDoneMoving = false;
        _lyricMinigame.gameObject.SetActive(true);
    }

    private void CamPlaced(BattleManagerEventContext obj)
    {
        _camIsDoneMoving = true;
    }

    private void BattleStartInit(BattleFlowStateMachineEventContext obj)
    {
        _playerCurrentHealth = _activePlayer.Health;
        _enemyCurrentHealth = _enemyStatus.MaxHealth;
         _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnUISetup(new BattleFlowStateMachineEventContext
        {
            EnemyStatus = _enemyStatus,
            PlayerStatus = _activePlayer
        });

        

        InstantiateModels();
        _battleFlowStateMachine.ChangeState(_battleFlowStateMachine.BattleStates.BattleFlowStatePlayerTurn);
    }

    private void ChangeSelection(BattleFlowStateMachineEventContext obj)
    {
        _activePlayer =  obj.PlayerStatus;
    }

    private void ValidateSelection(BattleFlowStateMachineEventContext obj)
    {
        _activePlayer = obj.PlayerStatus;
        
        for(int i = 0; i < _playerStatuses.Count; i++)
        {
            if (_activePlayer != _playerStatuses[i])
            {
                _playerContainer.GetChild(i).gameObject.SetActive(false);
            }
            else HasPlayed[i] = true;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _selectionMark.SetActive(false);
    }

    public void CancelSelection()
    {
        for (int i = 0; i < _playerStatuses.Count; i++)
        {
            if (_activePlayer != _playerStatuses[i])
            {
                _playerContainer.GetChild(i).gameObject.SetActive(true);
            }
            else HasPlayed[i] = false;
        }
        _selectionMark.SetActive(true);
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnSelectionCancel(new());
    }

    private void NewTurn(BattleFlowStateMachineEventContext obj)
    {
        for(int i = 0; i < _playerContainer.childCount; i++)
        {
            _playerContainer.GetChild(i).gameObject.SetActive(true);
            _playerContainer.GetChild(i).position = _playerPlatform.transform.GetChild(i).position;

            if (PlayerStillAlive[i])
            {
                _playerContainer.GetChild(i).GetComponent<CapsuleCollider>().enabled = true;
            }
            else
            {
                _playerContainer.GetChild(i).GetComponentsInChildren<Animator>()[1].SetBool("Dead", true);
                _playerContainer.GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        _mainBattleMusic =_audioManager.SwapInstances(_mainBattleMusic, _sfxEvents.LevelTheme[0], gameObject);
        _mainBattleMusic.start();
        if (PlayerStillAlive[1])_activePlayer = _playerStatuses[1];
        else if (PlayerStillAlive[2]) _activePlayer = _playerStatuses[2];
        else _activePlayer = _playerStatuses[0];


        if (Array.TrueForAll(HasPlayed, p => p)) //Si tout le monde a jouer, on reset _hasPLayed
        {
            for(int i = 0; i < HasPlayed.Length; i++)
            {
                if (PlayerStillAlive[i])
                    HasPlayed[i] = false;
            }
        }
        for (int i = 0; i < HasPlayed.Length; i++)
        {
            _playerContainer.GetChild(i).GetComponentsInChildren<Animator>()[1].SetBool("HasPlayed", HasPlayed[i] && PlayerStillAlive[i]);

        }

        _selectionMark.SetActive(true);
    }


    private void AddExp(BattleFlowStateMachineEventContext ctx)
    {
        foreach(var p in _playerStatuses)
        {
            p.Experience += ctx.ExpToAdd;
        }
    }
    private void InstantiateModels()
    {
        for(int i = 0; i < _playerStatuses.Count; i++)
        {
            GameObject player = Instantiate(
                _playerStatuses[i].CharacterGameObject,
                _playerPlatform.transform.GetChild(i).position,
                _playerPlatform.transform.GetChild(i).rotation,
                _playerContainer
                );
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Outline>().OutlineColor = _outlineColors[i];
        }

        GameObject enemy = Instantiate(
            _enemyStatus.CharacterGameObject, 
            _enemyPlatformTransform.position,
            _enemyPlatformTransform.rotation);

        enemy.gameObject.transform.parent = null;
    }

    public void PoeticTutorial()
    {
        if (_tutorialState.TutoPoetic)
        {
            BattleManagerEventBus.PoeticMinigame(new BattleManagerEventContext { });
            return;
        }
        _battleUIManager.BattleUIEventBus.DisplayTuto(new() { TutoPanelName = "Poetic" });
        _tutorialState.TutoPoetic = true;
    }

    public void LyricTutorial()
    {
        if (_tutorialState.TutoLyric)
        {
            BattleManagerEventBus.LyricMinigame(new BattleManagerEventContext { });
            return;
        }
        _battleUIManager.BattleUIEventBus.DisplayTuto(new() { TutoPanelName = "Lyric" });
        _tutorialState.TutoLyric = true;
    }

    public void NarrativeTutorial()
    {
        if (_tutorialState.TutoNarrative)
        {
            BattleManagerEventBus.NarrativeMinigame(new BattleManagerEventContext { });
            return;
        }
        _battleUIManager.BattleUIEventBus.DisplayTuto(new() { TutoPanelName = "Narrative" });
        _tutorialState.TutoNarrative = true;
    }

    public void ArgumentativeTutorial()
    {
        if (_tutorialState.TutoArgumentative)
        {
            BattleManagerEventBus.ArgumentativeMinigame(new BattleManagerEventContext { });
            return;
        }
        _battleUIManager.BattleUIEventBus.DisplayTuto(new() { TutoPanelName = "Argumentative" });
        _tutorialState.TutoArgumentative = true;
    }


    public void Poetic()
    {
        BattleManagerEventBus.PoeticMinigame(new BattleManagerEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnUIActivated(new()
        {
            UINames = new string[] { "Poetic" }
        });
    }

    public void Lyric()
    {

        BattleManagerEventBus.LyricMinigame(new BattleManagerEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnUIActivated(new()
        {
            UINames = new string[] { "Lyric" }
        });
    }

    public void Narrative()
    {
        BattleManagerEventBus.NarrativeMinigame(new BattleManagerEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnUIActivated(new()
        {
            UINames = new string[] { "Narrative" }
        });
    }

    public void Argumentative()
    {

        BattleManagerEventBus.ArgumentativeMinigame(new BattleManagerEventContext { });
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.OnUIActivated(new()
        {
            UINames = new string[] { "Argumentative" }
        });
    }

    public List<PlayerStatus> GetPlayerStatuses()
    {
        return _playerStatuses;
    }

    public PlayerStatus GetActivePlayerStatus()
    {
        return _activePlayer;
    }

    public EnemyStatus GetEnemyStatus()
    {
        return _enemyStatus;
    }

    public float GetEnemyCurrentHealth()
    { 
        return _enemyCurrentHealth; 
    }

    private void OnApplicationQuit()
    {
        _tutorialState.Reset();
    }
}
