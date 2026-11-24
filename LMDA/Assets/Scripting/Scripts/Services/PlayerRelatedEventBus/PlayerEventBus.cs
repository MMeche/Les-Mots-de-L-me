using System;
using UnityEngine;

public class PlayerEventBus
{
    public event Action<PlayerEventContext> OnAddExp;
    public void AddExp(PlayerEventContext context) => OnAddExp?.Invoke(context);

    public event Action<PlayerEventContext> OnSetExp;
    public void SetExp(PlayerEventContext context) => OnSetExp?.Invoke(context);

    public event Action<PlayerEventContext> OnSetLvl;
    public void SetLvl(PlayerEventContext context) => OnSetLvl?.Invoke(context);

    public event Action<PlayerEventContext> OnSetStats;
    public void SetStats(PlayerEventContext context) => OnSetStats?.Invoke(context);

    public event Action<PlayerEventContext> OnModifierAdded;
    public void AddModifier(PlayerEventContext context) => OnModifierAdded?.Invoke(context);

    public event Action<PlayerEventContext> OnModifierRemoved;
    public void RemoveModifier(PlayerEventContext context) => OnModifierRemoved?.Invoke(context);
}
