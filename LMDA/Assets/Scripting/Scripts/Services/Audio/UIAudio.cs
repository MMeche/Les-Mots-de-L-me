using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIAudio : MonoBehaviour
{
    Button _button;
    AudioManager _audioManager;
    FMODEvents _sfxEvents;
    Animator _animator;

    private void Start()
    {
        _audioManager = ServiceProvider.Instance.ServiceLocator.Get<AudioManager>();
        _sfxEvents = ServiceProvider.Instance.ServiceLocator.Get<FMODEvents>();
        TryGetComponent<Animator>(out _animator);
        _button = GetComponent<Button>();
    }
    public void OnPointerEnter()
    {
        if (_button.interactable)
        {
            _audioManager.PlayOneShotAttached(_sfxEvents.UIHover, gameObject);
            if (_animator != null)
            {
                BoolFalse();
                _animator.SetBool("Highlighted", true);
            }
                
        }
    }

    public void OnClick()
    {
        if (_button.interactable) 
        {
            _audioManager.PlayOneShotAttached(_sfxEvents.UIClick, gameObject);
            if(_animator != null)
            {
                BoolFalse();
                _animator.SetBool("Pressed", true);
            }
            
        }
        else _audioManager.PlayOneShotAttached(_sfxEvents.UICancel, gameObject);
    }

    public void OnCancel()
    {
        _audioManager.PlayOneShotAttached(_sfxEvents.UICancel, gameObject);
    }

    private void BoolFalse()
    {
        _animator.SetBool("Normal", false);
        _animator.SetBool("Highlighted", false);
        _animator.SetBool("Pressed", false);
        _animator.SetBool("Selected", false);

    }
}
