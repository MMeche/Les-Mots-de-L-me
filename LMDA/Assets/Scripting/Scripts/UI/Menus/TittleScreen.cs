#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System;
using System.IO;
using Ink.Runtime;
using System.Linq;
using FMOD.Studio;

public class TittleScreen : MonoBehaviour
{
    [SerializeField] string NextSceneName;
    [SerializeField] VideoPlayer VideoPlayer;
    [SerializeField] GameObject UI;
    [SerializeField] PlaySection _playSection;
    [SerializeField] LoadSectionManager _loadSection;
    [SerializeField] GameObject _audioSection;
    [SerializeField] LoadingScreen _loadingScreen;
    [SerializeField] GameObject _settingsSection;
    [SerializeField] GameObject _tittleIcon;
    
    //[SerializeField] AudioSource _musicMenu;

    [SerializeField] List<GameObject> _firstLvlUIComponents;
    [SerializeField] List<GameObject> _secondLvlUIComponents;

    [SerializeField] List<PlayerStatus> _playerStatuses;
    [SerializeField] TutorialState _tutoStatus;
    [SerializeField] InteractedSteleData _interactedSteles;
    
    EventInstance _titleTheme;
    bool _firstUILvlEnable = false;
    bool _secondUILvlEnable = false;
    private bool _isVideoFinished = false;
    private bool _isSceneLoaded = false;
    InputSystemUIInputModule inputModule;

    AudioManager _audioManager;
    FMODEvents _sfxEvents;
    
    private void OnEnable()
    {
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();

        inputModule = FindFirstObjectByType<InputSystemUIInputModule>();
        Cursor.lockState = CursorLockMode.None;
        inputModule.cancel.action.performed += Escape;
        _titleTheme = _audioManager.AttachToGameObject(_sfxEvents.LevelTheme[0], gameObject);
        _titleTheme.start();
    }

    private void OnDisable()
    {
        inputModule.cancel.action.performed -= Escape;
        _audioManager.RemoveFromGameObject(_titleTheme,STOP_MODE.ALLOWFADEOUT);
    }

    public void OnIntroductionButtonPressed()
    {
        UI.SetActive(false);
        foreach (var player in _playerStatuses)
        {
            player.Reset();
        }
        _tutoStatus.Reset();
        _interactedSteles.Reset();
        StartCoroutine(PlayVideoAndLoadScene());
    }

    public void OnQuitButtonPressed() 
    {
        Application.Quit();
    }

    IEnumerator PlayVideoAndLoadScene()
    {
        VideoPlayer.loopPointReached += OnVideoFinished;
        _audioManager.RemoveFromGameObject(_titleTheme, STOP_MODE.ALLOWFADEOUT);
        _titleTheme = _audioManager.AttachToGameObject(_sfxEvents.SteleVoice[0], gameObject);
        _titleTheme.start();
        VideoPlayer.Play();

        // Commencer à charger la scène en arrière-plan
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        _isSceneLoaded = true;

        // Attendre aussi que la vidéo soit terminée
        while (!_isVideoFinished)
        {
            yield return null;
        }

        // Activer la scène une fois que tout est prêt
        asyncLoad.allowSceneActivation = true;
    }

    private void OnVideoFinished(VideoPlayer source) 
    {
        _isVideoFinished = true;
        UI.SetActive(false);
        
    }

    public void OnPlayButtonPressed(Button b) 
    {
        if (_firstUILvlEnable)
        {
            _firstUILvlEnable = false;
            foreach (var c in _firstLvlUIComponents)
            {
                c.GetComponent<Button>().interactable = true;
            }
            _playSection.gameObject.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(_firstLvlUIComponents[0]);
        }
        else
        {
            _firstUILvlEnable = true;
            foreach (var c in _firstLvlUIComponents)
            {
                c.gameObject.SetActive(false);
            }
            b.interactable = true;
            _playSection.gameObject.SetActive(true);
            //EventSystem.current.SetSelectedGameObject(_playSection.transform.GetChild(0).gameObject);
        }
        
        
    }

