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
    public float bezierLaunchmulti = 4f;

    [HideInInspector]
    public Vector3 targetLocation;
    private Vector3 spawnLocation;
    private Vector3 hoverLocation;

    private Vector3 bezierpointStart;
    private Vector3 bezierPointEnd;

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
        Vector3 roffset = randomVector();
        hoverLocation = spawnLocation + roffset;
        bezierpointStart = hoverLocation + (roffset * bezierLaunchmulti);
        bezierPointEnd = Vector3.Lerp(spawnLocation, targetLocation, 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        TimeLocale += Time.deltaTime;
        if (launched)
        {
            TimeLaunch += Time.deltaTime;
            //transform.position = Vector3.Lerp(hoverLocation, targetLocation, Mathf.Clamp(TimeLaunch / traveltime, 0, 1));
            transform.position = BezierInterpolate(Mathf.Clamp(TimeLaunch / traveltime, 0, 1), spawnLocation, bezierpointStart, targetLocation, bezierPointEnd);
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

    private Vector3 BezierInterpolate(float t, Vector3 posStart, Vector3 startOffsetPos, Vector3 posEnd, Vector3 endOffsetPos)
    {
        t = Mathf.Clamp01(t);
        float invT = 1f - t;
        return
            invT * invT * invT * posStart +
            3f * invT * invT * t * startOffsetPos +
            3f * invT * t * t * endOffsetPos +
            t * t * t * posEnd;
    }

}
