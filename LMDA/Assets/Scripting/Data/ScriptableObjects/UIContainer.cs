using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIContainer", menuName = "UI/UIContainer", order = 2)]
public class UIContainer : ScriptableObject
{
    public List<UINameContainer> UserInterfaces = new();
}

[Serializable]
[ExecuteInEditMode]
public class UINameContainer
{
    public Transform UIPrefab;
    public string Name => UIPrefab != null ? UIPrefab.name : string.Empty;
}
