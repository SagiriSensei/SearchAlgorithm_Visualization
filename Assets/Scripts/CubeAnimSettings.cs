using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CubeAnimSettings", menuName = "Settings/CubeAnimSettings")]
public class CubeAnimSettings : ScriptableObject
{
    public float speed;
    public float upHeight;
    public float downHeight;
    public float jumpHeight;
    public Color startColor;
    public Color endColor;
    public Color blockColor;
    public Color runningColor;
    public Color originColor;
    public Color mouseOverColor;
}
