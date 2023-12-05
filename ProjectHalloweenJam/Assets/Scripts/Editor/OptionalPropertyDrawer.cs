using UnityEditor;
using UnityEngine;
using Utilities.Classes;

namespace Editor
{
    [CustomPropertyDrawer(typeof(OptionalProperty<>))]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("_value");
            var enabledProperty = property.FindPropertyRelative("_enabled");

            pos.width -= 24;
            EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
            EditorGUI.PropertyField(pos, valueProperty, label, true);
            EditorGUI.EndDisabledGroup();

            pos.x += pos.width + 24;
            pos.width = pos.height = EditorGUI.GetPropertyHeight(enabledProperty);
            pos.x -= pos.width;
            EditorGUI.PropertyField(pos, enabledProperty, GUIContent.none);
        }
    }
}