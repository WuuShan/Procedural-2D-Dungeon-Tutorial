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

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// 在死胡同处创建房间
    /// </summary>
    /// <param name="deadEnds">死胡同列表</param>
    /// <param name="roomFloors">房间地砖</param>
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (!roomFloors.Contains(position)) // 如果死胡同的位置不含于房间地砖
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    /// <summary>
    /// 根据地砖位置查找全部死胡同
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    /// <returns></returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;    // 当前地砖的相邻地砖数量
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                if (floorPositions.Contains(position + direction))  // 判断当前地砖的四面方向中是否包含地砖
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)   // 如果当前地砖的相邻地砖只有一个
            {
                deadEnds.Add(position); // 加入死胡同列表
            }
        }
        return deadEnds;
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
