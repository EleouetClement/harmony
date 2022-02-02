using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMissiles : AbstractSpell
{
    public GameObject BallPrefab;
    public int BallsToSpawn;

    private float timeLocale = 0f;
    [HideInInspector]
    public Transform targetTransform;
    [HideInInspector]
    public float castingTime;

    private List<GameObject> balls;
    private int spawnedballs = 0;

    public override void FixedUpdate()
    {
        timeLocale += Time.fixedDeltaTime;
        if (spawnedballs <= BallsToSpawn) {
            spawnedballs++;
            Instantiate(BallPrefab);
        }
    }

    /// <returns>The destination position of the child missiles. </returns>
    private Vector3 getDestination()
    {
        if (target == null) return base.target;
        return targetTransform.position;
    }

}
