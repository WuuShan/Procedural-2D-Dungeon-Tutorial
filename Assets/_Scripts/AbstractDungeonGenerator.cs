using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽象地牢生成
/// </summary>
public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    /// <summary>
    /// 瓦片地图可视化
    /// </summary>
    [SerializeField] protected TilemapVisualizer tilemapVisualizer = null;
    /// <summary>
    /// 起始位置
    /// </summary>
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    /// <summary>
    /// 生成地牢
    /// </summary>
    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    /// <summary>
    /// 运行程序生成
    /// </summary>
    protected abstract void RunProceduralGeneration();
}
