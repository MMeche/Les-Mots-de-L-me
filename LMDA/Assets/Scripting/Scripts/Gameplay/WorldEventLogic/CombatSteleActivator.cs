using Ink.Parsed;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using System.Collections.Generic;

public class CombatSteleActivator : MonoBehaviour
{
    [SerializeField] List<Animator> _animationControllers;
    [SerializeField] TutorialState _mainStatus;
    bool _interacted;

    FlowStateMachine _fsm;

    private void OnEnable()
    {
        if (_mainStatus.TutoSteleActivated) StayUpMiniSteles();
    }
    void Start()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.OnSteleInteraction += SteleInteracted;
        _fsm.FlowStateMachineEventBus.OnSteleExit += ActivateMiniSteles;
    }


    private void SteleInteracted(FlowStateMachineEventContext ctx)
    {
        _interacted = true;
    }

    private void ActivateMiniSteles(FlowStateMachineEventContext ctx)
    {
        if (_interacted && _mainStatus.TutoSteleActivated ==false)
        {
            _mainStatus.TutoSteleActivated = true;
            foreach(var controller in _animationControllers)
            {
                controller.Play("Up");
                controller.GetComponent<BoxCollider>().enabled =true;
            }
        }
    }

    private void ActivateMiniSteles()
    {
            foreach (var controller in _animationControllers)
            {
                controller.Play("Up");
            controller.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void StayUpMiniSteles()
    {
        foreach (var controller in _animationControllers)
        {
            controller.transform.position += new Vector3(0, 4f, 0);
            controller.GetComponent<BoxCollider>().enabled = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
