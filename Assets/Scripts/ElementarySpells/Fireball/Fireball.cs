using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AbstractSpell
{

    /// <summary>
    /// The maximum distance before applying bullet drop
    /// </summary>
    public float maxDistance;

    /// <summary>
    /// Force needed to be applied when bullet drop is on
    /// </summary>
    [SerializeField][Range(-20, 0)] private float gravityForce;

    /// <summary>
    /// false if SetTarget is called and the spell target value updated
    /// </summary>
    private bool calculateDirection = true;

    public Fireball()
    {

    }
    /// <summary>
    /// Moves the fireball following the AbstractSpell target direction
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(calculateDirection)
        {

        }
    }

    /// <summary>
    /// Set the fireball target using a raycast
    /// </summary>
    private void SetTarget()
    {
        RaycastHit hit;
        //if(Physics.Raycast(transform.position, ))
    }

    private void ApplyBulletDrop()
    {

    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
    }

    protected override void onChargeEnd(float chargetime)
    {
        throw new System.NotImplementedException();
    }
}
