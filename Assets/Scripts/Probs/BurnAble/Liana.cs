using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Liana : BurnableItem
{
    protected override void Update()
    {
        if (triggered)
        {
            
        }
    }

    public override void Consume()
    {
        base.Consume();
        Debug.Log("Lianas on fire!");
    }
}
