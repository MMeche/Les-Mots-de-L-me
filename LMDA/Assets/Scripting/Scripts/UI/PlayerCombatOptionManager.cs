using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class PlayerCombatOptionManager : MonoBehaviour
{
    [SerializeField] TMP_Text _descriptionField;
    List<Button> _buttons;
    Image _imageCompnt;
    [SerializeField] TutorialState _tutorialStatus;
    [SerializeField] PlayerStatus _playerStatus;

    BattleManager _battleManager;
    BattleUIManager _battleUIManager;
    BattleFlowStateMachine _battleFlowStateMachine;

    void OnEnable()
    {
        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleFlowStateMachine = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();

        _battleFlowStateMachine.BattleFlowStateMachineEventBus.UISetup += RemoveUnusedMinigame;
        //_battleFlowStateMachine.BattleFlowStateMachineEventBus.SelectionCancel += ToggleButtonInteraction;
        _battleUIManager.BattleUIEventBus.OnToggleInteraction += ToggleButtonInteraction;
        //_battleUIManager.BattleUIEventBus.OnSetBaseValue += SetAvailableMinigames;
        _buttons = GetComponentsInChildren<Button>().ToList();
        _imageCompnt = GetComponent<Image>();


    }

    private void RemoveUnusedMinigame(BattleFlowStateMachineEventContext ctx)
    {
        if (ctx.EnemyStatus != null)
        {
            if (!ctx.EnemyStatus.PoeticEnabled)
            {
                _buttons.Find(o => o.name == "Poetic").gameObject.SetActive(false);
                _buttons.Remove(_buttons.Find(o => o.name == "Poetic"));
            }
            if (!ctx.EnemyStatus.LyricEnabled)
            {
                _buttons.Find(o => o.name == "Lyric").gameObject.SetActive(false);
                _buttons.Remove(_buttons.Find(o => o.name == "Lyric"));
            }
            if (!ctx.EnemyStatus.NarrativeEnabled)
            {
                _buttons.Find(o => o.name == "Narrative").gameObject.SetActive(false);
                _buttons.Remove(_buttons.Find(o => o.name == "Narrative"));
            }
            if (!ctx.EnemyStatus.ArgEnabled)
            {
                _buttons.Find(o => o.name == "Argumentative").gameObject.SetActive(false);
                _buttons.Remove(_buttons.Find(o => o.name == "Argumentative"));
            }
            ToggleButtonInteraction(new BattleUIEventContext());
            
            if (_playerStatus != null && Resources.FindObjectsOfTypeAll<TutorialState>()[0].TutoStatDone)
            {
                var top2 = _playerStatus.Stats
                                           .OrderByDescending(kv => kv.Value)
                                           .Take(2)
                                           .ToList();
                var type1 = top2[0].Key;
                var type2 = top2[1].Key;
                foreach (Button button in _buttons)
                {
                    button.gameObject.SetActive(false);
                }
                if (type1 == StatType.Lyrical || type2 == StatType.Lyrical)
                {
                    _buttons.Find(o => o.name == "Lyric").gameObject.SetActive(true);
                }
                if (type1 == StatType.Poetic || type2 == StatType.Poetic)
                {
                    _buttons.Find(o => o.name == "Poetic").gameObject.SetActive(true);
                }
                if (type1 == StatType.Narrative || type2 == StatType.Narrative)
                {
                    _buttons.Find(o => o.name == "Narrative").gameObject.SetActive(true);
                }
                if (type1 == StatType.Argumentative || type2 == StatType.Argumentative)
                {
                    _buttons.Find(o => o.name == "Argumentative").gameObject.SetActive(true);
                }
            }
            //gameObject.SetActive(false);
        }
        
    }

    private void ToggleButtonInteraction(BattleUIEventContext obj)
    {
        _imageCompnt.raycastTarget = !_imageCompnt.raycastTarget;
        if (_buttons != null)
        {
            
            foreach (Button button in _buttons)
            {
                button.interactable = !button.interactable;
            }
            
        }
    }

    private void ToggleButtonInteraction(BattleFlowStateMachineEventContext ctx)
    {
        _imageCompnt.raycastTarget = !_imageCompnt.raycastTarget;
        if (_buttons != null)
        {

            foreach (Button button in _buttons)
            {
                button.interactable = !button.interactable;
            }

        }
    }

    private void SetAvailableMinigames(BattleUIEventContext obj)
    {
        if (_playerStatus != null && Resources.FindObjectsOfTypeAll<TutorialState>()[0].TutoStatDone)
        {
            var top2 = _playerStatus.Stats
                                       .OrderByDescending(kv => kv.Value)
                                       .Take(2)
                                       .ToList();
            var type1 = top2[0].Key;
            var type2 = top2[1].Key;
            foreach (Button button in _buttons)
            {
                button.gameObject.SetActive(false);
            }
            if(type1==StatType.Lyrical || type2 == StatType.Lyrical)
            {
                _buttons.Find(o => o.name == "Lyric").gameObject.SetActive(true);
            }
            if (type1 == StatType.Poetic || type2 == StatType.Poetic)
            {
                _buttons.Find(o => o.name == "Poetic").gameObject.SetActive(true);
            }
            if (type1 == StatType.Narrative || type2 == StatType.Narrative)
            {
                _buttons.Find(o => o.name == "Narrative").gameObject.SetActive(true);
            }
            if (type1 == StatType.Argumentative || type2 == StatType.Argumentative)
            {
                _buttons.Find(o => o.name == "Argumentative").gameObject.SetActive(true);
            }
        }
    }

    

    public void OnButtonHover(string description)
    {
        bool buttonsEnabled = true;

        foreach (Button button in _buttons)
        {
            if (!button.interactable)
            {
                buttonsEnabled = false;
            }
        }

        if (buttonsEnabled)
            _descriptionField.text = description;
        else
            _descriptionField.text = "Vous devez choisir un personnage avant d'agir. Utilisez WASD pour naviguer et E pour sélectionner un personnage";
    }

    public void NoButtonHover()
    {
        _descriptionField.text = "";
    }

    private void OnDisable()
    {
        _battleFlowStateMachine.BattleFlowStateMachineEventBus.UISetup -= RemoveUnusedMinigame;
        _battleUIManager.BattleUIEventBus.OnToggleInteraction -= ToggleButtonInteraction;
        _battleUIManager.BattleUIEventBus.OnSetBaseValue -= SetAvailableMinigames;
    }
}
