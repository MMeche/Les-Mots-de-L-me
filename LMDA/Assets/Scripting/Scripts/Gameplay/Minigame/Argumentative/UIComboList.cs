using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIComboList : MonoBehaviour
{

    [SerializeField] GameObject _comboInfoCard;
    BattleManager _battleManager;
    List<ArgComboData> _availableCombos;

    void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _availableCombos = new();
        _availableCombos = _battleManager.GetActivePlayerStatus().ComboList;
        Transform content = transform.GetChild(0);
        foreach(var combo in _availableCombos)
        {
            ComboInfo newCard = Instantiate(_comboInfoCard, content).GetComponent<ComboInfo>();
            newCard.LoadData(combo);
        }
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _availableCombos.Count * (100 + content.GetComponent<VerticalLayoutGroup>().spacing));
    }

    public void Display()
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        Transform content = transform.GetChild(0);
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
