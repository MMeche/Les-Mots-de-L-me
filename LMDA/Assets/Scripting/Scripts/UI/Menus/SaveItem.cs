using System;
using TMPro;
#if UNITY_EDITOR
using Unity.VisualScripting.ReorderableList;
#endif 
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class SaveItem : MonoBehaviour
{
    [SerializeField] TMP_Text _saveTextEntry;
    [SerializeField] TMP_Text _dateTextEntry;

    UIManager _uiManager;
    
    string _saveTitle;
    DateTime _date;

    Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Clicked);
        
    }

    private void Start()
    {
        _uiManager = ServiceProvider.Instance.ServiceLocator.Get<UIManager>();
    }

    public void Clicked()
    {
        _uiManager.UIEventBus.SaveItemClicked(new() { SaveItemIndex = transform.GetSiblingIndex() });
    }

    public void SetSaveTitle(string txt)
    {
        _saveTitle = txt;
        _saveTextEntry.text = Path.GetFileNameWithoutExtension(_saveTitle);
    }

    public string GetSaveTitle()
    {
        return _saveTitle;
    }

    public void SetDateTxt(DateTime t)
    {
        _date = t;
        _dateTextEntry.text = t.Hour + "H" + t.Minute + " le " + t.Day + "/" + t.Month + "/" + t.Year;
    }

    public DateTime GetDate()
    {
        return _date;
    }

}
