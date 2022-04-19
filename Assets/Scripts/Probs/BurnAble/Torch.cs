using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Torch : BurnableItem
{
    public bool ignited;


    public void Start()
    {
        if (ignited == true)
        {
            base.Consume();
        }
    }

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
