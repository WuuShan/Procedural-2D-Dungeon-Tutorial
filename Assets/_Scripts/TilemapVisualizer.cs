using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 瓦片地图可视化
/// </summary>
public class TilemapVisualizer : MonoBehaviour
{
    /// <summary>
    /// 地砖瓦片地图
    /// </summary>
    [SerializeField] private Tilemap floorTilemap;
    /// <summary>
    /// 墙壁瓦片地图
    /// </summary>
    [SerializeField] private Tilemap wallTilemap;
    /// <summary>
    /// 地砖瓦片
    /// </summary>
    [SerializeField] private TileBase floorTile;
    /// <summary>
    /// 墙壁瓦片
    /// </summary>
    [SerializeField] private TileBase wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull;

    /// <summary>
    /// 根据地砖位置在瓦片地图中绘制地砖瓦片
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    /// <summary>
    /// 根据位置在瓦片地图中绘制瓦片
    /// </summary>
    /// <param name="positions">位置</param>
    /// <param name="tilemap">瓦片地图</param>
    /// <param name="tile">瓦片</param>
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    /// <summary>
    /// 根据墙壁类型并绘制简单基础墙壁
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="binaryType">墙壁类型</param>
    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSiderLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

    /// <summary>
    /// 根据位置在瓦片地图中绘制单个瓦片
    /// </summary>
    /// <param name="tilemap">瓦片地图</param>
    /// <param name="tile">瓦片</param>
    /// <param name="position">位置</param>
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        // WorldToCell() 将世界位置转换为单元格位置。
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    /// <summary>
    /// 清除瓦片地图的全部瓦片
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {

    }
}
