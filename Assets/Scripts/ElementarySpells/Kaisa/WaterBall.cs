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

    /// <summary>
    /// True if the ball is launched and seeking the target, false if it's precast hover mode.
    /// </summary>
    public bool launched = false;

    /// <summary>
    /// Time since launch
    /// </summary>
    private float TimeLocale;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (launched)
        {
            TimeLocale += Time.deltaTime;
            transform.position = Vector3.Lerp(spawnLocation, targetLocation, Mathf.Clamp(TimeLocale / traveltime, 0, 1));
        }
    }
}
