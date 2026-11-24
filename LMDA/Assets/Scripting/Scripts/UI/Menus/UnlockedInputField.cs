using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockedInputField : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputField;

    private void Start()
    {
        _inputField.onSelect.AddListener(CursorParameter);
        _inputField.onEndEdit.AddListener(CursorParameter);
    }


    private void CursorParameter(string text)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
}
