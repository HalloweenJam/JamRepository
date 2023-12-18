using UnityEngine;

public class MinimapWorldAgent : MonoBehaviour
{
    public Sprite Icon;
    public Color IconColor = Color.white;

    private void Start() 
        => Minimap.Instance.OnLoadedMinimap += () => Minimap.Instance.RegisterMinimapPlayer(this);
}
