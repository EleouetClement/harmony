using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior on the individual water balls created by a waterMissile Script
/// </summary>
public class WaterBall : MonoBehaviour
{

    public float traveltime = 1f;
    [HideInInspector]
    public Vector3 targetLocation;
    private Vector3 spawnLocation;

    private float TimeLocale;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        TimeLocale += Time.deltaTime;
        transform.position = Vector3.Lerp(spawnLocation, targetLocation, Mathf.Clamp(TimeLocale / traveltime, 0, 1));
    }
}
