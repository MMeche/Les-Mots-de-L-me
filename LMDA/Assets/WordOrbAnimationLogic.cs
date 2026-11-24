using TMPro;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class WordOrbAnimationLogic: MonoBehaviour
{
    [SerializeField] bool _positiveSpin = true;
    [SerializeField] float _animationSpeed = 1f;
    [SerializeField] WordsData _words;

    int _wordIndex=0;
    TMP_Text _textComponent;
    Animator _animator;
    

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        
        _textComponent = GetComponentInChildren<TMP_Text>();
    }
    private void Start()
    {
        _textComponent.text = _words.Words[_wordIndex];
        _animator.speed = _animationSpeed;
        if (_positiveSpin)
        {
            
            
            _animator.Play("PositiveSpin");
        }
        else _animator.Play("NegativeSpin");

    }


    private void AnimationEndReached()
    {
        _wordIndex = (_wordIndex+1) % _words.Words.Count;
        _textComponent.text = _words.Words[_wordIndex];
        _animationSpeed = Random.Range(0.2f, 10f);
        _animator.speed = _animationSpeed;
        //SendMessageUpwards("AnimationEnd");
    }
}
