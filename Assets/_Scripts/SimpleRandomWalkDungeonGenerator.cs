using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 简单随机游走地牢生成器
/// </summary>
public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    /// <summary>
    /// 随机游走数据
    /// </summary>
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters);   // 地砖位置
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// 运行随机游走
    /// </summary>
    /// <param name="parameters">随机游走数据</param>
    /// <returns>地砖位置</returns>
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters)
    {
        var currentPosition = startPosition;    // 当前位置
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 地砖位置
        // 根据迭代次数生成地砖位置
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLenght);
            // UnionWith() 修改当前 HashSet<T> 对象以包含存在于该对象中、指定集合中或两者中的所有元素。
            // 去除重复的地砖位置
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                // ElementAt() 返回序列中指定索引处的元素。
                // 从地砖位置中随机选择一个位置当做当前位置
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
