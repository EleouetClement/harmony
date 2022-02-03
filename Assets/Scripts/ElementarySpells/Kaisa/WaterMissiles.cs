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

    public float maxSpellTime;

    private List<GameObject> balls = new List<GameObject>(20);
    private int spawnedballs = 0;
    private bool initialized = false;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        timeLocale += Time.fixedDeltaTime;
        // Spell inititalisation in scene
        if (!initialized)
        {
            elementary.GetComponent<MeshRenderer>().enabled = false;
            while (spawnedballs <= BallsToSpawn)
            {
                spawnedballs++;
                GameObject ball = Instantiate(BallPrefab, transform.position + randomVector(), Quaternion.identity);
                balls.Add(ball);
                ball.GetComponent<WaterBall>().targetLocation = getDestination();
            }
        }
        // Spell self destruction
        float castmax = maxSpellTime < 0.1f ? 1 : maxSpellTime;
        if (timeLocale > castmax || balls.Count <= 0)
        {
            balls.ForEach(e => { Destroy(e); });
            elementary.GetComponent<ElementaryController>().currentSpell = null;
            elementary.transform.position = getDestination();
            elementary.GetComponent<ElementaryController>().computePosition = true;
            elementary.GetComponent<MeshRenderer>().enabled = true;
            Destroy(gameObject);
        }

    }

    /// <returns>The destination position of the child missiles. </returns>
    private Vector3 getDestination()
    {
        if (targetTransform == null) return base.target;
        return targetTransform.position;
    }

    private Vector3 randomVector()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    protected override void onChargeEnd(float chargetime)
    {
        // Does nothing yet
    }
}
