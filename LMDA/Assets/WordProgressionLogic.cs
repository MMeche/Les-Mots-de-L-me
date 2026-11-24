using TMPro;
using UnityEngine;

public class WordProgressionLogic : MonoBehaviour
{
    [SerializeField] TMP_Text _goalText;

    [SerializeField] TMP_Text _targetText;

    private void OnEnable()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _goalText.rectTransform.rect.width + 50);
        _targetText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,_goalText.rectTransform.rect.width);
        _targetText.text = "";
    }

    private void OnDisable()
    {
        transform.parent.GetComponent<BattleWords>().CancelStack();        
    }

}
