using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class JournalTutoManager : MonoBehaviour
{
    List<GameObject> _pages;
    int _currentPageIndex = 0;
    FlowStateMachine _fsm;
    
    private void OnEnable()
    {
        _pages = new();
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        for(int i = 0;i<transform.childCount-1;i++)
        {
            _pages.Add(transform.GetChild(i).gameObject);
        }
        
        
    }

    public void OnContinue()
    {
        _pages[_currentPageIndex].SetActive(false);
        _currentPageIndex++;
        if (_currentPageIndex == _pages.Count)
        {
            gameObject.SetActive(false);
            _fsm.FlowStateMachineEventBus.SubscribeAction(new());
            _fsm.FlowStateMachineEventBus.OnJournalTutorialFinish(new());
            return;
        }
        _pages[_currentPageIndex].SetActive(true);
    }


    
}
