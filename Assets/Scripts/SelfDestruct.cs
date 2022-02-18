using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    public float LivingTime = 1.5f;

    private float timeLocale = 0f;

    void Update()
    {
        timeLocale += Time.deltaTime;

        if (timeLocale >= LivingTime) {
            Destroy(gameObject);
        }
    }
}
