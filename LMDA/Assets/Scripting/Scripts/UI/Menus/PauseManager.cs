using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ink.Parsed;
using TMPro;
#if UNITY_EDITOR
    using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.SearchService;

#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool IsPaused;
    [SerializeField] GameObject _uiPause;
    [SerializeField] GameObject _uiDebug;
    [SerializeField] LoadSectionManager _loadSection;
    [SerializeField] GameObject _saveSection;
    [SerializeField] GameObject _deleteConfirmSection;
    [SerializeField] GameObject _settingsSection;
    [SerializeField] GameObject _audioSection;
    [SerializeField] LoadingScreen _loadingScreen;
    [SerializeField] List<List<GameObject>> _uiComponents;
    

    public GameObject FirstSelectedPause;
    int _lvlUI = 0; // 0 : root , 1 : first lvl (save section, commands), 2 : second, etc
    
    FlowStateMachine _fsm;

    PauseManager _pauseManager;

    private void OnEnable()
    {
        _uiComponents = new();
        
        ServiceProvider.Instance.ServiceLocator.Register(this);

        //_pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();

        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.LevelFinished += EndPause;
        
    }

    private void EndPause(FlowStateMachineEventContext obj)
    {
        IsPaused = true;
        Time.timeScale = 0f;
        _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
    }

    
    public void PauseGame()
    {
        _lvlUI = 0;
        _uiComponents = new();
        _uiComponents.Add(null);
        _uiComponents.Add(null);
        IsPaused = true;
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(FirstSelectedPause);
        if (_uiPause != null)
        {
            _uiPause.SetActive(true);
            _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
        }
    }    

    public void DebugPause()
    {
        _lvlUI = 0;
        IsPaused = true;
        Time.timeScale = 0f;
        if(_uiDebug != null)
        {
            _uiDebug.SetActive(true);
            _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
        }
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        if(_uiPause != null)
        {
            _uiPause.SetActive(false);
            _uiDebug.SetActive(false);
            _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterOverWorld);
        }
    }

    public void Cancel()
    {
        if (_lvlUI == 0 && (_uiPause.activeSelf||_uiDebug.activeSelf)) UnpauseGame();
        if (_lvlUI > 0)
        {
            for(int i =0; i<_uiComponents[_lvlUI-1].Count;i++)
            {
                _uiComponents[_lvlUI-1][i].SetActive(false); 
            }
            _uiComponents[_lvlUI - 1].Clear();
            _lvlUI--;
            
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        if (_uiPause != null)
        {
            _uiPause.SetActive(false);
        }
        //string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        //string currentFile = files[0];
        //foreach(var file in files)
        //{
        //    if (File.GetCreationTime(file) > File.GetCreationTime(currentFile)) currentFile = file;
        //}
        //LoadSystem.LoadGameData(currentFile);
        _loadingScreen.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TittleScreen()
    {

        Time.timeScale = 1f;
        if (_uiPause != null)
        {
            _uiPause.SetActive(false);
        }
        _loadingScreen.LoadScene("Scenes/TittleScreen");
    }

    public void Settings()
    {
        _settingsSection.SetActive(true);
        _uiComponents[0] = GameObject.FindGameObjectsWithTag("UILvl1").ToList<GameObject>();
        _lvlUI = 1;
    }

    public void Audio()
    {
        _audioSection.SetActive(true);
        _uiComponents[1] = (GameObject.FindGameObjectsWithTag("UILvl2").ToList<GameObject>());
        _lvlUI = 2;
    }

    public void OnLoadMenu()
    {
        _loadSection.gameObject.SetActive(true);
        _uiComponents[0] = (GameObject.FindGameObjectsWithTag("UILvl1").ToList<GameObject>());
        _lvlUI = 1;
    }

    public void OnLoadedGameLaunch()
    {
        SaveData lastSave = LoadSystem.LoadGameData(_loadSection.GetComponent<LoadSectionManager>().GetSaveFile());
        Cancel();
        Cancel();
        _loadingScreen.LoadScene("Niveau_1-Level Art");
    }
    private void OnDisable()
    {
        IsPaused = false;
    }

    public void OnSaveButton()
    {
        _saveSection.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_saveSection.transform.GetChild(0).GetChild(0).gameObject);
        _uiComponents[1]= (GameObject.FindGameObjectsWithTag("UILvl2").ToList<GameObject>());
        _lvlUI = 2;
    }

    public void OnDeleteButton()
    {
        _deleteConfirmSection.SetActive(true);
        _uiComponents[1] = (GameObject.FindGameObjectsWithTag("UILvl2").ToList<GameObject>());
        _lvlUI = 2;
    }

    public void OnSave(TMP_Text text)
    {
        string filePath = "/"+text.text+".json";
        
        //TEMPORARY : TESTING THE SAVE/LOAD SYSTEM
        //THE POSITION OF THE PLAYER STATUS MUST BE UPDATED AT RUNTIME NOT IN PAUSE

        Vector3 savePos = GameObject.FindGameObjectWithTag("Player").transform.position;
        List<PlayerStatus> statuses = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
        statuses[0].Position = savePos + new Vector3(0,0.5f,0);
        statuses[1].Position = savePos + new Vector3(0.5f, 0, 0.5f);
        statuses[2].Position = savePos + new Vector3(-0.5f, 0, 0.5f);
        

        SaveSystem.SaveGameState(filePath);
        _loadSection.GetComponent<LoadSectionManager>().RefreshList();
        Cancel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToOverWorld()
    {
        _loadingScreen.LoadScene("Niveau_1-Level Art");
    }

    public LoadingScreen GetLoadingScreen()
    {
        return _loadingScreen;
    }
}
