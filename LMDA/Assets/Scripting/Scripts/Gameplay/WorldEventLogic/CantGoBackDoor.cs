using Ink.Parsed;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
public class CantGoBackDoor: MonoBehaviour
{

    
    [SerializeField] Animator _controller;
    [SerializeField] TutorialState _mainStatus;

    int theRightNumber = 4;

    void OnEnable()
    {
        
        if (!_mainStatus.InvisibleWall)
        {
            OpenDoor();
        }
    }

    
    public void OpenDoor()
    {
        _controller.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        _controller.SetTrigger("Close");
    }
    
}
