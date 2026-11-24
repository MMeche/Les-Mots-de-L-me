using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteleScrollbar : ScrollRect
{

    bool _hasBeenDraged=false;
    float _speed;
    bool _ministele = false;
    [SerializeField] float _clipDuration;
    [SerializeField] Vector2 _horizontalDimensions;
    [SerializeField] GameObject _launchTutorialUIElement;
    [SerializeField] GameObject _closeButton;


    new void OnEnable()
    {
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,_horizontalDimensions.x);
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _horizontalDimensions.y);
        //Back when ministele were smaller sized
        //if (!_ministele)
        //{
        //    horizontalNormalizedPosition = 0f;
        //    _speed = 0.6f / _clipDuration;
        //    StartCoroutine(nameof(AutomaticPan));
        //}
        //else
        //{
        //    horizontalNormalizedPosition = 0.5f;
        //    RawImage image = content.GetComponent<RawImage>();
        //    image.SetNativeSize();
        //    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.texture.height * 1000 / image.texture.width);
        //    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1000);
        //    _launchTutorialUIElement.SetActive(true);

        //}
        if (_ministele)
        {
            _launchTutorialUIElement.SetActive(true);
        }
        else _launchTutorialUIElement.SetActive(false);
        _closeButton.SetActive(false);
        _hasBeenDraged = false;
        RawImage image = content.GetComponent<RawImage>();
        content.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
        horizontalNormalizedPosition = 0f;
        image.SetNativeSize();
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.texture.width*_horizontalDimensions.y/ image.texture.height);
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _horizontalDimensions.y);
        _speed = 1f / _clipDuration;
        StartCoroutine(nameof(AutomaticPan));
    }

    IEnumerator AutomaticPan()
    {
        yield return null; //After first update 
        while (!_hasBeenDraged && horizontalNormalizedPosition < 0.99f && _clipDuration>0)
        {
            horizontalNormalizedPosition += Time.unscaledDeltaTime  * _speed;
            yield return null;
        }
        if (!_hasBeenDraged)
        {
            _closeButton.SetActive(true);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        _hasBeenDraged = true;
        _closeButton.SetActive(true);
        base.OnDrag(eventData);
    }

    public override void OnScroll(PointerEventData data)
    {
        _hasBeenDraged = true;
        _closeButton.SetActive(true);
        base.OnScroll(data);
    }

    public void SetMinistele(bool setter)
    {
        _ministele = setter;
    }

    public void SetEvtDuration(float setter)
    {
        _clipDuration = setter;
    }

}
