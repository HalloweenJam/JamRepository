using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private CursorData _cursorData;  
    public void SetCursor(CursorData.CursorType type)   
    {
        var (cursorTexture, hotspot) = _cursorData.GetTexture(type);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
