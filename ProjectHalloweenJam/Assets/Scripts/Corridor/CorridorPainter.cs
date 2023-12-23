using UnityEngine;
using UnityEngine.Tilemaps;
using DR = Door.Dir;

namespace CorridorGeneration
{
    public class CorridorPainter
    {
        public enum Shape
        {
            ZigzagHorizontal,
            ZigzagVertical,
            HalfZigzag,
            Rectangular,
            BracketsVertical,
            BracketsHorizontal,
            StraightX,
            StraightY,
        }

        private Grid _grid;
        private TileData _tileData;

        #region Variables
        private float _distanceX;
        private float _distanceY;

        private float _coeffX;
        private float _coeffY;

        private float _width;
        private float _height;

        private bool _isFirstTile;
        private bool _firstCellHigher;

        private int _drawingOption;
        #endregion

        public CorridorPainter(TileData tileData, int drawingOption)
        {
            _tileData = tileData;
            _drawingOption = drawingOption;
        }

        public Grid GenerateCorridor(Door door1, Door door2)
        {
            _grid = CorridorGenerator.Instance.InstantiateCorridorGrid();
            Tilemap groundTilemap = _grid.transform.GetChild(0).GetComponent<Tilemap>();
            Tilemap wallsTilemap = _grid.transform.GetChild(1).GetComponent<Tilemap>();
            wallsTilemap.GetComponent<TilemapRenderer>().renderingLayerMask = 2;
            _grid.name = "Corridor_" + $"{_drawingOption}";
            groundTilemap.name = "Tilemap";

            DrawCorridor(groundTilemap, wallsTilemap, door1, door2);
            return _grid;
        }

        private void CalculateCoefficients(Door startDoor, Door endDoor)
        {
            Vector2 startDoorPosition = startDoor.transform.position;
            Vector2 endDoorPosition = endDoor.transform.position;

            _distanceX = endDoorPosition.x - startDoorPosition.x;
            _distanceY = endDoorPosition.y - startDoorPosition.y;

            _coeffX = 0;
            _coeffY = 0;

            if (_distanceX != 0)
                _coeffX = _distanceX > 0 ? 1 : -1f;
            if (_distanceY != 0)
                _coeffY = _distanceY > 0 ? 1f : -1f;

            _width = Mathf.Abs(_distanceX);
            _height = Mathf.Abs(_distanceY);
        }

        private void SetDoors(bool firstCondition, bool secondCondition, Door firstDoor, Door secondDoor, out Door startDoor, out Door endDoor)
        {
            startDoor = firstDoor;
            endDoor = secondDoor;
            if (firstCondition)
            {
                startDoor = firstDoor;
                endDoor = secondDoor;
            }
            else if (secondCondition)
            {
                startDoor = secondDoor;
                endDoor = firstDoor;
            }

            CalculateCoefficients(startDoor, endDoor);
            _firstCellHigher = startDoor.Cells[0].position.y > startDoor.Cells[1].position.y;
        }

        private void DrawCorridor(Tilemap groundTilemap, Tilemap wallsTilemap, Door firstDoor, Door secondDoor)
        {
            Door startDoor;
            Door endDoor;

            Vector2 startPosition = default;
            Vector2 lastPositionY = default;

            bool firstCondition;
            bool secondCondition;

            if ((firstDoor.Direction == DR.Right && secondDoor.Direction == DR.Left) || (firstDoor.Direction == DR.Left && secondDoor.Direction == DR.Right))
            {
                #region horizontal zigzag
                firstCondition = firstDoor.Direction == DR.Right;
                secondCondition = secondDoor.Direction == DR.Right;
                SetDoors(firstCondition, secondCondition, firstDoor, secondDoor, out startDoor, out endDoor);

                if (_coeffY == 0)
                {
                    for (int i = 0; i < startDoor.Cells.Count; i++)
                    {
                        _isFirstTile = i == 0 ? true : false;
                        startPosition = startDoor.Cells[i].position;
                        DrawByX(groundTilemap, wallsTilemap, startPosition, _width, _coeffX, _firstCellHigher, _isFirstTile);
                    }
                    return;
                }

                float halfWidth = Mathf.Round(_width / 2);
                _width -= halfWidth;

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;
                    startPosition = startDoor.Cells[i].position;
                    startPosition = DrawByX(groundTilemap, wallsTilemap, startPosition, halfWidth, _coeffX, _firstCellHigher, _isFirstTile);
                }

                startPosition.y = _coeffY == -1f ? startPosition.y += 1 : startPosition.y;
                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    startPosition.x += 1;
                    lastPositionY = DrawByY(groundTilemap, wallsTilemap, startPosition, _height + 2, _coeffY, _coeffX, i, Shape.ZigzagHorizontal);
                }

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;

