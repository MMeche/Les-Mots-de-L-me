using Ink.Parsed;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
public class CombatDoorManager : MonoBehaviour
{

    FlowStateMachine _fsm;
    [SerializeField] List<Animator> _controllers;
    [SerializeField] TutorialState _mainStatus;

    int theRightNumber = 4;

    void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        
        if (_mainStatus.DoorOpened)
        {
            if (_mainStatus.DoorPartsOpened >= 4 && !_mainStatus.TutoStatDone)
            {
                foreach (var c in _controllers)
                {
                    c.Play("Down");
                    _mainStatus.DoorPartsOpened++;
                    //c.transform.position += new Vector3(0, -2f, 0);
                }
                GetComponentInChildren<BoxCollider>().enabled = false;
                _fsm.FlowStateMachineEventBus.OnTutorialFinish(new());
                List<PlayerStatus> statuses = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
                foreach(var p in statuses)
                {
                    p.AvailableSkillPoints += 2;
                }
                
            }
            else
            {
                transform.position += new Vector3(0, -10f, 0);
                GetComponentInChildren<BoxCollider>().enabled = false;
            }
        }
        //else
        //{
        //    for (int i = 0; i < _mainStatus.DoorPartsOpened; i++)
        //    {
        //        ActivateDoor(i);
        //        //_controllers[i].transform.position +=  new Vector3(0, -2f, 0);
        //    }
        //}
    }

    void Start()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.CombatMinisteleActivated += ActivateDoor;
    }

    
    void ActivateDoor(FlowStateMachineEventContext ctx)
    {
        if (_mainStatus.DoorPartsOpened < 4)
        {
            _mainStatus.DoorPartsOpened++;
        }
        if (_mainStatus.DoorPartsOpened >= 4)
        {
            _mainStatus.DoorOpened = true;
            GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }

    void ActivateDoor(int index)
    {
        _controllers[index].Play("Down");
    }
}
