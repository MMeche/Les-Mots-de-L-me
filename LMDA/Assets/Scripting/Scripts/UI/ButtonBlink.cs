using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ButtonBlink : MonoBehaviour
{
    [SerializeField] float _blinkSpeed = 1f;
    [SerializeField] float _blinkDelay = 8f;

    float _timer;

    Image _imageToBlink;

    private void OnEnable()
    {
        _imageToBlink = GetComponent<Image>();
        _timer = 0;
        _imageToBlink.color = Color.white;
    }

    void Update()
    {
        _timer += Time.unscaledDeltaTime;

        if (_timer < _blinkDelay) return;

        float alpha = Mathf.PingPong(Time.unscaledTime * _blinkSpeed, 1f);

        Color c = _imageToBlink.color;
        c.a = alpha;
        _imageToBlink.color = c;
    }
}
