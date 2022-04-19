using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreakableItem : MonoBehaviour, IDamageable
{


    private void Awake()
    {

    }
    
    public virtual void Break()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(HarmonyLayers.LAYER_PLAYERSPELL))
        {
            Break();
        }
    }

    public void OnDamage(DamageHit hit)
    {
        Break();
    }

    protected abstract void Update();

}
