using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gameManager : MonoBehaviour
{
    public enum GameStates { Start, End, Block, Running };
    public enum AlgorithmCategory { Astar, BFS };

    public static gameManager instance;
    public GameObject cubaPrefabs;
    private ShakeCamera shakeCam;
    public float camShakeTime = 0.1f;
    public ISearchAlgorithm algo;
    public AlgorithmCategory algoCate; 
    public GameStates state;

    [Header("Basic Settings")]
    public float animInterval;
    public Transform startPos;
    public int num;
    public float offset = 0.5f;

    public CubeAnimSettings settings;

    public Cube[,] cubes;
    public int[][] vis;
    public Queue<Coordinate> queue;
    public Coordinate startIdx;
    public Coordinate endIdx;

    private Vector3 pos;

    private void Awake()
    {
        instance = this;
        shakeCam = Camera.main.GetComponent<ShakeCamera>();
        pos = startPos.position;
    }

    void Start()
    {
        state = GameStates.Start;
        algoCate = AlgorithmCategory.Astar;
        CreateCubes();
        Initialize();
    }

    public void Initialize()
    {
        startIdx.SetXY(-1, -1);
        endIdx.SetXY(-1, -1);
        vis = new int[num][];
        for (int i = 0; i < num; i++)
        {
            vis[i] = new int[num];
        }
        queue = new Queue<Coordinate>();
        for (int i = 0; i < num; i++)
        {
            pos = startPos.position;
            pos += new Vector3(0, 0, 1) * offset * i;
            for (int j = 0; j < num; j++)
            {
                cubes[i, j].Clickable = true;
                cubes[i, j].mat.color = settings.originColor;
                cubes[i, j].gameObject.transform.position = pos;
                pos += Vector3.right * offset;
            }
        }
    }


    void CreateCubes()
    {
        cubes = new Cube[num, num];
        for (int i = 0; i < num; i++)
        {
            pos = startPos.position;
            pos += new Vector3(0, 0, 1) * offset * i;
            for (int j = 0; j < num; j++)
            {
                cubes[i, j] = Instantiate(cubaPrefabs, pos, Quaternion.identity).GetComponent<Cube>();
                cubes[i, j].coor.x = i;
                cubes[i, j].coor.y = j;
                pos += Vector3.right * offset;
            }
        }
    }

    public void SearchWay()
    {
        if (algoCate == AlgorithmCategory.Astar)
        {
            algo = new Astar(vis);
        }
        else if (algoCate == AlgorithmCategory.BFS)
        {
            algo = new BFS(vis);
        }
        algo.SearchWay(startIdx, endIdx);
        if (algo.isFind)
        {
            StartCoroutine(SearchAnim());
        }
        else
        {
            StartCoroutine(SearchFailedAnim());
        }
    }

    IEnumerator SearchFailedAnim()
    {
        shakeCam.shakeTime = camShakeTime;
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                if (cubes[i, j].Clickable == false)
                {
                    StartCoroutine(CubeAnim.Jump(cubes[i, j], settings.blockColor, settings.jumpHeight, settings.jumpHeight, settings.speed));
                }
            }
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                if (cubes[i, j].Clickable == true)
                {
                    StartCoroutine(CubeAnim.Jump(cubes[i, j], settings.blockColor, settings.jumpHeight + settings.upHeight, settings.jumpHeight, settings.speed));
                }
            }
        }
        yield break;
    }

    IEnumerator SearchAnim()
    {
        while (queue.Count > 0)
        {
            yield return new WaitForSeconds(animInterval);
            Coordinate temp = queue.Peek();
            if (!temp.Equals(endIdx))
            {
                StartCoroutine(CubeAnim.Jump(cubes[temp.x, temp.y], settings.runningColor, settings.upHeight + settings.jumpHeight, settings.jumpHeight, settings.speed));
            }
            queue.Dequeue();
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(BackTraceAnim());
    }

    IEnumerator BackTraceAnim()
    {
        List<Coordinate> way = algo.BackTrace(startIdx, endIdx);
        for (int i = 0; i < way.Count; i++)
        {
            yield return new WaitForSeconds(animInterval);
            StartCoroutine(CubeAnim.Jump(cubes[way[i].x, way[i].y], settings.endColor, settings.jumpHeight, settings.jumpHeight, settings.speed));
        }
    }
}
