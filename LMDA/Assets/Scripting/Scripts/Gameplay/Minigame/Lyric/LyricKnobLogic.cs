using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class LyricKnobLogic : MonoBehaviour
{
    public KnobType Type = KnobType.classic;
    [SerializeField] Material _hoveredMaterial;
    [SerializeField] Material _unhoveredMaterial;
    [SerializeField] List<TMP_Text> _orbTexts;
    [SerializeField] Color _hoveredColor;
    [SerializeField] Color _unhoveredColor;
    [SerializeField] ParticleSystem _particleSystem; 

    bool _isHovered=false;
    bool _hasFinished = false;
    SplineAnimate _animation;
    MeshRenderer _renderer;
    float _timeEnteringStart = 0;
    float _timeEnteringEnd = 0;
    float _elapsedTimeHoveringKnob = 0;
    BattleManager _battleManager;

    private void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _hasFinished = false;
        _isHovered = false;

        _timeEnteringStart = 0;
        _timeEnteringEnd = 0;
        _elapsedTimeHoveringKnob = 0;

        _animation = GetComponent<SplineAnimate>();
        _renderer = GetComponent<MeshRenderer>();

        ChangeColors(false);
        _battleManager.BattleManagerEventBus.KnobEnabled(new() { });
        //SendMessageUpwards("OnKnobEnabled");
        _animation.Restart(true); // facultatif mais utile
        _animation.Pause();
        StartCoroutine(nameof(CountDown));
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(4);
        if (!_animation.IsPlaying)
        {
            _battleManager.BattleManagerEventBus.CursorTriggerEnter(new() { Type = Type });
            _battleManager.BattleManagerEventBus.CursorTriggerExit(new() { Type = Type });
        }
    }



    void Update()
    {
        //Refactoring??
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Knob"))) 
        {
            if (hit.collider.gameObject == gameObject && !_isHovered)
            {
                _isHovered = true;
                _timeEnteringStart = Time.time;
                _battleManager.BattleManagerEventBus.CursorTriggerEnter(new() { Type = Type });
                //SendMessageUpwards("CursorTriggerEnter", Type);
                ChangeColors(_isHovered);
                _renderer.material = _hoveredMaterial;
            }
            
        }
        else if (_isHovered)
        {
            _isHovered = false;
            _timeEnteringEnd = Time.time;
            _elapsedTimeHoveringKnob += _timeEnteringEnd - _timeEnteringStart;
            _battleManager.BattleManagerEventBus.CursorTriggerExit(new() { Type = Type });
            //SendMessageUpwards("CursorTriggerExit", Type);
            _renderer.material = _unhoveredMaterial;
            ChangeColors(_isHovered);
        }
        if (!_animation.IsPlaying && _animation.ElapsedTime > 0.1f && !_hasFinished)
        {
            if (_isHovered)
            {
                _timeEnteringEnd = Time.time;
                _elapsedTimeHoveringKnob += _timeEnteringEnd - _timeEnteringStart;
                _renderer.material = _unhoveredMaterial;
                ChangeColors(false);
            }

            SendDamage();
        }
            
    }

    public void SendDamage()
    {
        KnobMessageEndContext messageContext = new KnobMessageEndContext();
        messageContext.Type = Type;
        messageContext.ElapsedTimeHovering = _elapsedTimeHoveringKnob;
        _hasFinished = true;
        _battleManager.BattleManagerEventBus.KnobEndReached(new() { Type = Type, ElapsedTimeHoveringKnob = _elapsedTimeHoveringKnob });
        //SendMessageUpwards("KnobEndReached", messageContext);
        
    }

    private void ChangeColors(bool hovered)
    {
        if (hovered)
        {
            foreach (var text in _orbTexts)
            {
                text.color = _hoveredColor;
            }
            _particleSystem.startColor = _hoveredColor;
        }
        else
        {
            foreach (var text in _orbTexts)
            {
                text.color = _unhoveredColor;
            }
            _particleSystem.startColor = _unhoveredColor;
        }
       
    }



}



public enum KnobType
{
    classic,
    defense,
    heal,
    whatever
}

public struct KnobMessageEndContext
{
    public KnobType Type;
    public float ElapsedTimeHovering;
}
