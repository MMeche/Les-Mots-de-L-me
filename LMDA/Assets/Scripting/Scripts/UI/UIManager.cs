using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIEventBus UIEventBus;
    public UIEventContext UIEventContext;
    private FlowStateMachine _fsm;

    [SerializeField] private UIContainer _userInterfaceElements;
    [SerializeField] private List<GameObject> _uiObjects = new();

    private Dictionary<string, GameObject> _nameUIObjectPairs = new();

    void OnEnable()
    {
        UIEventBus = new();
        UIEventContext = new();
        ServiceProvider.Instance.ServiceLocator.Register(this);
        BuildDictionary();
    }

    private void Start()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.TryGet<FlowStateMachine>();
        if (_fsm)
        {
            _fsm.FlowStateMachineEventBus.OnUIActivated += ActivateUI;
            _fsm.FlowStateMachineEventBus.OnUIChangeSheet += ChangeUISheet;
        }
        
    }

    private void ActivateUI(FlowStateMachineEventContext ctx)
    {
        if (ctx == null)
        {
            Debug.LogWarning($"WARNING: Context of type {typeof(FlowStateMachineEventContext).FullName} is null");
            return;
        }

        if (ctx.UIElements.Length < 0 || ctx.UIElements == null)
            return;

        foreach (var name in ctx.UIElements)
        {
            if (!TryGetDictionaryPrefabFromName(name, out var uiObject))
            {
                Debug.LogWarning($"WARNING: Can't find KeyValuePair with name {name}");
                return;
            }
            Debug.Log($"{name}");
            uiObject.SetActive(!uiObject.activeSelf);
            //uiObject.name = "Fuck yu";
        }
    }

    private void ChangeUISheet(FlowStateMachineEventContext ctx)
    {
        if (ctx == null)
        {
            Debug.LogWarning($"WARNING: Context of type {typeof(FlowStateMachineEventContext).FullName} is null");
            return;
        }
        if (ctx.ReturnToRoot)
        {
            _uiObjects[7].SetActive(false);
            _uiObjects[4].SetActive(true);
            return;
        }
        if (_uiObjects[4].activeSelf)
        {
            _uiObjects[4].SetActive(false);
            _uiObjects[7].SetActive(true);
            return;
        }
        if (_uiObjects[7].activeSelf)
        {
            _uiObjects[7].SetActive(false);
            _uiObjects[4].SetActive(true);
        }
    }
    

    private bool TryGetDictionaryPrefabFromName(string name, out GameObject uiObject)
    {
        uiObject = null;

        foreach (KeyValuePair<string, GameObject> keyValuePair in _nameUIObjectPairs)
        {
            if (keyValuePair.Key == name)
            {
                uiObject = keyValuePair.Value;
            }
        }

        return uiObject != null;
    }

    private void BuildDictionary()
    {
        foreach (var name in _userInterfaceElements.UserInterfaces)
        {
            foreach (var prefab in _uiObjects)
            {
                if (name.Name == prefab.name)
                {
                    _nameUIObjectPairs.Add(name.Name, prefab);
                }
            }
        }
    }

}
