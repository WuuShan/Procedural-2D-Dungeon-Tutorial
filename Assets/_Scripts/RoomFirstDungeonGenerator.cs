using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间优先地牢生成器
/// </summary>
public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    /// <summary>
    /// 最小房间宽度
    /// </summary>
    [SerializeField] private int minRoomWidth = 4;
    /// <summary>
    /// 最小房间高度
    /// </summary>
    [SerializeField] private int minRoomHeight = 4;
    /// <summary>
    /// 地牢宽度
    /// </summary>
    [SerializeField] private int dungeonWidth = 20;
    /// <summary>
    /// 地牢高度
    /// </summary>
    [SerializeField] private int dungeonHeight = 20;
    /// <summary>
    /// 房间的外围空间大小
    /// </summary>
    [SerializeField, Range(0, 10)] private int offset = 1;
    /// <summary>
    /// 随机游走房间
    /// </summary>
    [SerializeField] private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    /// <summary>
    /// 根据房间列表创建简单房间
    /// </summary>
    /// <param name="roomsList">房间列表</param>
    /// <returns>地砖位置</returns>
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();  // 地砖位置
        foreach (var room in roomsList)
        {
            // 房间的大小 - 房间的外围空间大小 得到 房间地砖位置
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
