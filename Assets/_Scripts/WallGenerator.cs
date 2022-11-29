using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 墙壁生成器
/// </summary>
public static class WallGenerator
{
    /// <summary>
    /// 创建墙壁
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    /// <param name="tilemapVisualizer">瓦片地图可视化</param>
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    /// <summary>
    /// 查找每块地砖周围是否有空位放墙壁
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    /// <param name="directionList">方向列表</param>
    /// <returns>墙壁位置</returns>
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();  // 地砖位置
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;   // 相邻位置
                if (!floorPositions.Contains(neighbourPosition))    // 如果相邻位置不包含地砖位置
                {
                    wallPositions.Add(neighbourPosition);   // 将相邻位置添加到地砖位置
                }
            }
        }
        return wallPositions;
    }
}
