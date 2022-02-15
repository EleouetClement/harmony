using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : BurnableItem
{
    

    // Start is called before the first frame update


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == HarmonyLayers.LAYER_PLAYERSPELL)
    //    {
    //        Consume();
    //    }      
    //}

    public override void Consume()
    {
        base.Consume();
        Debug.Log("Torche lighted On!");
    }
}
