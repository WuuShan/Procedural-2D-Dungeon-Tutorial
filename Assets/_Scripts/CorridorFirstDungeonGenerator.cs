using System;
using System.Collections;
using System.Collections.Generic;
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
    /// 房间生成占比
    /// </summary>
    [SerializeField, Range(0.1f, 1)] private float roomPercent = 0.8f;
    /// <summary>
    /// 房间生成数据
    /// </summary>
    [SerializeField] public SimpleRandomWalkSO roomGenerationParameters;

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

        CreateCorridors(floorPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// 创建走廊且并集到地砖位置里
    /// </summary>
    /// <param name="floorPositions">地砖位置</param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions)
    {
        var currentPosition = startPosition;    // 当前位置

        // 每次循环都从走廊尽头开始
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLenght);
            currentPosition = corridor[corridor.Count - 1]; // 当前位置为走廊尽头
            floorPositions.UnionWith(corridor); // 并入地砖位置
        }
    }
}
