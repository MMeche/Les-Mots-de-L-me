using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor.Rendering;
#endif

public class ArgWordCardLogic : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] TMP_Text _inputField;
    [SerializeField] Image _imageField;
    [SerializeField] Sprite _offensiveSprite;
    [SerializeField] Sprite _defensiveSprite;
    [SerializeField] Sprite _neutralSprite;
    [SerializeField] GameObject _border;

    [SerializeField] TMP_Text _ATKText;
    [SerializeField] TMP_Text _DEFText;

    [Header("Offensive Colors")]
    [SerializeField] ColorBlock _colorBlockOffensive;

    [Header("Defensive Colors")]
    [SerializeField] ColorBlock _colorBlockDefensive;

    [Header("Neutral Colors")]
    [SerializeField] ColorBlock _colorBlockNeutral;

    ArgCardType _type=ArgCardType.neutral;

    BattleUIManager _buiManager;
    public bool _onSelectionGrid;
    
    private void Start()
    {
        _buiManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _ATKText.transform.parent.gameObject.SetActive(false);
    }

    public TMP_Text GetInputField()
    {
        return _inputField;
    }

    public void SetInputFieldText(string input)
    {
        _inputField.text = input;
    }

    public ArgCardType GetArgCardType()
    {
        return _type;
    }

    public void SetArgCardType(ArgCardType type)
    {
        _type = type;
        switch (_type)
        {
            case ArgCardType.offensive:
                _imageField.sprite = _offensiveSprite;
                GetComponent<Button>().colors = _colorBlockOffensive;
                _border = transform.GetChild(2).gameObject;
                SetText(3, 0);

                break;
            case ArgCardType.defensive:
                _imageField.sprite = _defensiveSprite;
                GetComponent<Button>().colors = _colorBlockDefensive;
                _border = transform.GetChild(2).gameObject;
                SetText(0, 3);
                break;
            case ArgCardType.neutral:
                _imageField.sprite = _neutralSprite;
                GetComponent<Button>().colors = _colorBlockNeutral;
                if (_onSelectionGrid)
                {
                    RectTransform borderRect = _border.GetComponent<RectTransform>();
                    borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
                    borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
                }
                SetText(1, 1);
                break;
            default:
                _imageField.sprite = _neutralSprite;
                break;
        }
        SetNativeSize();
    }

    public void SetNativeSize()
    {
        _imageField.SetNativeSize();
        _imageField.transform.rotation = Quaternion.identity;
    }


    private void SetText(int atkInt,int defInt)
    {
        _ATKText.text = "+ " + atkInt.ToString() + " ATK";
        _DEFText.text = "+ " + defInt.ToString() + " DEF";
    }
    

    
    public void OnClick()
    {
        _onSelectionGrid = !_onSelectionGrid;
        RectTransform borderRect = _border.GetComponent<RectTransform>();
        if (!_onSelectionGrid)
        {
            BorderShorten();
            if(_type == ArgCardType.offensive)
            {
                _imageField.GetComponent<RectTransform>().rotation = Quaternion.identity;
            }
        }
        if(_onSelectionGrid && _type == ArgCardType.offensive)
        {
            _imageField.GetComponent<RectTransform>().rotation *= Quaternion.Euler(0, 0, -72);
        }
        _buiManager.BattleUIEventBus.ArgWordCardClicked(new() { Card = transform });

        if (_type == ArgCardType.neutral)
        {
            _onSelectionGrid = !_onSelectionGrid;
            
        }



        //SendMessageUpwards("ArgCardClicked", transform);
    }

    public void BorderWiden()
    {
        
        RectTransform borderRect = _border.GetComponent<RectTransform>();
        borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
    }

    public void BorderShorten()
    {
        RectTransform borderRect = _border.GetComponent<RectTransform>();
        borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 105);
        borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 155);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            _border.SetActive(true);
            if(_type != ArgCardType.neutral)
            {
                _imageField.color = new Color(1, 1, 1);
            }
            else _imageField.color = new Color(1, 0.6901961f, 0.8392157f);
            if (!_onSelectionGrid)
            {
                _ATKText.transform.parent.gameObject.SetActive(true);
            }
        }
        
        //#6F7497
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            _border.SetActive(false);
            _imageField.color = new Color(0.4352941f, 0.454902f, 0.5921569f);
            _ATKText.transform.parent.gameObject.SetActive(false);
        }
            
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            _border.SetActive(false);
            _imageField.color = new Color(0.4352941f, 0.454902f, 0.5921569f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}

public enum ArgCardType
{
    offensive,
    defensive,
    neutral
}
