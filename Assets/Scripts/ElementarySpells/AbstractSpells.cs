using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Define elementary behavior while casting an instance of this spell class
/// </summary>
public abstract class AbstractSpells
{
    
    public Vector3 target { get; protected set; }

    public GameObject elementary { get; private set; }

    /// <summary>
    /// define spell power
    /// <br></br>
    /// charge is between 0 and 100
    /// </summary>
    public float charge { get; private set; }

    public AbstractSpells(GameObject elemRef, Vector3 target, float charge)
    {
        this.charge = Mathf.Clamp(charge, 0, 100);
        this.target = target;
        elementary = elemRef;
    }
    
    public abstract void FixedUpdate();

}
