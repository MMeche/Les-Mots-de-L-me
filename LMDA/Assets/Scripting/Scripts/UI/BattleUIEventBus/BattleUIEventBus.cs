using System;
using UnityEngine;

public class BattleUIEventBus
{
    public event Action<BattleUIEventContext> OnTutoDisplay;
    public void DisplayTuto(BattleUIEventContext context) => OnTutoDisplay?.Invoke(context);

    public event Action<BattleUIEventContext> OnSetBaseValue;
    public void SetBaseValue(BattleUIEventContext context) => OnSetBaseValue?.Invoke(context);

    public event Action<BattleUIEventContext> OnToggleInteraction;
    public void InteractionToggle(BattleUIEventContext context) => OnToggleInteraction?.Invoke(context);

    public event Action<BattleUIEventContext> OnEnemyStatusUpdate;
    public void UpdateEnemyStatus(BattleUIEventContext context) => OnEnemyStatusUpdate?.Invoke(context);

    public event Action<BattleUIEventContext> OnPlayerStatusUpdate;
    public void UpdatePlayerStatus(BattleUIEventContext context) => OnPlayerStatusUpdate?.Invoke(context);
    
    public event Action<BattleUIEventContext> OnEnemyIncomingDamageUpdate;
    public void UpdateEnemyIncomingDamage(BattleUIEventContext context) => OnEnemyIncomingDamageUpdate?.Invoke(context);
    public event Action<BattleUIEventContext> OnPlayerIncomingDamageUpdate;
    public void UpdatePlayerIncomingDamage(BattleUIEventContext context) => OnPlayerIncomingDamageUpdate?.Invoke(context);

    public event Action<BattleUIEventContext> OnArgWordCardClicked;
    public void ArgWordCardClicked(BattleUIEventContext context) => OnArgWordCardClicked?.Invoke(context);

}