    public void OnSettingsButtonPressed(Button b)
    {
        if (_firstUILvlEnable)
        {
            _firstUILvlEnable = false;
            foreach (var c in _firstLvlUIComponents)
            {
                c.GetComponent<Button>().interactable = true;
            }
            _settingsSection.SetActive(false);
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126); ; //-373, 126
            //EventSystem.current.SetSelectedGameObject(_firstLvlUIComponents[0]);
        }
        else
        {
            _firstUILvlEnable = true;
            foreach (var c in _firstLvlUIComponents)
            {
                c.gameObject.SetActive(false);
            }
            b.interactable = true;
            _settingsSection.SetActive(true);
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 283);
            //EventSystem.current.SetSelectedGameObject(_settingsSection.transform.GetChild(0).gameObject);
        }
    }

    public void OnAudioButtonPressed(Button b)
    {
        if (_secondUILvlEnable)
        {
            _secondUILvlEnable = false;
            foreach (var c in _secondLvlUIComponents)
            {
                c.GetComponent<Button>().interactable = true;
            }
            _audioSection.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(_audioSection.transform.GetChild(0).gameObject);
        }
        else
        {
            _secondUILvlEnable = true;
            foreach (var c in _secondLvlUIComponents)
            {
                c.gameObject.SetActive(false);
            }
            b.interactable = true;
            _audioSection.SetActive(true); //the focus is done inside this call...

        }
    }

    public void OnNewGameButtonPressed()
    {
        UI.SetActive(false);
        foreach(var player in _playerStatuses)
        {
            player.Reset();
        }
        _tutoStatus.Reset();
        _interactedSteles.Reset();
        _loadingScreen.LoadScene(NextSceneName);
    }

    public void OnLoadGameButtonPressed(Button b)
    {
        if (_secondUILvlEnable)
        {
            _secondUILvlEnable = false;
            foreach (var c in _secondLvlUIComponents)
            {
                c.gameObject.SetActive(true);
            }
            _loadSection.gameObject.SetActive(false);
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126); //-373, 126
            //EventSystem.current.SetSelectedGameObject(_playSection.transform.GetChild(0).gameObject);
        }
        else
        {
            _secondUILvlEnable = true;
            foreach (var c in _secondLvlUIComponents)
            {
                c.gameObject.SetActive(false);
            }
            b.interactable = true;
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 283);  // //-373, 283
            _loadSection.gameObject.SetActive(true); //the focus is done inside this call...
            
        }
    }

    public void OnLoadedGameLaunch()
    {
        SaveData lastSave = LoadSystem.LoadGameData(_loadSection.GetComponent<LoadSectionManager>().GetSaveFile());
        UI.SetActive(false);
        _loadingScreen.LoadScene(NextSceneName);
    }

    public void OnContinuButtonPressed()
    {
        string lastModifiedFile="";
        DateTime time = new();
        foreach(var file in Directory.GetFiles(Application.persistentDataPath, "*.json"))
        {
            if (time < Directory.GetCreationTime(file))
            {
                lastModifiedFile = file;
                time = Directory.GetCreationTime(file);
            }
        }
        SaveData lastSave = LoadSystem.LoadGameData(lastModifiedFile);
        UI.SetActive(false);
        _loadingScreen.LoadScene(NextSceneName);
    }

    private void Escape(InputAction.CallbackContext ctx)
    {
        if (_secondUILvlEnable)
        {
            _secondUILvlEnable = false;
            _loadSection.gameObject.SetActive(false);
            _audioSection.SetActive(false);
            if (_playSection.gameObject.activeSelf)
            {
                //EventSystem.current.SetSelectedGameObject(_playSection.transform.GetChild(0).gameObject);
                
            }
            if(_settingsSection.activeSelf) //EventSystem.current.SetSelectedGameObject(_settingsSection.transform.GetChild(0).gameObject);
            _audioManager.PlayOneShot(_sfxEvents.UICancel);
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126);
            _secondLvlUIComponents[0].SetActive(true);
            return;
        }
        if (_firstUILvlEnable)
        {
            _firstUILvlEnable = false;
            _playSection.gameObject.SetActive(false);
            _settingsSection.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(_firstLvlUIComponents[0]);
            _audioManager.PlayOneShot(_sfxEvents.UICancel);
            foreach (var c in _firstLvlUIComponents)
            {
                c.gameObject.SetActive(true);
                
            }
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126);
        }
        else
        {
            _audioManager.PlayOneShot(_sfxEvents.UIError);
        }
    }

    public void Escape()
    {
        if (_secondUILvlEnable)
        {
            _secondUILvlEnable = false;
            _loadSection.gameObject.SetActive(false);
            _audioSection.SetActive(false);
            if (_playSection.gameObject.activeSelf)
            {
                //EventSystem.current.SetSelectedGameObject(_playSection.transform.GetChild(0).gameObject);
                
            }
            if (_settingsSection.activeSelf) //EventSystem.current.SetSelectedGameObject(_settingsSection.transform.GetChild(0).gameObject);
                _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126);
            _audioManager.PlayOneShot(_sfxEvents.UICancel);
            _secondLvlUIComponents[0].SetActive(true);
            return;
        }
        if (_firstUILvlEnable)
        {
            _firstUILvlEnable = false;
            _playSection.gameObject.SetActive(false);
            _settingsSection.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(_firstLvlUIComponents[0]);
            _audioManager.PlayOneShot(_sfxEvents.UICancel);
            
            foreach (var c in _firstLvlUIComponents)
            {
                c.gameObject.SetActive(true);
                _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126);
            }
            _tittleIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, 126);
        }
        else
        {
            _audioManager.PlayOneShot(_sfxEvents.UIError);
        }
    }

}
