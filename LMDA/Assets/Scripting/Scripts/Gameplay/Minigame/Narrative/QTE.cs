using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class QTE : MonoBehaviour
{
    [SerializeField] Image _qteArrow;
    [SerializeField] GameObject _playerFeedbackParticles;
    [SerializeField] GameObject _playerFeedbackBurst;
    [SerializeField] List<Sprite> _arrows;
    [SerializeField]
    [Tooltip("The smaller the number, the slower the animation and vice-versa")]
    float _circleShrinkingSpeed = 1;

    int _keyValue;
    float _damageMultiplier = 0;
    bool _isDefensive;
    PlayerInputs _inputActions;
    QTEManager _manager;
    BattleFlowStateMachine _bsm;
    private void OnEnable()
    {
        _bsm = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();
        _bsm.BattleFlowStateMachineEventBus.OnBattleLose += BattleLose;
    }
    public void SetupQTEInfo(int randomKeyValue, PlayerInputs inputActions, bool isDefensive, QTEManager manager)
    {
        _inputActions = inputActions;
        _isDefensive = isDefensive;
        _keyValue = randomKeyValue;
        _manager = manager;

        switch (_keyValue)
        {
            case 0:
                _qteArrow.sprite = _arrows[0]; //haut
                _inputActions.Narrative.Input0.performed += QTEPressed;
                break;
            case 1:
                _qteArrow.sprite = _arrows[1]; //gauche
                _inputActions.Narrative.Input1.performed += QTEPressed;
                break;
            case 2:
                _qteArrow.sprite = _arrows[2]; //bas
                _inputActions.Narrative.Input2.performed += QTEPressed;
                break;
            case 3:
                _qteArrow.sprite = _arrows[3]; //Droit
                _inputActions.Narrative.Input3.performed += QTEPressed;
                break;
        }
    }

    private void QTEPressed(InputAction.CallbackContext ctx)
    {
        QTEEnd();
    }

    public void QTETimingStamps(float timingMultiplier)
    {
        _damageMultiplier = timingMultiplier;
    }

    public void QTEEnd()
    {
        _inputActions.Narrative.Input0.performed -= QTEPressed;
        _inputActions.Narrative.Input1.performed -= QTEPressed;
        _inputActions.Narrative.Input2.performed -= QTEPressed;
        _inputActions.Narrative.Input3.performed -= QTEPressed;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GetComponent<RectTransform>().position);
        Vector3 _qtePosition = Camera.main.ScreenToWorldPoint(
        new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane + 10f)
        );
        if (_isDefensive)
        {
            if (_damageMultiplier == 0)
            {
                Instantiate(_playerFeedbackBurst, _qtePosition, Camera.main.transform.rotation);
            }
            _manager.DamagePlayer(_damageMultiplier, _keyValue);
        }
        else
        {
            if (_damageMultiplier != 0)
            {
                
                Instantiate(_playerFeedbackParticles,_qtePosition,Quaternion.identity);
            }
            if (_damageMultiplier > 1f)
            {
                
                Instantiate(_playerFeedbackBurst, _qtePosition, Camera.main.transform.rotation);
            }
            _manager.DamageEnemy(_damageMultiplier, _keyValue);
        }

        Destroy(gameObject.transform.parent.gameObject);
    }

    private void BattleLose(BattleFlowStateMachineEventContext ctx)
    {
        Destroy(gameObject);
    }
}
