using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarEventLink : MonoBehaviour
{
    Scrollbar _scrollbar;
    Slider _slider;

    private void OnEnable()
    {
        TryGetComponent<Scrollbar>(out _scrollbar);
        TryGetComponent<Slider>(out _slider);
        if(_scrollbar)_scrollbar.value = 0;
        if (_slider) _slider.value = 0;
    }

    public void OnScrollRectValueChanged(Vector2 scrollRectValue)
    {
        if(_scrollbar)_scrollbar.value = scrollRectValue.x;
        if (_slider) _slider.value = scrollRectValue.x;
    }
}
