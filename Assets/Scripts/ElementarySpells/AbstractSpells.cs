using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpells
{
    public Vector3 target { get; protected set; }

    public GameObject elementary { get; private set; }

    public AbstractSpells(GameObject elemRef, Vector3 target)
    {
        this.target = target;
        elementary = elemRef;
    }

    public abstract void FixedUpdate();

}
