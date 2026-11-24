using System;

public class UIEventBus
{
    public event Action<UIEventContext> OnLvlDisplayUpdated;
    public void UpdateLvlDisplay(UIEventContext context) => OnLvlDisplayUpdated?.Invoke(context);

    public event Action<UIEventContext> OnExpDisplayUpdated;
    public void UpdateExpDisplay(UIEventContext context) => OnExpDisplayUpdated?.Invoke(context);

    public event Action<UIEventContext> OnStatDisplayUpdated;
    public void UpdateStatDisplay(UIEventContext context) => OnStatDisplayUpdated?.Invoke(context);

    public event Action<UIEventContext> OnSkillPointDisplayUpdated;
    public void UpdateSkillPointDisplay(UIEventContext context) => OnSkillPointDisplayUpdated?.Invoke(context);

    public event Action<UIEventContext> OnCharacterSwap;
    public void SwapCharacter(UIEventContext context) => OnCharacterSwap?.Invoke(context);

    public event Action<UIEventContext> OnUISetup;
    public void SetupUI(UIEventContext context) => OnUISetup?.Invoke(context);

    public event Action<UIEventContext> OnSaveItemClicked;
    public void SaveItemClicked(UIEventContext context) => OnSaveItemClicked?.Invoke(context);
}
