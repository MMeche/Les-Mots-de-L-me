using System;
using System.Linq;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Profiling.RawFrameDataView;
#endif

public class SteleInteractable : MonoBehaviour , IInteractable
{
    FlowStateMachine _fsm;
    [SerializeField] bool Ministele=false;
    [SerializeField] GameObject _tutorialCombat;
    [SerializeField] Texture2D _visuelStele;
    [SerializeField] Texture2D _visuelJournal;
    [SerializeField] int SteleIndx;
    [SerializeField] Transform _saveSpawnPoint;
    [SerializeField] GameObject _flamesParticle;
    bool _interacted;

    Outline _outline;
    void OnEnable()
    {
        _fsm = ServiceProvider.Instance.ServiceLocator.Get<FlowStateMachine>();
        _fsm.FlowStateMachineEventBus.OnSteleExit += EndInteraction;
        _outline = GetComponent<Outline>();
    }

    public void ShowOutline()
    {
        _outline.enabled = true;
    }

    public void HideOutline()
    {
        _outline.enabled = false;
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")) ShowOutline();
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) HideOutline();
    //}

    public void Interact()
    {
        _interacted = true;
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(FMODEvents.Instance.SteleVoice[SteleIndx]);
        if (!Ministele)
        {
            List<PlayerStatus> statuses = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
            statuses[0].Position = _saveSpawnPoint.position;
            statuses[1].Position = _saveSpawnPoint.position + new Vector3(0.5f, 0, 0.5f);
            statuses[2].Position = _saveSpawnPoint.position + new Vector3(-0.5f, 0, 0.5f);

            SaveSystem.SaveGameState("/AutoSave" + gameObject.name + DateTime.Now.Hour + DateTime.Now.Minute + ".json");

            if (_flamesParticle != null)
            {
                _flamesParticle.SetActive(true);
            }
        }
        _fsm.ChangeState(_fsm.FlowStates.FlowStateEnterDialog);
        if(!Ministele) _fsm.FlowStateMachineEventBus.SteleInteraction(new()
        {
            VisuelStele = _visuelStele,
            VisuelJournal = _visuelJournal,
            InstanceVoiceStele = instance,
            Ministele = Ministele,
            TutorialEnemy = _tutorialCombat,
            SteleIndex = SteleIndx,
            Replay = false
        });
        else _fsm.FlowStateMachineEventBus.SteleInteraction(new()
        {
            VisuelStele = _visuelStele,
            InstanceVoiceStele = instance,
            Ministele = Ministele,
            TutorialEnemy = _tutorialCombat,
            SteleIndex = SteleIndx,
            Replay = false
        });
    }

    private void EndInteraction(FlowStateMachineEventContext ctx)
    {
        //_interacted = false;
    }

}
