using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShrapnelBehavior : MonoBehaviour
{

    /// <summary>
    /// Grow time of the shrapnel, in seconds.
    /// </summary>
    public float growtime = 0.5f;
    /// <summary>
    /// Target size of the shrapnel, to which it will grow
    /// </summary>
    public float targetSize = 2f;

    private Vector3 targetposLocale;
    private Vector3 spawnPosLocale;

    private float timeLocale = 0f;

    void Start()
    {
        transform.localScale = Vector3.zero;

        targetposLocale = transform.localPosition;
        spawnPosLocale = targetposLocale * 3;
        transform.localPosition = spawnPosLocale;
    }

    void Update()
    {
        timeLocale += Time.deltaTime;
        if (timeLocale <= growtime)
        {
            float localsize = Easing.Quadratic.In(timeLocale / growtime) * targetSize;
            transform.localScale = new Vector3(localsize, localsize, localsize);

            float timeratio = Easing.Cubic.In(timeLocale / growtime);
            float posX = targetposLocale.x * timeratio + spawnPosLocale.x * (1 - timeratio);
            float posY = targetposLocale.y * timeratio + spawnPosLocale.y * (1 - timeratio);
            float posZ = targetposLocale.z * timeratio + spawnPosLocale.z * (1 - timeratio);

            transform.localPosition = new Vector3(posX, posY, posZ);
        }
    }
}
