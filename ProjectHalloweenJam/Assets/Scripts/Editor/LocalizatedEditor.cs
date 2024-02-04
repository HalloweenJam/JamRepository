using UnityEditor;

[CustomEditor(typeof(LocalizedText))]
public class LocalizatedEditor : UnityEditor.Editor
{
    private SerializedProperty _key;

    private void OnEnable() => _key = serializedObject.FindProperty("_key");

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        EditorGUILayout.PropertyField(_key, true);
        serializedObject.ApplyModifiedProperties();
    }
}
