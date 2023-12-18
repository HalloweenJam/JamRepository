using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CursorData", menuName = "Cursor")]
public class CursorData : ScriptableObject
{
    public enum CursorType
    {
        Default,
        Aim,
        Reload
    }

    [SerializeField] private List<CursorItem> _cursorItems = new();
         
    private Dictionary<CursorType, (Texture2D, Vector2)> _cursors = new();

    private void OnValidate()
    {
        foreach (CursorItem item in _cursorItems)       
            _cursors.Add(item.CursorType, (item.Texture, item.Hotspot));     
    }

    public (Texture2D texture, Vector2 hotspot) GetTexture(CursorType type) => _cursors.GetValueOrDefault(type);
}

[Serializable]
public class CursorItem
{
    public CursorData.CursorType CursorType;
    public Vector2 Hotspot;
    public Texture2D Texture;
}
