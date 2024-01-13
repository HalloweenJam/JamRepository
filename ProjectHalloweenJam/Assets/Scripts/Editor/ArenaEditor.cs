using UnityEditor;

[CustomEditor(typeof(Arena))]
public class ArenaEditor : UnityEditor.Editor
{
    private Arena _arena;
    private SerializedProperty _fogTransform;
    private SerializedProperty _fogRadius;
    private SerializedProperty _castscene;

    private void OnEnable()
    {
        _arena = (Arena)target;

        _fogTransform = serializedObject.FindProperty("_centerRoom");   
        _fogRadius = serializedObject.FindProperty("_radius");
        _castscene = serializedObject.FindProperty("_castscene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if(!_arena.IsBossArena)
        {
            EditorGUILayout.PropertyField(_fogTransform, true);
            EditorGUILayout.PropertyField(_fogRadius, true);
        }
        else
            EditorGUILayout.PropertyField(_castscene, true);

        serializedObject.ApplyModifiedProperties();
    }
}
