using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitCoverUpListener : MonoBehaviour
{
    [SerializeField] List<Image> _coverUps;
    UIManager _uiManager;
    private void OnEnable()
    {
        _uiManager = ServiceProvider.Instance.ServiceLocator.Get<UIManager>();
        _uiManager.UIEventBus.OnUISetup += UpdateCoverUps;
    }


    private void UpdateCoverUps(UIEventContext ctx)
    {
        foreach(var cover in _coverUps)
        {
            cover.enabled = true;
        }
        _coverUps[ctx.CharacteIndex].enabled = false;

    }

    private void OnDisable()
    {
        _uiManager.UIEventBus.OnUISetup -= UpdateCoverUps;
    }
}
