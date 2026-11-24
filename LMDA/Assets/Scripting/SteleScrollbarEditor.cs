#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SteleScrollbar))]
public class SteleScrollEditor : UnityEditor.UI.ScrollRectEditor
{
    SerializedProperty _duration;
    SerializedProperty _horizontalDimensions;
    SerializedProperty _tutorialUIElement;
    SerializedProperty _closeButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _duration = serializedObject.FindProperty("_clipDuration");
        _horizontalDimensions = serializedObject.FindProperty("_horizontalDimensions");
        _tutorialUIElement = serializedObject.FindProperty("_launchTutorialUIElement");
        _closeButton = serializedObject.FindProperty("_closeButton");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_duration);
        EditorGUILayout.PropertyField(_horizontalDimensions);
        EditorGUILayout.PropertyField(_tutorialUIElement);
        EditorGUILayout.PropertyField(_closeButton);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
