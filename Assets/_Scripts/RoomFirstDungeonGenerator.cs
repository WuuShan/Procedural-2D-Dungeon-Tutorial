using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    /// <summary>
    /// 根据房间中心点列表用走廊连接各个房间
    /// </summary>
    /// <param name="roomCenters">房间中心点列表</param>
    /// <returns>走廊位置</returns>
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];    // 当前房间中心点
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    /// <summary>
    /// 在当前房间中心点和目标房间中心点之间创建走廊
    /// </summary>
    /// <param name="currentRoomCenter">当前房间中心点</param>
    /// <param name="destination">目标房间中心点</param>
    /// <returns>走廊位置</returns>
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    /// <summary>
    /// 查找距离当前房间中心点最近的房间中心点
    /// </summary>
    /// <param name="currentRoomCenter">当前房间中心点</param>
    /// <param name="roomCenters">房间中心点列表</param>
    /// <returns>最近房间中心点</returns>
    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;   // 最近房间中心点
        float distance = float.MaxValue;        // 最近房间中心点距离
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);  // 当前房间中心点距离
            // 如果 当前房间中心点距离 小于 最近房间中心点距离
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
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
