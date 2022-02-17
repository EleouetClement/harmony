using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Torch : BurnableItem
{

    protected override void Update()
    {
        if(triggered)
        {
            
        }
    }

    public override void Consume()
    {
        base.Consume();
        Debug.Log("Torche lighted On!");
    }
}
