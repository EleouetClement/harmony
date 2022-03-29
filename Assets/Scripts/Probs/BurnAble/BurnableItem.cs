using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurnableItem : MonoBehaviour, IDamageable
{

    protected ParticleSystem fireSystem;
    protected bool triggered = false;

    private void Awake()
    {
        fireSystem = GetComponent<ParticleSystem>();
        fireSystem.Stop();
    }
    
    public virtual void Consume()
    {
        fireSystem.Play();
        triggered = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(HarmonyLayers.LAYER_PLAYERSPELL) && GameModeSingleton.GetInstance().GetElementaryReference.GetComponent<ElementaryController>().currentElement == AbstractSpell.Element.Fire)
        {
            Consume();
        }
    }

    public void OnDamage(DamageHit hit)
    {
        Consume();
    }

    protected abstract void Update();

    
}
