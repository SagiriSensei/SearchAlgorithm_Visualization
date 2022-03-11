using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform camTrans;

    public float shakeTime = 0f;
    public float shakeAmount = 0.1f;
    public float shakeInterval = 0.2f;
    public float decreaseFactor = 1f;

    private float interval = 0;

    Vector3 originalPos;

    private void Awake()
    {
        camTrans = GetComponent<Transform>();
    }

    private void Start()
    {
        originalPos = camTrans.position;
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            if (interval <= 0)
            {
                camTrans.position = originalPos + Random.insideUnitSphere * shakeAmount;
                interval = shakeInterval;
            }
            interval -= Time.deltaTime;
            shakeTime -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            camTrans.position = originalPos;
        }
    }
}
