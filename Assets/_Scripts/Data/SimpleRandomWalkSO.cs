using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 简单随机游走SO
/// </summary>
[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    /// <summary>
    /// 迭代次数
    /// </summary>
    public int iterations = 10;
    /// <summary>
    /// 游走长度
    /// </summary>
    public int walkLenght = 10;
    /// <summary>
    /// 随机开始每次迭代
    /// </summary>
    public bool startRandomlyEachIteration = true;
}
