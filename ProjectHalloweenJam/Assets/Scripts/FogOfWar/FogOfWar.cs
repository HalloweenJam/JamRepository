using Managers;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : Singleton<FogOfWar>
{
    [SerializeField] private Texture2D _fogOfWarTexture;
    [SerializeField] private Image _image;
    [SerializeField] private SpriteMask _spriteMask;

    private Vector2 _worldScale;
    private Vector2Int _pixelScale;

    private void Start()
    {
        _pixelScale.x = _fogOfWarTexture.width;
        _pixelScale.y = _fogOfWarTexture.height;

        _worldScale.x = _pixelScale.x / 100f * transform.localScale.x;
        _worldScale.y = _pixelScale.y / 100f * transform.localScale.y;

        for (int i = 0; i < _pixelScale.x; i++)
        {
            for (int j = 0; j < _pixelScale.y; j++)
                _fogOfWarTexture.SetPixel(i, j, Color.clear);
        }
    }

    private Vector2Int WorldToPixel(Vector2 position)
    {
        Vector2Int pixelPosition = Vector2Int.zero;

        float deltaX = position.x - transform.position.x;
        float deltaY = position.y - transform.position.y;

        pixelPosition.x = Mathf.RoundToInt(.5f * _pixelScale.x + deltaX * (_pixelScale.x / _worldScale.x));
        pixelPosition.y = Mathf.RoundToInt(.5f * _pixelScale.y + deltaY * (_pixelScale.y / _worldScale.y));
        return pixelPosition;
    }

    public void MakeHole(Vector2 position, float holeRadius)
    {
        Vector2Int pixelPosition = WorldToPixel(position);
        int radius = Mathf.RoundToInt(holeRadius * _pixelScale.x / _worldScale.x);
        int px, nx, py, ny, distance;
        for (int i = 0; i < radius; i++)
        {
            distance = Mathf.RoundToInt(Mathf.Sqrt(radius * radius - i * i));
            for (int j = 0; j < distance; j++)
            {
                px = Mathf.Clamp(pixelPosition.x + i, 0, _pixelScale.x);
                nx = Mathf.Clamp(pixelPosition.x - i, 0, _pixelScale.x);
                py = Mathf.Clamp(pixelPosition.y + j, 0, _pixelScale.y);
                ny = Mathf.Clamp(pixelPosition.y - j, 0, _pixelScale.y);

                _fogOfWarTexture.SetPixel(px, py, Color.black);
                _fogOfWarTexture.SetPixel(nx, py, Color.black);
                _fogOfWarTexture.SetPixel(px, ny, Color.black);
                _fogOfWarTexture.SetPixel(nx, ny, Color.black);
            }
        }
        _fogOfWarTexture.Apply();        
        CreateSprite();
    }

    private void CreateSprite()
    {
        _spriteMask.sprite = Sprite.Create(_fogOfWarTexture, new Rect(0, 0, _fogOfWarTexture.width, _fogOfWarTexture.height), Vector2.one * .5f, 100);
        _image.sprite = _spriteMask.sprite;
    } 
}
