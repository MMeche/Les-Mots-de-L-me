using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStatDisplay : MonoBehaviour
{
    UIManager _uiManager;
    [SerializeField] CharacterUIData[] _characterUIData;
    [SerializeField] TMP_Text[] _statText;
    [SerializeField] TMP_Text _skillPointsText;
    [SerializeField] List<Slider> _expSlider;
    [SerializeField] TMP_Text _currentLevelText;
    [SerializeField] TMP_Text _nextLevelText;
    [SerializeField] TMP_Text _characterName;
    [SerializeField] Image _characterIcon;

    void Start()
    {
        _uiManager = ServiceProvider.Instance.ServiceLocator.Get<UIManager>();
        _uiManager.UIEventBus.OnStatDisplayUpdated += UpdateStatDisplay;
        _uiManager.UIEventBus.OnSkillPointDisplayUpdated += UpdateSkillPoints;
        _uiManager.UIEventBus.OnExpDisplayUpdated += UpdateExpDisplay;
        _uiManager.UIEventBus.OnLvlDisplayUpdated += UpdateLevelDisplay;
        _uiManager.UIEventBus.OnUISetup += SetUIData;
    }

    private void SetUIData(UIEventContext obj)
    {
        _currentLevelText.text = obj.Level.ToString();
        _nextLevelText.text = (obj.Level + 1).ToString();
        _skillPointsText.text = obj.SkillPoints.ToString();
        foreach(var slider in _expSlider)
        {
            slider.maxValue = obj.ExpToNextLvl;
            slider.value = obj.CurrentExp;
        }
        

        _statText[(int)StatType.Narrative].text = obj.Narrative.ToString();
        _statText[(int)StatType.Poetic].text = obj.Poetic.ToString();
        _statText[(int)StatType.Lyrical].text = obj.Lyric.ToString();
        _statText[(int)StatType.Argumentative].text = obj.Argumentative.ToString();

        _characterIcon.sprite = _characterUIData[obj.CharacteIndex].CharacterPicture;
        _characterName.text = _characterUIData[obj.CharacteIndex].CharacterName;
    }

    
    

    private void UpdateLevelDisplay(UIEventContext obj)
    {
        _currentLevelText.text = obj.Level.ToString();
        int nextLvl = (obj.Level + 1);
        _nextLevelText.text = nextLvl.ToString();
    }

    private void UpdateSkillPoints(UIEventContext obj)
    {
        _skillPointsText.text = obj.SkillPoints.ToString();
    }

    private void UpdateStatDisplay(UIEventContext obj)
    {
        _statText[obj.StatTypeIndex].text = obj.StatValue.ToString();
    }

    private void UpdateExpDisplay(UIEventContext obj)
    {
        foreach (var slider in _expSlider)
        {
            slider.maxValue = obj.ExpToNextLvl;
            slider.value = obj.CurrentExp;
        }
    }
}
