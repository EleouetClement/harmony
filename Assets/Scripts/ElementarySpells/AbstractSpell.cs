using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define elementary behavior while casting an instance of this spell class
/// </summary>
public abstract class AbstractSpell : MonoBehaviour
{
    
    public Vector3 target { get; protected set; }

    public GameObject elementary { get; protected set; }

    /// <summary>
    /// define spell power
    /// <br>
    /// charge is between 0 and 100
    /// </summary>
    public float charge { get; protected set; }
    public abstract void FixedUpdate();

    /// <summary>
    /// Common initializer for all player cast spells.
    /// </summary>
    /// <param name="elemRef">Reference to the elementaryControler instance</param>
    /// <param name="target">Hit to the target of the spell. Might or might not be used, and usually determines a direction</param>
    /// <param name="charge">The charge scale of the spell at cast, scalar from 0f to 100f</param>
    public void init(GameObject elemRef, Vector3 target, float charge) {
        this.charge = Mathf.Clamp(charge, 0, 100);
        this.target = target;
        elementary = elemRef;
    }

}
