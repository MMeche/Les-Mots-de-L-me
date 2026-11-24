using System.IO;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.EventSystems;
using NUnit.Framework.Interfaces;

public class LoadSectionManager : MonoBehaviour
{
    [SerializeField] GameObject _saveItem;
    [SerializeField] Transform _content;

    [SerializeField] Button _delete;
    [SerializeField] Button _loadLaunch;

    [SerializeField] Button _continue;
    [SerializeField] Button _load;

    string[] _allSaves;

    string _saveFile;
    int _saveItemIndex;

    UIManager _uiManager;
    private void Start()
    {

        _uiManager = ServiceProvider.Instance.ServiceLocator.TryGet<UIManager>();

        _uiManager.UIEventBus.OnSaveItemClicked += PreloadSaveData;
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        _allSaves = files;
        
        foreach(var file in files)
        {
            SaveItem item = GameObject.Instantiate(_saveItem, _content).GetComponent<SaveItem>();
            item.SetSaveTitle(file);
            item.SetDateTxt(File.GetCreationTime(file));
            
            
        }
       
        SortOnDate();
        //EventSystem.current.SetSelectedGameObject(_content.GetChild(0).GetChild(0).gameObject);

    }

    private void OnEnable()
    {
        if (_content.childCount != 0)
        {
            //EventSystem.current.SetSelectedGameObject(transform.GetChild(3).GetChild(0).GetChild(0).gameObject);
        }
    }

    private void SortOnDate()
    {
        List<Transform> itemList = new();
        foreach (Transform child in _content.transform)
        {
            itemList.Add(child);
        }
        itemList.Sort((a, b) => b.GetComponent<SaveItem>().GetDate().CompareTo(a.GetComponent<SaveItem>().GetDate()));
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].SetSiblingIndex(i);
        }
    }

    public void RefreshList()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        if (files.Length > _content.childCount)
        {
            
            float contentHeight = _content.GetComponent<RectTransform>().rect.height;
            foreach(var file in files)
            {
                if (!_allSaves.Contains(file))
                {
                    SaveItem item = GameObject.Instantiate(_saveItem, _content).GetComponent<SaveItem>();
                    item.SetSaveTitle(file);
                    item.SetDateTxt(File.GetCreationTime(file));
                    contentHeight += item.GetComponent<RectTransform>().rect.height + _content.GetComponent<VerticalLayoutGroup>().spacing;
                }
            }
            _content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,contentHeight);
            _allSaves = files;
            SortOnDate();
        }
    }

    private void PreloadSaveData(UIEventContext ctx)
    {
        _saveFile = _content.GetChild(ctx.SaveItemIndex).GetComponent<SaveItem>().GetSaveTitle();
        _saveItemIndex = ctx.SaveItemIndex;
        _delete.interactable = true;
        _loadLaunch.interactable = true;
    }

    public void DeleteSave()
    {
        File.Delete(_saveFile);
        GameObject.Destroy(_content.GetChild(_saveItemIndex).gameObject);
        _delete.interactable = false;
        _loadLaunch.interactable = false;
    }

    public string GetSaveFile()
    {
        return _saveFile;
    }
    private void OnDisable()
    {
        if (_content.childCount==0 )
        {
            if(_load != null) _load.gameObject.SetActive(false);
            if(_continue != null) _continue.gameObject.SetActive(false); 
        }
    }
    

}
