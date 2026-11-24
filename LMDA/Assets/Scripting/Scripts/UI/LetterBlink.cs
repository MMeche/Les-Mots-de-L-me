using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LetterBlink : MonoBehaviour
{
    [SerializeField] float _blinkSpeed = 1f;
    [SerializeField] float _blinkDelay = 8f;

    float _timer;

    TMP_Text _textToBlink;

    private void OnEnable()
    {
        _textToBlink = GetComponent<TMP_Text>();
        _timer = 0;
        _textToBlink.color = Color.white;
    }

    void Update()
    {
        _timer += Time.unscaledDeltaTime;

        if (_timer < _blinkDelay) return;

        float alpha = Mathf.PingPong(Time.unscaledTime * _blinkSpeed, 1f);

        Color c = _textToBlink.color;
        c.a = alpha;
        _textToBlink.color = c;
    }
}
