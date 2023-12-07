using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Generation.AnchorSystem.Data
{
    [CreateAssetMenu(fileName = "SpawnPointData", menuName = "Generation/SpawnPointData", order = 0)]
    public class SpawnPointsData : ScriptableObject
    {
        [SerializeField] private List<Vector2> _positions;

        public IReadOnlyList<Vector2> Positions => _positions;

#if UNITY_EDITOR
        public void InitList(List<Vector2> positions)
        {
            _positions = positions;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
        
    }
}