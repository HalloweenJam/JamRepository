using System.Collections.Generic;
using Generation.AnchorSystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PositionsGenerator))]
    public class PositionsGeneratorEditor : UnityEditor.Editor
    {
        private PositionsGenerator _positionsGenerator;
        
        private const int _buttonWidth = 300;
        private const int _buttonHeight = 30;
        
        private readonly GUILayoutOption[] _buttonsGUIOptions =
        {
            GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonHeight)
        };
        
        private void OnEnable()
        {
            _positionsGenerator = (PositionsGenerator) target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUI.BeginChangeCheck();
            
            DrawEditorUtility.DrawSpace();
            
            if (_positionsGenerator.SpawnPositions.Count < 1)
                DrawEditorUtility.DrawMessage("Spawn points count less than 1, save button cannot be work");
            
            DrawEditorUtility.DrawHorizontalFields(DrawExportButtons);
            DrawEditorUtility.DrawSpace();
            DrawEditorUtility.DrawHorizontalFields(DrawAnchorButtons);
            DrawEditorUtility.DrawHorizontalFields(DrawSpawnPointButtons);
            
            if (EditorGUI.EndChangeCheck())
                SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            ReadInput();
            
            DrawPoints(_positionsGenerator.Anchors, _positionsGenerator.AnchorColor);
            DrawPoints(_positionsGenerator.SpawnPositions, _positionsGenerator.SpawnPointColor);

            DrawBorders();
        }
        
        private void ReadInput()
        {
            var guiEvent = Event.current;
            var mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
            
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.control)
            {
                var minDistToPoint = .5f;
                var closestPointIndex = -1;

                var positions = _positionsGenerator.AllPositionsList;

                for (int i = 0; i < positions.Count; i++)
                {
                    var dist = Vector2.Distance(mousePos, positions[i]);

                    if (dist > minDistToPoint) 
                        continue;
                    
                    minDistToPoint = dist;
                    closestPointIndex = i;
                }

                if (closestPointIndex != -1)
                {
                    Undo.RecordObject(_positionsGenerator, "Point Deleted");
                    _positionsGenerator.DeletePoint(closestPointIndex);
                }
            }
            
            HandleUtility.AddDefaultControl(0);
        }

        private void DrawExportButtons()
        {
            if (_positionsGenerator.SpawnPointsData == null)
            {
                DrawEditorUtility.DrawMessage("Spawn point data is null");
                return;
            }
            
            if (GUILayout.Button("Save", _buttonsGUIOptions) && _positionsGenerator.SpawnPositions.Count > 0)
            {
                var positionsList = new List<Vector2>();

                foreach (var position in _positionsGenerator.SpawnPositions)
                {
                    positionsList.Add(position);
                }

                _positionsGenerator.SpawnPointsData.InitList(positionsList);
            }
            
            if (GUILayout.Button("Load", _buttonsGUIOptions))
            {
                _positionsGenerator.SpawnPositions.Clear();
                
                var positions = _positionsGenerator.SpawnPointsData.Positions;

                foreach (var position in positions)
                {
                   _positionsGenerator.SpawnPositions.Add(position);
                }
            }
        }
        
        private void DrawSpawnPointButtons()
        {
            if (GUILayout.Button("Generate Spawn Points", _buttonsGUIOptions))
            {
                Undo.RecordObject(_positionsGenerator, "Spawn Positions Generated");
                _positionsGenerator.GenerateSpawnPositions(_positionsGenerator.PointsCount);
            }

            if (GUILayout.Button("Clear Spawn Points", _buttonsGUIOptions))
            {
                Undo.RecordObject(_positionsGenerator, "Spawn Positions Deleted");
                _positionsGenerator.SpawnPositions.Clear();
            }
        }

        private void DrawAnchorButtons()
        {
            if (GUILayout.Button("New Anchor", _buttonsGUIOptions))
            {
                Undo.RecordObject(_positionsGenerator, "Anchor Added");
                _positionsGenerator.Anchors.Add(new Vector2(10, 10));
            }

            if (GUILayout.Button("Clear Anchors", _buttonsGUIOptions))
            {
                Undo.RecordObject(_positionsGenerator, "Anchors Deleted");
                _positionsGenerator.Anchors.Clear();
            }
        }

        private void DrawPoints(List<Vector2> positions, Color color)
        {
            Handles.color = color;
            
            for (var index = 0; index < positions.Count; index++)
            {
                var position = positions[index];
                
                Handles.DrawSolidDisc(position, Vector3.forward, .5f);

                var newPos = (Vector2) Handles.FreeMoveHandle(position, .5f, Vector2.zero, Handles.CylinderHandleCap);

                if (position == newPos)
                    continue;
                
                Undo.RecordObject(_positionsGenerator, "Point Moved");
                MovePoint(positions, index, newPos);
            }
        }
        
        private void DrawBorders()
        {
            Handles.color = _positionsGenerator.AreaColor;
            var capsPositions = _positionsGenerator.BorderCapsPositions;

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
                
                Undo.RecordObject(_positionsGenerator, "Border Moved");
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
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[0].y),new Vector2(capsPositions[2].x, capsPositions[0].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[1].y),new Vector2(capsPositions[2].x, capsPositions[1].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[2].x, capsPositions[1].y),new Vector2(capsPositions[2].x, capsPositions[0].y), 3);
            Handles.DrawLine(new Vector2(capsPositions[3].x, capsPositions[1].y),new Vector2(capsPositions[3].x, capsPositions[0].y), 3);
        }

        private void MoveBorderPoint(int index, Vector2 pos)
        {
            var newPosition = _positionsGenerator.UseSnap
                ? new Vector2(index > 1 ? Mathf.RoundToInt(pos.x) : 0, index > 1 ? 0 : Mathf.RoundToInt(pos.y))
                : new Vector2(index > 1 ? pos.x : 0, index > 1 ? 0 : pos.y);

            _positionsGenerator.BorderCapsPositions[index] = newPosition;
        }
        
        private void MovePoint(List<Vector2> positions, int index, Vector2 pos)
        {
            positions[index] = _positionsGenerator.UseSnap ? new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)) : pos;
        }
    }
}