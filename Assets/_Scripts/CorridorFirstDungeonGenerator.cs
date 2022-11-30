using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 走廊优先地牢生成器
/// </summary>
public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    /// <summary>
    /// 走廊长度
    /// </summary>
    [SerializeField] private int corridorLenght = 14;
    /// <summary>
    /// 走廊数量
    /// </summary>
    [SerializeField] private int corridorCount = 5;
    /// <summary>
    /// 房间生成比例
    /// </summary>
    [SerializeField, Range(0.1f, 1)] private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    /// <summary>
    /// 走廊优先生成
    /// </summary>
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        floorPositions.UnionWith(roomPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// 根据潜在房间位置获得创建房间位置
    /// </summary>
    /// <param name="potentialRoomPositions">潜在房间位置</param>
    /// <returns></returns>
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();  // 房间位置
        // RoundToInt() 返回舍入为最近整数的值。
        // 获得要创建的房间数量
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        // OrderBy(x => Guid.NewGuid()) 根据键:x按升序对序列的元素进行排序。
        // NewGuid() 初始化 Guid 结构的新实例。表示全局唯一标识符 (GUID)。
        // Take() 从序列的开头返回指定数量的相邻元素。
        // ToList() 从 IEnumerable<T> 创建一个 List<T>。
        // 得到要创建的房间位置列表
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }
    /// <summary>
    /// 创建走廊且并集到地砖位置里
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    /// <param name="potentialRoomPositions">潜在房间位置</param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;    // 当前位置
        potentialRoomPositions.Add(currentPosition);

        // 每次循环都从走廊尽头开始
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLenght);
            currentPosition = corridor[corridor.Count - 1]; // 当前位置为走廊尽头
            potentialRoomPositions.Add(currentPosition);    // 将走廊尽头的位置加入潜在房间位置
            floorPositions.UnionWith(corridor); // 并入地砖位置
        }
    }
}
