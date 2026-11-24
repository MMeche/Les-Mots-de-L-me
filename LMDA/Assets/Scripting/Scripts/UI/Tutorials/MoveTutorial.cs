using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTutorial : MonoBehaviour
{
    FlowStateMachine _fsm;

    PlayerInputs _playerInputs;
    [SerializeField] TextMeshPro[] _textComponentsWASD;
    [SerializeField] GameObject _mouseComponent;
    [SerializeField] GameObject _tutoSprint;
    [SerializeField] GameObject _tutoJump;
    int _directionsLearned;
    bool _mouseLearned;
    //[SerializeField] GameObject[] _gameObjects;

    [SerializeField] Transform _currentCam;
    [SerializeField] TutorialState _tutorialState;

    private void OnEnable()
    {
        if (_tutorialState.TutoMoveDone) gameObject.SetActive(false);
        _directionsLearned=0;
    }

    void Start()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.OnIntroFinished += EnableMoveTuto;
        _playerInputs = transform.parent.GetComponent<PlayerActions>().GetPlayerInputs();

        _playerInputs.Player.Move.performed += DirectionLearned;
        _playerInputs.Player.Look.performed += CamControlLearned;
        _playerInputs.Player.Sprint.performed += SprintLearned;
        _playerInputs.Player.Jump.performed += JumpLearned;
    }



    private void EnableMoveTuto(FlowStateMachineEventContext obj)
    {
        foreach (TextMeshPro item in _textComponentsWASD)
        {
            item.transform.parent.gameObject.SetActive(true);
        }
        _mouseComponent.SetActive(true);
    }

    private void FixedUpdate()
    {
        transform.rotation = _currentCam.rotation;
        Debug.Log(_directionsLearned);
        if (_directionsLearned == 6 && _mouseLearned)
        {
            _tutorialState.TutoMoveDone = true;
            gameObject.SetActive(false);
        }
    }


    private void DirectionLearned(InputAction.CallbackContext ctx)
    {
        foreach(var txt in _textComponentsWASD)
        {
            if (ctx.control.displayName == txt.text)
            {
                Destroy(txt.transform.parent.gameObject);
                _directionsLearned++;

                if (_directionsLearned == 4)
                {
                    _playerInputs.Player.Move.performed -= DirectionLearned;
                }
            }
            
        }
    }

    private void CamControlLearned(InputAction.CallbackContext ctx)
    {
        Invoke("DisableMouseTuto", 5f);
    }    
    
    private void JumpLearned(InputAction.CallbackContext obj)
    {
        if (_tutoJump.activeSelf)
        {
            _directionsLearned++;
            _tutoJump.SetActive(false);
        }
    }

    private void SprintLearned(InputAction.CallbackContext obj)
    {
        if (_tutoSprint.gameObject.activeSelf)
        {
            _directionsLearned++;
            _tutoSprint.SetActive(false);
        }
    }

    private void DisableMouseTuto()
    {
        _mouseComponent.SetActive(false);
        _mouseLearned = true;
    }

    private void OnDestroy()
    {
        _playerInputs.Player.Move.performed -= DirectionLearned;
        _playerInputs.Player.Look.performed -= CamControlLearned;
        _playerInputs.Player.Sprint.performed -= SprintLearned;
        _playerInputs.Player.Jump.performed -= JumpLearned;
    }
}
