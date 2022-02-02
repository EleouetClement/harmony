using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMissiles : AbstractSpells
{
    private float timeLocale = 0f;
    private new Transform target;

    public WaterMissiles(GameObject elemRef, Vector3 target, float charge) : base(elemRef, target, charge)
    {
    }

    public WaterMissiles(GameObject elemRef, Transform target, float charge) : base(elemRef, Vector3.zero, charge)
    {
        this.target = target;
    }

    public override void FixedUpdate()
    {
        timeLocale += Time.fixedDeltaTime;
    }

    /// <returns>The destination position of the child missiles. </returns>
    private Vector3 getDestination() {
        if (target == null) return base.target;
        return target.position;
    }
}
