using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMissiles : AbstractSpell
{
    public WaterBall BallPrefab;
    public int BallsToSpawn;

    private float timeLocale = 0f;
    [HideInInspector]
    public Transform targetTransform;

    public float maxSpellTime;

    private List<WaterBall> balls = new List<WaterBall>(20);
    private int spawnedballs = 0;

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        elementary.GetComponent<MeshRenderer>().enabled = false;
        while (spawnedballs <= BallsToSpawn)
        {
            spawnedballs++;
            WaterBall ball = Instantiate(BallPrefab, transform.position + randomVector(), Quaternion.identity);
            ball.parent = this;
            balls.Add(ball);
            ball.GetComponent<WaterBall>().targetLocation = getDestination();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Time locale
        timeLocale += Time.fixedDeltaTime;
        // Spell self destruction
        float castmax = maxSpellTime < 0.1f ? 1 : maxSpellTime;
        if (timeLocale > castmax || balls.Count <= 0)
        {
            Debug.Log("AbstractSpell ended");
            balls.ForEach(e => { Destroy(e.gameObject); });
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
        balls.ForEach(e => { e.launched = true; });
    }
}
