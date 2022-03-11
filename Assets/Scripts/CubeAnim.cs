using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnim : MonoBehaviour
{
    //判断全部动画是否结束
    public static bool animEnd = true;

    public void CubeUp(Cube cube, Color color, float height, float speed)
    {
        cube.mat.color = color;
        StartCoroutine(Up(cube, color, height, speed));
    }

    public static IEnumerator Up(Cube cube, Color color, float height, float speed)
    {
        animEnd = false;
        Vector3 startPos = cube.transform.position;
        Vector3 endPos = startPos + new Vector3(0, height, 0);
        cube.mat.color = color;
        float curHeight = 0;
        while (curHeight < height)
        {
            curHeight += Time.deltaTime * speed;
            cube.transform.position = Vector3.Lerp(startPos, endPos, curHeight / height);
            yield return null;
        }
        animEnd = true;
        yield break;
    }

    public static IEnumerator Down(Cube cube, Color color, float height, float speed)
    {
        animEnd = false;
        Vector3 startPos = cube.transform.position;
        Vector3 endPos = startPos - new Vector3(0, height, 0);
        cube.mat.color = color;
        float curHeight = height;
        while (curHeight >= 0)
        {
            curHeight -= Time.deltaTime * speed;
            cube.transform.position = Vector3.Lerp(endPos, startPos, curHeight / height);
            yield return null;
        }
        animEnd = true;
        yield break;
    }

    public static IEnumerator Jump(Cube cube, Color color, float upHeight, float downHeight, float speed)
    {
        yield return Up(cube, color, upHeight, speed);
        yield return Down(cube, color, downHeight, speed);
    }
}
