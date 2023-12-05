using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileData" , menuName = "TileData")]
public class TileData : ScriptableObject
{
    [SerializeField] private Grid _gridPrefab;

    [Header("Tiles")]
    [SerializeField] private Tile _ground;

    [Header("Walls")]
    [SerializeField] private Tile _wallTop;
    [SerializeField] private Tile _wallBottom;
    [SerializeField] private Tile _wallLeft;
    [SerializeField] private Tile _wallRight;
    [SerializeField] private Tile _wallTopRight;
    [SerializeField] private Tile _wallTopLeft;
    [SerializeField] private Tile _wallBottomLeft;
    [SerializeField] private Tile _wallBottomRight;
    [SerializeField] private Tile _wallRoomTop;
    [SerializeField] private Tile _wallRoomTopBody;

    public Grid GridPrefab => _gridPrefab;
    public Tile Ground => _ground;
    public Tile WallTop => _wallTop;
    public Tile WallBottom => _wallBottom;
    public Tile WallLeft => _wallLeft;
    public Tile WallRight => _wallRight;
    public Tile WallTopRight => _wallTopRight;
    public Tile WallTopLeft => _wallTopLeft;
    public Tile WallBottomLeft => _wallBottomLeft;
    public Tile WallBottomRight => _wallBottomRight;
    public Tile WallRoomTopBody => _wallRoomTopBody;
    public Tile WallRoomTop => _wallRoomTop;
}
