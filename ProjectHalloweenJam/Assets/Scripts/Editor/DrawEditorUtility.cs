using System;
using UnityEditor;

namespace Editor
{
    public static class DrawEditorUtility
    {
        public static void DrawPropertyField(this SerializedProperty serializedProperty)
        {
            EditorGUILayout.PropertyField(serializedProperty);
        }

        public static void DrawEnum(this SerializedProperty serializedProperty)
        {
            var currentValue = serializedProperty.intValue;

            serializedProperty.DrawPropertyField();

            if (serializedProperty.intValue != 0 || currentValue == 0) 
                return;
            
            serializedProperty.intValue = 0;
            serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        public static void DrawDisabledFields(Action action, bool drawDisabled = true)
        {
            EditorGUI.BeginDisabledGroup(drawDisabled);
            
            action.Invoke();
            
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawHorizontalFields(Action action)
        {
            EditorGUILayout.BeginHorizontal();
            
            action.Invoke();
            
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawHeader(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }
        
        public static int DrawPopup(string label, int selectedIndex, string[] options)
        {
            return EditorGUILayout.Popup(label, selectedIndex, options);
        }
        
        public static void DrawMessage(string message, MessageType messageType = MessageType.Warning)
        {
            EditorGUILayout.HelpBox(message, messageType);
        }

        public static void DrawSpace(int amount = 12)
        {
            EditorGUILayout.Space(amount);
        }
    }
}