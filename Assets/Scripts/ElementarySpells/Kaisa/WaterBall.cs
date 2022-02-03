using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior on the individual water balls created by a waterMissile Script
/// </summary>
public class WaterBall : MonoBehaviour
{
    [HideInInspector]
    public WaterMissiles parent;

    public float setuptime = 0.5f;
    public float traveltime = 1f;

    [HideInInspector]
    public Vector3 targetLocation;
    private Vector3 spawnLocation;
    private Vector3 hoverLocation;

    /// <summary>
    /// True if the ball is launched and seeking the target, false if it's precast hover mode.
    /// </summary>
    [HideInInspector]
    public bool launched = false;

    /// <summary>
    /// Time since the object has been created
    /// </summary>
    private float TimeLocale;
    /// <summary>
    /// Time since the object has been launched
    /// </summary>
    private float TimeLaunch;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.position;
        hoverLocation = spawnLocation + randomVector();
    }

    // Update is called once per frame
    void Update()
    {
        TimeLocale += Time.deltaTime;
        if (launched)
        {
            TimeLaunch += Time.deltaTime;
            transform.position = Vector3.Lerp(hoverLocation, targetLocation, Mathf.Clamp(TimeLaunch / traveltime, 0, 1));
        }
        else
        {
            transform.position = Vector3.Lerp(spawnLocation, hoverLocation, Mathf.Clamp(TimeLocale / setuptime, 0, 1));
        }
    }

    private void FixedUpdate()
    {
        if (parent.gameObject == null)
            Destroy(gameObject);
    }
    private Vector3 randomVector()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

}
