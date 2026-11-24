using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnemyDataHolder[] _enemiesDataHolder;
    [SerializeField] PlayerStatus _playerStatus;
    [SerializeField] PlayerStatus _followerLeftStatus;
    [SerializeField] PlayerStatus _followerRightStatus;
    [SerializeField] EnemyStatus _enemyStatus;
    [SerializeField] TutorialState _tutoStatus;
    [SerializeField] InteractedSteleData _journalData;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _followerLeft;
    [SerializeField] GameObject _followerRight;
    [SerializeField] Transform _skipPos;


    FlowStateMachine _fsm;
    AudioManager _audioManager;
    FMODEvents _sfxEvents;
    EventInstance _levelTheme;

    

    private void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _enemiesDataHolder = FindObjectsByType<EnemyDataHolder>(FindObjectsSortMode.None);
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();

        _player.transform.position = _playerStatus.Position;
        _fsm.FlowStateMachineEventBus.OnSteleInteraction += ThemeStop;
        _fsm.FlowStateMachineEventBus.OnSteleExit += ThemePlay;
        _fsm.FlowStateMachineEventBus.StatTutorialFinished += TutoStatDone;
        _fsm.FlowStateMachineEventBus.JournalTutorialFinished += TutoJournal;
        _levelTheme = _audioManager.AttachToGameObject(_sfxEvents.LevelTheme[0], gameObject);
        _levelTheme.start();
    }

    private void OnDisable()
    {
        _audioManager.RemoveFromGameObject(_levelTheme,FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
    }

    void ThemeStop(FlowStateMachineEventContext ctx)
    {
        _levelTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void TutoStatDone(FlowStateMachineEventContext ctx)
    {
        _tutoStatus.TutoStatDone = true;
    }

    void TutoJournal(FlowStateMachineEventContext ctx)
    {
        _tutoStatus.TutoJournalDone = true;
    }

    void ThemePlay(FlowStateMachineEventContext ctx)
    {
        _levelTheme.start();
    }

    public void SkipTuto()
    {
        _player.transform.position = _skipPos.position;
        _followerLeft.transform.position = _skipPos.position;
        _followerRight.transform.position = _skipPos.position;
        _tutoStatus.TutoStatDone = true;
        _tutoStatus.TutoJournalDone = true;
        _tutoStatus.TutoMoveDone = true;
        _tutoStatus.TutorialDefeated = new() {"Tutoriel Poétique","Tutoriel Narratif","Tutoriel Argumentatif","Tutoriel Lyrique"};
        _tutoStatus.DoorPartsOpened = 4;
    }

    public void ResetOverworld()
    {
        _tutoStatus.Reset();
        _playerStatus.Reset();
        _journalData.Reset();
        _followerLeftStatus.Reset();
        _followerRightStatus.Reset();

    }

    private void OnApplicationQuit()
    {
        ResetOverworld();
    }
}
