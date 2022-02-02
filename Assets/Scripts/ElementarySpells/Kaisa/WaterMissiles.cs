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

    private List<GameObject> balls = new List<GameObject>(20);
    private int spawnedballs = 0;

    public override void FixedUpdate()
    {
        timeLocale += Time.fixedDeltaTime;
        if (spawnedballs <= BallsToSpawn) {
            spawnedballs++;
            balls.Add(Instantiate(BallPrefab));
        }

        float castmax = castingTime < 0.1f ? 2 : castingTime;
        if (timeLocale > castmax) {
            balls.ForEach(e => { Destroy(e); });

            elementary.GetComponent<ElementaryController>().currentSpell = null;
            elementary.transform.position = getDestination();
            elementary.GetComponent<ElementaryController>().computePosition = true;

            Destroy(gameObject);
        }
       
    }

    /// <returns>The destination position of the child missiles. </returns>
    private Vector3 getDestination()
    {
        if (targetTransform == null) return base.target;
        return targetTransform.position;
    }

}
