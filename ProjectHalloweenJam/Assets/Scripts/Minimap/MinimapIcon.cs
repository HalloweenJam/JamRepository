using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _color;
    public RectTransform RectTransform;

    public void SetIcon(Sprite icon) => _image.sprite = icon;
    public void SetColor(Color color) => _image.color = color;

    public Image Image => _image;
}

