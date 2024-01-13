using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bootstrap))]
public class BootstrapEditor : UnityEditor.Editor
{
    private Bootstrap _bootstrap;

    [Header("StartGameProperty")]
    private SerializedProperty _minimap;
    private SerializedProperty _mainCollider;
    private SerializedProperty _collisionCollider;
    private SerializedProperty _navMeshSurface;
    private SerializedProperty _fogOfWarSprite;

    private void OnEnable()
    {
        _bootstrap = (Bootstrap)target;

        _minimap = serializedObject.FindProperty("_minimap");
        _mainCollider = serializedObject.FindProperty("_mainCollider");
        _collisionCollider = serializedObject.FindProperty("_collisionCollider");
        _navMeshSurface = serializedObject.FindProperty("_navMeshSurface");
        _fogOfWarSprite = serializedObject.FindProperty("_fogOfWarSprite");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if(_bootstrap.UseStartProperties)
        {
            EditorGUILayout.PropertyField(_minimap, true);
            EditorGUILayout.PropertyField(_mainCollider, true);
            EditorGUILayout.PropertyField(_collisionCollider, true);
            EditorGUILayout.PropertyField(_navMeshSurface, true);
            EditorGUILayout.PropertyField(_fogOfWarSprite, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
