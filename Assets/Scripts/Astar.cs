using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : ISearchAlgorithm
{
    private Hashtable preCube = new Hashtable();
    private int[][] vis;
    private int[][] costs;

    public Astar(int[][] vis)
    {
        this.vis = vis;
        costs = new int[vis.Length][];
        for (int i = 0; i < costs.Length; i++)
        {
            costs[i] = new int[vis[0].Length];
        }
    }

    //内部类，小顶堆实现优先队列
    public class MinHeap
    {
        private List<int> costs;
        private List<Coordinate> coors;
        private int size = 0;

        public MinHeap()
        {
            costs = new List<int>();
            costs.Add(0);
            coors = new List<Coordinate>();
            coors.Add(new Coordinate(0, 0));
        }

        public int Size { get { return size; } }

        private void Swap(int idx1, int idx2)
        {
            int iTmp = costs[idx1];
            Coordinate coorTmp = coors[idx1];
            costs[idx1] = costs[idx2];
            coors[idx1] = coors[idx2];
            costs[idx2] = iTmp;
            coors[idx2] = coorTmp;
        }

        private void Up(int i)
        {
            int parent = i / 2;
            if (parent >= 1 && costs[parent] > costs[i])
            {
                Swap(parent, i);
                Up(parent);
            }
        }
        private void Down(int i)
        {
            int leftChild = 2 * i;
            int rightChild = 2 * i + 1;
            int minIdx = i;
            if (leftChild <= Size && costs[leftChild] < costs[i]) minIdx = leftChild;
            if (rightChild <= Size && costs[rightChild] < costs[i]) minIdx = rightChild;
            if (minIdx != i)
            {
                Swap(minIdx, i);
                Down(minIdx);
            }
        }
        
        public void Add(int cost, Coordinate coor)
        {
            size++;
            costs.Add(cost);
            coors.Add(coor);
            Up(Size);
        }

        public void RemoveFirst()
        {
            if (Size >= 1)
            {
                Swap(1, Size);
                costs.RemoveAt(Size);
                coors.RemoveAt(Size);
                size--;
                Down(1);
            }
        }

        public Coordinate First()
        {
            return coors[1];
        }
    }


    //函数


    private int ManDistance(Coordinate coor1, Coordinate coor2)
    {
        //Fixed:代价计算错误
        return Mathf.Abs(coor1.x - coor2.x) + Mathf.Abs(coor1.y - coor2.y);
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
        MinHeap heap = new MinHeap();
        heap.Add(ManDistance(start, end), start);
        int[,] costSoFar = new int[vis.Length, vis[0].Length];
        costSoFar[start.x, start.y] = 0;
        while (heap.Size > 0)
        {
            Coordinate temp = heap.First();
            heap.RemoveFirst();
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
                    costSoFar[neighber.x, neighber.y] = costSoFar[temp.x, temp.y] + 1;
                    int H = ManDistance(neighber, end);
                    int F = costSoFar[neighber.x, neighber.y] + H;
                    preCube.Add(neighber, temp);
                    vis[neighber.x][neighber.y] = 1;
                    heap.Add(F, neighber);
                    //将加入小顶堆的cube放入gameManager中的队列，之后显示动画
                    gameManager.instance.queue.Enqueue(neighber);
                }
            }
        }
    }
}
