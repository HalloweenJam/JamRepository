using System.Collections.Generic;
using UnityEngine;
using CorridorGeneration;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public enum Dir
    {
        Left,
        Right,
        Top,
        Bottom,
    }

    [SerializeField] private List<Transform> _cells;
    public List<Transform> Cells => _cells;

    public Dir Direction;

    public void SetActive(bool active, Tilemap wallTilemap)
    {
        float coeffX = 0;
        float coeffY = 0;

        gameObject.SetActive(active);

        if (Direction == Dir.Right || Direction == Dir.Left)
            coeffX = Direction == Dir.Right ? -1 : 1;

        for (int i = 0; i < Cells.Count; i++)
        {
            Tile tile = GetTile(active, 0);
            if (Direction == Dir.Top || Direction == Dir.Bottom)
                coeffY = Direction == Dir.Top ? 0 : 1;

            Transform doorTransform = Cells[i].transform;
            Vector3 position = doorTransform.position;
            position = wallTilemap.transform.InverseTransformPoint(position);

            position.x += coeffX;
            position.y += coeffY;

            Vector3Int tilePosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
            wallTilemap.SetTile(tilePosition, tile);

            if (Direction == Dir.Top)
            {
                tile = GetTile(active, 1);
                coeffY = -1;
                position.y += coeffY;
                tilePosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
                wallTilemap.SetTile(tilePosition, tile);
            }
        }
    }


    private Tile GetTile(bool active, int drawOption)
    {
        if (active)
            return null;

        switch (Direction)
        {
            case Dir.Left:
                return CorridorGenerator.Instance.TildeData.WallLeft;

            case Dir.Right:
                return CorridorGenerator.Instance.TildeData.WallRight;

            case Dir.Top:
                if(drawOption == 0)
                    return CorridorGenerator.Instance.TildeData.WallRoomTop;
                else
                    return CorridorGenerator.Instance.TildeData.WallRoomTopBody;

            case Dir.Bottom:
                return CorridorGenerator.Instance.TildeData.WallBottom;

            default:
                return null;
        }
    }
}