                    if (_coeffY == -1)
                        lastPositionY.y = i == 0 ? lastPositionY.y += 1 : lastPositionY.y -= 1;
                    else
                        lastPositionY.y = i == 0 ? lastPositionY.y : lastPositionY.y -= 1;

                    DrawByX(groundTilemap, wallsTilemap, lastPositionY, _width - 1, _coeffX, _firstCellHigher, _isFirstTile);
                }
                #endregion
            }

            if ((firstDoor.Direction == DR.Right || firstDoor.Direction == DR.Left) && (secondDoor.Direction == DR.Top || secondDoor.Direction == DR.Bottom) ||
                (firstDoor.Direction == DR.Top || firstDoor.Direction == DR.Bottom) && (secondDoor.Direction == DR.Right || secondDoor.Direction == DR.Left))
            {
                #region rectangular
                firstCondition = firstDoor.Direction == DR.Right || firstDoor.Direction == DR.Left;
                secondCondition = secondDoor.Direction == DR.Right || secondDoor.Direction == DR.Left;
                SetDoors(firstCondition, secondCondition, firstDoor, secondDoor, out startDoor, out endDoor);

                _height = _coeffY == -1f ? _height - 1 : _height;
                _width = _coeffX == 1f ? _width : _width - 1;

                for (int i = 0; i < firstDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;
                    startPosition = startDoor.Cells[i].position;
                    startPosition = DrawByX(groundTilemap, wallsTilemap, startPosition, _width, _coeffX, _firstCellHigher, _isFirstTile);
                }

                startPosition.y = _coeffY == -1f ? startPosition.y += 1 : startPosition.y;
                for (int i = 0; i < firstDoor.Cells.Count; i++)
                {
                    startPosition.x += _coeffX;
                    DrawByY(groundTilemap, wallsTilemap, startPosition, _height + 1, _coeffY, _coeffX, i, Shape.Rectangular);
                }
                #endregion
            }

            if ((firstDoor.Direction == DR.Top && secondDoor.Direction == DR.Bottom) || (firstDoor.Direction == DR.Bottom && secondDoor.Direction == DR.Top))
            {
                #region zigzag vertical
                firstCondition = firstDoor.Direction == DR.Top;
                secondCondition = secondDoor.Direction == DR.Top;
                SetDoors(firstCondition, secondCondition, firstDoor, secondDoor, out startDoor, out endDoor);

                if (_coeffX == 0)
                {
                    for (int i = 0; i < startDoor.Cells.Count; i++)
                    {
                        startPosition = startDoor.Cells[i].position;
                        startPosition.y += 1;
                        DrawByY(groundTilemap, wallsTilemap, startPosition, _height - 1, _coeffY, _coeffX, i, Shape.StraightX);
                    }
                    return;
                }

                float halfHeight = Mathf.Round(_height / 2);
                _height -= halfHeight;

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    startPosition = startDoor.Cells[i].position;
                    startPosition.y += 1;
                    lastPositionY = DrawByY(groundTilemap, wallsTilemap, startPosition, halfHeight, _coeffY, _coeffX, i, Shape.ZigzagVertical);
                }

                if (_width < 3)
                {
                    Shape shape = _width < 2 ? Shape.HalfZigzag : Shape.StraightY;
                    lastPositionY.y -= 1;
                    lastPositionY.x += (_width - 1) * _coeffX;
                    for (int i = 0; i < startDoor.Cells.Count; i++)
                    {
                        lastPositionY.x = _coeffX == 1 ? lastPositionY.x += i : lastPositionY.x += _coeffX;
                        DrawByY(groundTilemap, wallsTilemap, lastPositionY, _height + 1, _coeffY, _coeffX, i, shape);
                    }
                    return;
                }

                lastPositionY.x = _coeffX == -1 ? lastPositionY.x -= 1 : lastPositionY.x;
                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    lastPositionY.y -= i;
                    startPosition = lastPositionY;
                    _isFirstTile = i == 0 ? true : false;
                    startPosition = DrawByX(groundTilemap, wallsTilemap, startPosition, _width - 1, _coeffX, !_firstCellHigher, _isFirstTile);
                }

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    startPosition.x += _coeffX;
                    DrawByY(groundTilemap, wallsTilemap, startPosition, _height + 1, _coeffY, _coeffX, i, Shape.StraightY);
                }
                #endregion
            }

            if ((firstDoor.Direction == DR.Right && secondDoor.Direction == DR.Right) || (firstDoor.Direction == DR.Left && secondDoor.Direction == DR.Left))
            {
                #region brackets vertical
                firstCondition = secondDoor.transform.position.y - firstDoor.transform.position.y >= 0;
                secondCondition = secondDoor.transform.position.y - firstDoor.transform.position.y < 0;
                SetDoors(firstCondition, secondCondition, firstDoor, secondDoor, out startDoor, out endDoor);

                float firstWidth = _coeffX == 0 ? 2 : 0;
                float secondWidth = _coeffX == 0 ? 2 : 0;

                if (firstDoor.Direction == DR.Right && secondDoor.Direction == DR.Right && _coeffX != 0)
                {
                    firstWidth = _coeffX == -1 ? 2 : _width + 2;
                    secondWidth = _coeffX == -1 ? _width + 2 : 2;
                }
                else if (firstDoor.Direction == DR.Left && secondDoor.Direction == DR.Left && _coeffX != 0)
                {
                    firstWidth = _coeffX == -1 ? _width + 2 : 2;
                    secondWidth = _coeffX == -1 ? 2 : _width + 2;
                }

                _coeffX = (firstDoor.Direction == DR.Right && secondDoor.Direction == DR.Right) ? 1 : -1;
                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;
                    startPosition = startDoor.Cells[i].position;
                    startPosition = DrawByX(groundTilemap, wallsTilemap, startPosition, firstWidth, _coeffX, _firstCellHigher, _isFirstTile);
                }

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    startPosition.x += _coeffX;
                    DrawByY(groundTilemap, wallsTilemap, startPosition, _height + 2, _coeffY, _coeffX, i, Shape.BracketsVertical);
                }

                for (int i = 0; i < endDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;
                    startPosition = endDoor.Cells[i].position;
                    DrawByX(groundTilemap, wallsTilemap, startPosition, secondWidth, _coeffX, _firstCellHigher, _isFirstTile);
                }
                #endregion
            }

            if ((firstDoor.Direction == DR.Top && secondDoor.Direction == DR.Top) || (firstDoor.Direction == DR.Bottom && secondDoor.Direction == DR.Bottom))
            {
                #region brackets horizontal
                firstCondition = secondDoor.transform.position.x - firstDoor.transform.position.x >= 0;
                secondCondition = secondDoor.transform.position.x - firstDoor.transform.position.x < 0;
                SetDoors(firstCondition, secondCondition, firstDoor, secondDoor, out startDoor, out endDoor);

                float firstHeight = _coeffY == 0 ? 2 : 0;
                float secondHeight = _coeffY == 0 ? 2 : 0;

                if (firstDoor.Direction == DR.Top && secondDoor.Direction == DR.Top && _coeffY != 0)
                {
                    firstHeight = _coeffY == -1 ? 2 : _height + 2;
                    secondHeight = _coeffY == -1 ? _height + 2 : 2;
                }
                else if (firstDoor.Direction == DR.Bottom && secondDoor.Direction == DR.Bottom && _coeffY != 0)
                {
                    firstHeight = _coeffY == -1 ? _height + 2 : 2;
                    secondHeight = _coeffY == -1 ? 2 : _height + 2;
                }

                _coeffY = (firstDoor.Direction == DR.Top && secondDoor.Direction == DR.Top) ? 1 : -1;
                Shape shape = _coeffY == 1 ? Shape.ZigzagVertical : Shape.BracketsHorizontal;

                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    startPosition = startDoor.Cells[i].position;
                    startPosition.y += _coeffY;
                    lastPositionY = DrawByY(groundTilemap, wallsTilemap, startPosition, firstHeight, _coeffY, _coeffX, i, shape);
                }

                lastPositionY.y = _coeffY == -1 ? lastPositionY.y += 1 : lastPositionY.y;
                for (int i = 0; i < startDoor.Cells.Count; i++)
                {
                    _isFirstTile = i == 0 ? true : false;
                    lastPositionY.y -= i;
                    DrawByX(groundTilemap, wallsTilemap, lastPositionY, _width, _coeffX, true, _isFirstTile);
                }

                for (int i = 0; i < endDoor.Cells.Count; i++)
                {
                    startPosition = endDoor.Cells[i].position;
                    startPosition.y += _coeffY;
                    DrawByY(groundTilemap, wallsTilemap, startPosition, secondHeight, _coeffY, -_coeffX, i, shape);
                }
                #endregion
            }
        }

        private void CheckPosition(Vector2 position)
        {
            RaycastHit2D hit;
            hit = Physics2D.BoxCast(position, Vector2.one, 0, Vector2.zero, 0, CorridorGenerator.Instance.CorridorLayer);
            if (hit.collider != null)
            {
                CorridorGenerator.Instance.DestroyGrid(_grid.gameObject);
                return;
            }
        }

        private Vector2 DrawByY(Tilemap ground, Tilemap walls, Vector3 startPosition, float height, float coefY, float coefX, int tileNumber, Shape shape)
        {
            Vector2 lastPosition = Vector2.zero;
            Tile wallTile = null;
            Tile getTile;
            for (int block = 0; block < height; block++)
            {
                GetWallTile(shape, ref wallTile, out getTile, block, tileNumber, height, coefY, coefX);
                wallTile = getTile;
                Vector3 position = startPosition;
                Vector3Int tilePosition;
                position.y += (block * coefY);
                CheckPosition(position);
                tilePosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
                ground.SetTile(tilePosition, _tileData.Ground);
                walls.SetTile(tilePosition, wallTile);
                lastPosition = position;
            }
            return lastPosition;
        }

        private Vector2 DrawByX(Tilemap ground, Tilemap walls, Vector3 startPosition, float repeat, float coefX, bool firstCellHigher, bool isFirst)
        {
            Vector2 lastPosition = Vector2.zero;
            Tile wallTile = firstCellHigher && isFirst ? _tileData.WallTop : _tileData.WallBottom;
            for (int i = 1; i < repeat; i++)
            {
                Vector3 position = startPosition;
                Vector3Int tilePosition;
                position.x += (i * coefX);
                CheckPosition(position);
                tilePosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
                ground.SetTile(tilePosition, _tileData.Ground);
                walls.SetTile(tilePosition, wallTile);
                lastPosition = position;
            }
            return lastPosition;
        }

        private void GetWallTile(Shape shape, ref Tile takeTile, out Tile wallTile, int block, int tileNumber, float height, float coefY, float coefX)
        {
            wallTile = takeTile;
            if (shape != Shape.BracketsHorizontal || shape != Shape.StraightX && shape != Shape.ZigzagVertical)
            {
                if (tileNumber == 0 && block < 2 && block != 0)
                    wallTile = null;

                if (tileNumber == 0 && block == 2)
                    wallTile = coefX == 1 ? _tileData.WallLeft : _tileData.WallRight;
                else if (tileNumber == 1 && block == 1)
                    wallTile = coefX == 1 ? _tileData.WallRight : _tileData.WallLeft;
            }
            if (shape == Shape.ZigzagHorizontal || shape == Shape.Rectangular || shape == Shape.BracketsVertical)
            {
                if (block == 0)
                {
                    if (tileNumber == 0)
                        wallTile = coefY == 1 ? _tileData.WallBottom : _tileData.WallTop;
                    else if (tileNumber == 1)
                    {
                        if (coefX == 1)
                            wallTile = coefY == 1 ? _tileData.WallBottomRight : _tileData.WallTopRight;
                        else
                            wallTile = coefY == 1 ? _tileData.WallBottomLeft : _tileData.WallTopLeft;
                    }
                }
            }
            switch (shape)
            {
                case Shape.StraightY:
                    if (block == 0)
                    {
                        if (tileNumber == 0 && shape == Shape.StraightY)
                            wallTile = _tileData.WallBottom;
                        else if (tileNumber == 0 && shape == Shape.HalfZigzag)
                            wallTile = null;
                        if (tileNumber == 1)
                            wallTile = coefX == 1 ? _tileData.WallBottomRight : _tileData.WallBottomLeft;
                    }
                    break;

                case Shape.HalfZigzag:
                    goto case Shape.StraightY;

                case Shape.StraightX:
                    wallTile = tileNumber == 0 ? _tileData.WallLeft : _tileData.WallRight;
                    break;

                case Shape.ZigzagHorizontal:
                    if (tileNumber == 1 && block == height - 2)
                        wallTile = null;

                    if (tileNumber == 0 && block == height - 1)
                        wallTile = coefY == 1 ? _tileData.WallTopLeft : _tileData.WallBottomLeft;
                    else if (tileNumber == 1 && block == height - 1)
                        wallTile = coefY == 1 ? _tileData.WallTop : _tileData.WallBottom;
                    break;

                case Shape.ZigzagVertical:
                    wallTile = tileNumber == 0 ? _tileData.WallLeft : _tileData.WallRight;
                    if (coefX == 1)
                    {
                        if (tileNumber == 0 && block == height - 1)
                            wallTile = _tileData.WallTopLeft;
                        if (tileNumber == 1 && block == height - 1)
                            wallTile = _tileData.WallTop;
                        else if (tileNumber == 1 && block == height - 2)
                            wallTile = null;
                    }
                    else
                    {
                        if (tileNumber == 0 && block == height - 1)
                            wallTile = _tileData.WallTop;
                        if (tileNumber == 1 && block == height - 1)
                            wallTile = _tileData.WallTopRight;
                        else if (tileNumber == 0 && block == height - 2)
                            wallTile = null;
                    }
                    break;

                case Shape.BracketsVertical:
                    if (tileNumber == 0 && block == height - 2)
                        wallTile = null;

                    if (tileNumber == 0 && block == height - 1)
                        wallTile = coefY == 1 ? _tileData.WallTop : _tileData.WallBottom;
                    else if (tileNumber == 1 && block == height - 1)
                        wallTile = coefX == 1 ? _tileData.WallTopRight : _tileData.WallTopLeft;
                    break;

                case Shape.BracketsHorizontal:
                    wallTile = tileNumber == 0 ? _tileData.WallLeft : _tileData.WallRight;

                    if (block == height - 2)
                    {
                        if ((tileNumber == 1 && coefX == 1) || tileNumber == 0 && coefX == -1)
                            wallTile = null;
                    }
                    if (tileNumber == 0 && block == height - 1)
                        wallTile = coefX == 1 ? _tileData.WallBottomLeft : _tileData.WallBottom;
                    else if (tileNumber == 1 && block == height - 1)
                        wallTile = coefX == 1 ? _tileData.WallBottom : _tileData.WallBottomRight;
                    break;
            }
        }
    }
}
