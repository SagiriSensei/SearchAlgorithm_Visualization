using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Coordinate
{
    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsNull()
    {
        if (x == -1 && y == -1) return true;
        else return false;
    }
}
public class Cube : MonoBehaviour
{
    public Coordinate coor;
    public Transform transform;
    public Material mat;
    public bool Clickable = true;

    public CubeAnimSettings settings;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        mat = GetComponent<MeshRenderer>().material;
    }

    private void OnMouseOver()
    {
        if (Clickable && gameManager.instance.state != gameManager.GameStates.Running)
        {
            mat.color = settings.mouseOverColor;
        }
    }

    private void OnMouseExit()
    {
        if (Clickable && gameManager.instance.state != gameManager.GameStates.Running)
        {
            mat.color = settings.originColor;
        }
    }

    private void OnMouseDown()
    {
        if (Clickable)
        {
            if (gameManager.instance.state == gameManager.GameStates.Start && CubeAnim.animEnd)
            {
                StartCoroutine(CubeAnim.Up(this, settings.startColor, settings.upHeight, settings.speed));
                //Debug.Log(gameManager.instance.startIdx.x);
                if (!gameManager.instance.startIdx.IsNull())
                {
                    Cube tmp = gameManager.instance.cubes[gameManager.instance.startIdx.x, gameManager.instance.startIdx.y];
                    tmp.Clickable = true;
                    StartCoroutine(CubeAnim.Down(tmp, settings.originColor, settings.downHeight, settings.speed));
                }
                gameManager.instance.startIdx = coor;
                Clickable = false;
            }
            else if (gameManager.instance.state == gameManager.GameStates.End && CubeAnim.animEnd)
            {
                StartCoroutine(CubeAnim.Up(this, settings.endColor, settings.upHeight, settings.speed));
                if (!gameManager.instance.endIdx.IsNull())
                {
                    //gameManager.instance.queue.Dequeue();
                    Cube tmp = gameManager.instance.cubes[gameManager.instance.endIdx.x, gameManager.instance.endIdx.y];
                    tmp.Clickable = true;
                    StartCoroutine(CubeAnim.Down(tmp, settings.originColor, settings.downHeight, settings.speed));
                }
                gameManager.instance.endIdx = coor;
                Clickable = false;
            }
            else if (gameManager.instance.state == gameManager.GameStates.Block)
            {
                StartCoroutine(CubeAnim.Up(this, settings.blockColor, settings.upHeight, settings.speed));
                gameManager.instance.vis[coor.x][coor.y] = 1;
                Clickable = false;
                //Fixed:≤‚ ‘”√£¨ø……æ≥˝
                //gameManager.instance.queue.Enqueue(coor);
            }
        }
    }
}
