using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 简单随机游走地牢生成器
/// </summary>
public class SimpleRandomWalkDungeonGenerator : MonoBehaviour
{
    /// <summary>
    /// 起始位置
    /// </summary>
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    /// <summary>
    /// 迭代次数
    /// </summary>
    [SerializeField] private int iterations = 10;
    /// <summary>
    /// 游走长度
    /// </summary>
    [SerializeField] public int walkLenght = 10;
    /// <summary>
    /// 随机开始每次迭代
    /// </summary>
    [SerializeField] public bool startRandomlyEachIteration = true;

    /// <summary>
    /// 运行程序生成
    /// </summary>
    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();   // 地砖位置
        foreach (var position in floorPositions)
        {
            Debug.Log(position);
        }
    }

    /// <summary>
    /// 运行随机游走
    /// </summary>
    /// <returns>地砖位置</returns>
    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;    // 当前位置
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 地砖位置
        // 根据迭代次数生成地砖位置
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLenght);
            // UnionWith() 修改当前 HashSet<T> 对象以包含存在于该对象中、指定集合中或两者中的所有元素。
            // 去除重复的地砖位置
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
            {
                // ElementAt() 返回序列中指定索引处的元素。
                // 从地砖位置中随机选择一个位置当做当前位置
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
