using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using FMODUnity;
using System.Linq;

public class JournalUIManager : MonoBehaviour
{
    [SerializeField] RawImage[] _images;

    [SerializeField] InteractedSteleData _stelesData;

    FlowStateMachine _flowStateMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _flowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _flowStateMachine.FlowStateMachineEventBus.OnSteleInteraction += SteleInteracted;
    }

    void OnEnable()
    {
        for(int i=0;i<_stelesData.Steles.Count;i++)
        {
            _images[i].texture = _stelesData.Steles[i].VisuelJournal;
            //RawImage image = _images[i].GetComponent<RawImage>();
            //image.texture = _stelesData.Steles[i].VisuelStele;
            //_images[i].SetActive(true);
        }
    }

    private void SteleInteracted(FlowStateMachineEventContext obj)
    {
        RawImage image = _images[_stelesData.Steles.Count()];
        if (!obj.Ministele)
        {
            if (_stelesData.Steles.Count == 0)
            {
                image.texture = obj.VisuelJournal;
                //_images[_stelesData.Steles.Count()].SetActive(true);
                _stelesData.Steles.Add(new()
                {
                    VisuelStele = obj.VisuelStele,
                    VisuelJournal = obj.VisuelJournal,
                    Ministele = false,
                    SteleIndex = obj.SteleIndex,
                    InstanceVoiceStele = obj.InstanceVoiceStele,
                });
                return;
            }
            foreach (var stele in _stelesData.Steles)
            {
                if (stele.SteleIndex == obj.SteleIndex)
                {
                    return;
                }
            }
            image.texture = obj.VisuelJournal;
            //_images[_stelesData.Steles.Count()].SetActive(true);
            _stelesData.Steles.Add(new()
            {
                VisuelStele = obj.VisuelStele,
                VisuelJournal = obj.VisuelJournal,
                Ministele = false,
                SteleIndex = obj.SteleIndex,
                InstanceVoiceStele = obj.InstanceVoiceStele,
            });
        }
    }

    public void ReplayStele(int buttonIndex)
    {
        _flowStateMachine.FlowStateMachineEventBus.ActivateUI(new() { UIElements = new[] { "JournalUIPanel" } });
        _flowStateMachine.ChangeState(_flowStateMachine.FlowStates.FlowStateEnterDialog);
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(FMODEvents.Instance.SteleVoice[buttonIndex]);
        _flowStateMachine.FlowStateMachineEventBus.SteleInteraction(new()
        {
            VisuelStele = _stelesData.Steles[buttonIndex].VisuelStele,
            InstanceVoiceStele = instance,
            Ministele = _stelesData.Steles[buttonIndex].Ministele,
            TutorialEnemy = null,
            SteleIndex = _stelesData.Steles[buttonIndex].SteleIndex,
            Replay = true
        });
    }
}
