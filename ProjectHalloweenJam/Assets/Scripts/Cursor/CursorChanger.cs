using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private CursorData _cursorData;

    private void Start() => SetCursor(CursorData.CursorType.Aim);
  
    public void SetCursor(CursorData.CursorType type)   
    {
        var (cursorTexture, hotspot) = _cursorData.GetTexture(type);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
