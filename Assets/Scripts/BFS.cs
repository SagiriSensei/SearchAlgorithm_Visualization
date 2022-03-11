using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : ISearchAlgorithm
{
    private Hashtable preCube = new Hashtable();
    private int[][] vis;

    public BFS(int[][] vis)
    {
        this.vis = vis;
    }

    private List<Coordinate> Neighbers(Coordinate coor)
    {
        List<Coordinate> neighbers = new List<Coordinate>();
        int row = vis.Length;
        int col = vis[0].Length;
        if (coor.x - 1 >= 0 && vis[coor.x - 1][coor.y] == 0) neighbers.Add(new Coordinate(coor.x - 1, coor.y));
        if (coor.x + 1 < row && vis[coor.x + 1][coor.y] == 0) neighbers.Add(new Coordinate(coor.x + 1, coor.y));
        if (coor.y - 1 >= 0 && vis[coor.x][coor.y - 1] == 0) neighbers.Add(new Coordinate(coor.x, coor.y - 1));
        if (coor.y + 1 < col && vis[coor.x][coor.y + 1] == 0) neighbers.Add(new Coordinate(coor.x, coor.y + 1));
        return neighbers;
    }

    public override List<Coordinate> BackTrace(Coordinate start, Coordinate end)
    {
        List<Coordinate> pre = new List<Coordinate>();
        Coordinate cur = end;
        while (!cur.Equals(start))
        {
            cur = (Coordinate)preCube[cur];
            pre.Add(cur);
        }
        return pre;
    }

    public override void SearchWay(Coordinate start, Coordinate end)
    {
        Queue<Coordinate> queue = new Queue<Coordinate>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            Coordinate temp = queue.Peek();
            queue.Dequeue();
            if (temp.Equals(end))
            {
                isFind = true;
                break;
            }
            else
            {
                List<Coordinate> neighbers = Neighbers(temp);
                foreach (var neighber in neighbers)
                {
                    preCube.Add(neighber, temp);
                    vis[neighber.x][neighber.y] = 1;
                    gameManager.instance.cubes[neighber.x, neighber.y].Clickable = false;
                    queue.Enqueue(neighber);
                    //将加入队列的cube放入gameManager中的队列，之后显示动画
                    gameManager.instance.queue.Enqueue(neighber);
                }
            }
        }
    }


}
