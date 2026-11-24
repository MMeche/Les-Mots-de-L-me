using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboInfo : MonoBehaviour
{
    ArgComboData _comboData;
    [SerializeField] GameObject _comboCard;
    [SerializeField] TMP_Text tmp;

    public void LoadData(ArgComboData data)
    {
        _comboData = data;
        foreach(var card in _comboData.ComboProfile)
        {
            Instantiate(_comboCard, transform.GetChild(0)).GetComponent<ArgWordCardLogic>().SetArgCardType(card);
            tmp.text = "" + _comboData.DamageModifier + "\n" + _comboData.EnemiDamageModifier;
        }
    }
}
