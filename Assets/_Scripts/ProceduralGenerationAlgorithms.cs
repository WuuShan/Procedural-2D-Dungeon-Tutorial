using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地牢程序化生成算法
/// </summary>
public static class ProceduralGenerationAlgorithms
{
    /// <summary>
    /// 通过简单随机游走算法生成路径
    /// </summary>
    /// <param name="startPosition">起始位置</param>
    /// <param name="walkLenght">游走长度</param>
    /// <returns>游走路径</returns>
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();   // 游走路径

        path.Add(startPosition);
        var previousPosition = startPosition;   // 上个位置

        // 根据游走长度随机生成一条路径
        for (int i = 0; i < walkLenght; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    /// <summary>
    /// 随机游走走廊
    /// </summary>
    /// <param name="startPosition">起始位置</param>
    /// <param name="corridorLenght">走廊长度</param>
    /// <returns>走廊列表</returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLenght)
    {
        List<Vector2Int> corridor = new List<Vector2Int>(); // 走廊列表
        var direction = Direction2D.GetRandomCardinalDirection();   // 获得一个方向向量
        var currentPosition = startPosition;    // 当前位置
        corridor.Add(currentPosition);

        // 往一个方向一直移动并添加到走廊列表
        for (int i = 0; i < corridorLenght; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }

    /// <summary>
    /// 二叉空间划分
    /// </summary>
    /// <param name="spaceToSplit">要划分的空间</param>
    /// <param name="minWidth">最小宽度</param>
    /// <param name="minHeight">最小高度</param>
    /// <returns>房间列表</returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();   // 房间队列
        List<BoundsInt> roomsList = new List<BoundsInt>();      // 房间列表
        roomsQueue.Enqueue(spaceToSplit);   // 将要划分的空间入队
        while (roomsQueue.Count > 0)    // 判断队里还有没有人
        {
            var room = roomsQueue.Dequeue();    // 拿到第一个队员
            if (room.size.y >= minHeight && room.size.x >= minWidth)    // 判断房间的大小是否大于等于最小空间
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)   // 房间高度大于两倍的最小高度
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)   // 房间宽度大于两倍的最小宽度
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVertically(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        throw new System.NotImplementedException();
    }

    private static void SplitHorizontally(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// 路径方向
/// </summary>
public static class Direction2D
{
    /// <summary>
    /// 基本方向列表
    /// </summary>
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1),    // UP
        new Vector2Int(1,0),    // RIGHT
        new Vector2Int(0,-1),   // DOWN
        new Vector2Int(-1,0)    // LEFT
    };

    /// <summary>
    /// 获得随机的基本方向向量
    /// </summary>
    /// <returns>基本方向向量</returns>
    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
