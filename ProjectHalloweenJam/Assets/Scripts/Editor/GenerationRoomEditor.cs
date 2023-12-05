using System.Collections.Generic;
using Generation.Rooms;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GenerationRoom))]
    public class GenerationRoomEditor : UnityEditor.Editor
    {
        private GenerationRoom _target;
        
        private void OnEnable()
        {
            _target = (GenerationRoom) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            
            if (EditorGUI.EndChangeCheck())
                SceneView.RepaintAll();

            if (!GUI.changed)
                return;
            
            Save();
        }

        private void Save()
        {
            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
        }

        private void OnSceneGUI()
        {
            DrawBorders();
            
            var centerX = 0f;
            var centerY = 0f;

            foreach (var sideCenter in _target.BorderCapsPositions)
            {
                centerX += sideCenter.x;
                centerY += sideCenter.y;
            }
            centerX /= 2;
            centerY /= 2;

            var pos = new Vector2(centerX, centerY);
            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(pos, Vector3.forward, .5f);
            _target.OverlapCenter = pos;
        }
        
        private void DrawBorders()
        {
            Handles.color = Color.green;
            var capsPositions = _target.BorderCapsPositions;

            for (var borderCap = 0; borderCap < 4; borderCap++)
            {
                var position = capsPositions[borderCap];

                if (borderCap > 1)
                    position += (capsPositions[1] + capsPositions[0]) / 2;
                else
                    position += (capsPositions[3] + capsPositions[2]) / 2;

                DrawSolidDisc(position);

                var newPos = (Vector2) Handles.FreeMoveHandle(position, .5f, Vector2.zero, Handles.CylinderHandleCap);

                if (position == newPos)
                    continue;

                Undo.RecordObject(_target, "Border Moved");
                MoveBorderPoint(borderCap, newPos);
            }

            DrawRectLines(capsPositions);
        }

        private static void DrawSolidDisc(Vector2 position)
        {
            Handles.DrawSolidDisc(position, Vector3.forward, .5f);
        }

        private static void DrawRectLines(List<Vector2> capsPositions)
        {
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[0].y),
                new Vector2(capsPositions[2].x, capsPositions[0].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[1].y),
                new Vector2(capsPositions[2].x, capsPositions[1].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[2].x, capsPositions[1].y),
                new Vector2(capsPositions[2].x, capsPositions[0].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[1].y),
                new Vector2(capsPositions[3].x, capsPositions[0].y), 3);
        }

        private void MoveBorderPoint(int index, Vector2 pos)
        {
            var snapValue = _target.SnapValue;

            var roundedX = snapValue * Mathf.Round(pos.x / snapValue);
            var roundedY = snapValue * Mathf.Round(pos.y / snapValue);

            var newPosition = _target.UseSnap
                ? new Vector2(index > 1 ? roundedX : 0, index > 1 ? 0 : roundedY)
                : new Vector2(index > 1 ? pos.x : 0, index > 1 ? 0 : pos.y);

            _target.BorderCapsPositions[index] = newPosition;
            _target.UpdateBorders();
        }
    }
}