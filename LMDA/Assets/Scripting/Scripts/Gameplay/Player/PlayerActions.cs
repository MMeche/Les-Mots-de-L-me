using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
    [SerializeField] Transform _camTransform;
    
    [Header("Movement parameters")]
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _sprintSpeedModifier = 5;
    [SerializeField] float _rotationSpeed = 10;
    [SerializeField] float _jumpForce = 1;
    private bool _airborned = false;

    [Header("Interaction parameters")]
    [SerializeField] InteractionDetectionManager _idm;
    
    PauseManager _pauseManager;

    FlowStateMachine _fsm;

    PlayerStatManager _statManager;
    Rigidbody _rb;
    PlayerInputs _playerInputs;
    Vector2 _inputDirection;
    bool _menuIsOpen =false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _statManager = ServiceProvider.Instance.ServiceLocator.Get<PlayerStatManager>();
    }

    private void OnEnable()
    {
        ServiceProvider.Instance.ServiceLocator.Register(this);

        _pauseManager = ServiceProvider.Instance.ServiceLocator.Get<PauseManager>();

        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();

        _fsm.FlowStateMachineEventBus.EnterOverworld += SetPlayerInputAsset;
        _fsm.FlowStateMachineEventBus.CameraBackToFollowingView += ActivateStatTutorial;
        _fsm.FlowStateMachineEventBus.OnUIActivated += PlayerMenuActivated;
        _fsm.FlowStateMachineEventBus.OnActionSubscribed += Subscribe;
        _fsm.FlowStateMachineEventBus.OnActionUnsubscribed += Unsubscribe;
        StartCoroutine(WaitForManager());
    }



    IEnumerator WaitForManager()
    {
        while (_playerInputs == null)
        {
            yield return null;
        }

        _playerInputs.Player.Enable();

        _playerInputs.Player.Move.performed += UpdateDirection;
        _playerInputs.Player.Move.canceled += StopMovement;
        _playerInputs.Player.Sprint.performed += StartSprinting;
        _playerInputs.Player.Sprint.canceled += StopSprinting;
        _playerInputs.Player.Jump.started += Jump;
        
        _playerInputs.Player.Interact.started += Interact;
        _playerInputs.Player.Menu.started += PauseMenu;
        _playerInputs.Player.Debug.started += DebugMenu;
        _playerInputs.Player.OpenCharacterMenu.started += OpenCharacterMenu;
        _playerInputs.UI.CloseCharacterMenu.started += CloseCharacterMenu;
        _playerInputs.UI.ChangeSheetDown.started += ChangeCharacterSheet;
        _playerInputs.UI.ChangeSheetUp.started += ChangeCharacterSheet;
        _playerInputs.UI.Cancel.started += CancelMenu;
    }


    private void SetPlayerInputAsset(FlowStateMachineEventContext obj)
    {
        _playerInputs = obj.PlayerInputs;
        _fsm.FlowStateMachineEventBus.EnterOverworld -= SetPlayerInputAsset;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        CheckIfFalling();
        if (Input.GetKeyDown(KeyCode.KeypadPlus) && _menuIsOpen)
        {
            DebugAddExp(new());
        }
    }

    void DebugAddExp(InputAction.CallbackContext ctx)
    {
        _statManager.PlayerEventBus.AddExp(new() { ExpToAdd = 20 });
    }

    private void StopMovement(InputAction.CallbackContext ctx)
    { 
        _inputDirection = Vector2.zero;
        BroadcastMessage("MoveStopped");
    }

    private void UpdateDirection(InputAction.CallbackContext ctx) 
    { 
        _inputDirection = ctx.ReadValue<Vector2>();
        if (_inputDirection.magnitude != 0) BroadcastMessage("MoveStarted");
    }

    private void MovePlayer()
    {
        if (_inputDirection != Vector2.zero)
        {
            Vector3 camForward = _camTransform.forward;
            Vector3 camRight = _camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();


            Vector3 direction = camForward * _inputDirection.y + camRight * _inputDirection.x;
            direction.Normalize();
            Vector3 move = direction * _moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + move);

            RotatePlayer(direction);
        }
    }

    private void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        IInteractable target = _idm.GetInteractionTarget();
        target?.Interact();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!_airborned)
        {
            StartCoroutine(BoostUp());
            BroadcastMessage("BeginJump");
            
        }
        
    }

    IEnumerator BoostUp()
    {
        _airborned = true;
        yield return new WaitForSeconds(0.25f);
        _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
    }

    private void CheckIfFalling()
    {
        if((!Physics.CheckSphere(transform.position + (new Vector3(0, -1, 0)), 0.5f, 1 << 8)|| _airborned )&& _rb.linearVelocity.y <= 0.05f)
        {
            _airborned = true;
            BroadcastMessage("Falling");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // 8 : Ground
        {
            _airborned = false;
            BroadcastMessage("EndJump");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if(collision.gameObject.layer == 8)
        {
            if(!Physics.CheckSphere(transform.position + (new Vector3(0, -1, 0)), 0.5f, 1<<8))
            {
                Debug.Log("LE SOL EST PARTI");
            }
        }
    }

    private void StartSprinting(InputAction.CallbackContext ctx)
    {
        _moveSpeed += _sprintSpeedModifier;
        BroadcastMessage("BeginSprint");
    }

    private void StopSprinting(InputAction.CallbackContext ctx)
    {
        _moveSpeed -= _sprintSpeedModifier;
        BroadcastMessage("EndSprint");
    }

    private void PlayerMenuActivated(FlowStateMachineEventContext ctx)
    {
        if (ctx.UIElements.Contains("JournalUIPanel") || ctx.UIElements.Contains("CharUIPanel")) _menuIsOpen = !_menuIsOpen;
    }

    private void ChangeCharacterSheet(InputAction.CallbackContext ctx)
    {
        _fsm.FlowStateMachineEventBus.UIChangeSheet(new() { });
    }

    private void OpenCharacterMenu(InputAction.CallbackContext ctx)
    {
        if (!_pauseManager.IsPaused)
        {
            
            _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "CharUIPanel" } });
            _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
        }    
    }
    private void CloseCharacterMenu(InputAction.CallbackContext ctx)
    {
        if (_menuIsOpen)
        {
            _fsm.FlowStateMachineEventBus.UIChangeSheet(new() { ReturnToRoot = true });
            _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "CharUIPanel" } });
            _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterOverWorld);
        }
        
    }

    private void ActivateStatTutorial(FlowStateMachineEventContext ctx)
    {
        
        _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "CharUIPanel", "StatTutorial" } });
       // _fsm.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "StatTutorial" } });
        _fsm.FlowStateMachineEventBus.CameraBackToFollowingView -= ActivateStatTutorial;
        _fsm.ChangeState(_fsm.FlowStates.FlowStatePause);
    }

    private void PauseMenu(InputAction.CallbackContext ctx)
    {
        //if (PauseManager.IsPaused) PauseManager.instance.Cancel();
        //else
        if (!_pauseManager.IsPaused && _fsm.GetCurrentState()!=_fsm.FlowStates.FlowStateEnterDialog &&! _menuIsOpen) _pauseManager.PauseGame();
    }

    private void DebugMenu(InputAction.CallbackContext ctx)
    {
        if (!_pauseManager.IsPaused && _fsm.GetCurrentState() != _fsm.FlowStates.FlowStateEnterDialog && !_menuIsOpen) _pauseManager.DebugPause();
    }

    private void CancelMenu(InputAction.CallbackContext ctx)
    {
        _pauseManager.Cancel();
        if (_menuIsOpen)
        {
            CloseCharacterMenu(new());
        }
        //else
    }

    public PlayerInputs GetPlayerInputs()
    {
        return _playerInputs;
    }
    private void Subscribe(FlowStateMachineEventContext obj)
    {
        _playerInputs.UI.Cancel.started += CancelMenu;
    }

    private void Unsubscribe(FlowStateMachineEventContext obj)
    {

        _playerInputs.UI.Cancel.started -= CancelMenu;
    }

    void OnDisable()
    {
        if (_playerInputs != null)
        {
            _playerInputs.Player.Move.performed -= UpdateDirection;
            _playerInputs.Player.Move.canceled -= StopMovement;
            _playerInputs.Player.Sprint.performed -= StartSprinting;
            _playerInputs.Player.Sprint.canceled -= StopSprinting;
            _playerInputs.Player.Jump.started -= Jump;

            _playerInputs.Player.Interact.started -= Interact;
            _playerInputs.Player.Menu.started -= PauseMenu;
            _playerInputs.Player.OpenCharacterMenu.started -= OpenCharacterMenu;
            _playerInputs.UI.CloseCharacterMenu.started -= CloseCharacterMenu;
            _playerInputs.UI.ChangeSheetDown.started -= ChangeCharacterSheet;
            _playerInputs.UI.ChangeSheetUp.started -= ChangeCharacterSheet;
            _playerInputs.UI.Cancel.started -= CancelMenu;

            _playerInputs.Player.Disable();

            ServiceProvider.Instance.ServiceLocator.Unregister<PlayerActions>();
        }
    }
}
