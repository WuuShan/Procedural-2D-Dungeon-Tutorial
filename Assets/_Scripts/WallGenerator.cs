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
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    /// <summary>
    /// 创建对角线墙壁
    /// </summary>
    /// <param name="tilemapVisualizer">瓦片地图可视化</param>
    /// <param name="cornerWallPositions">对角线墙壁位置</param>
    /// <param name="floorPositions">地砖位置</param>
    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions,
        HashSet<Vector2Int> floorPositions)
    {
        // 判断每块墙壁对角线是否有地砖存在
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";   // 相邻墙壁以二进制类型存储
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition)) // 存在地砖则设为1
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    /// <summary>
    /// 创建基础墙壁
    /// </summary>
    /// <param name="tilemapVisualizer">瓦片地图可视化</param>
    /// <param name="cornerWallPositions">对角线墙壁位置</param>
    /// <param name="floorPositions">地砖位置</param>
    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions,
        HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
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
