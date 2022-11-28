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
    /// 地砖瓦片
    /// </summary>
    [SerializeField] private TileBase floorTile;

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
    /// 清除地砖瓦片地图的全部瓦片
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
