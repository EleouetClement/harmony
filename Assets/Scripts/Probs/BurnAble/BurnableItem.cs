using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurnableItem : MonoBehaviour, IDamageable
{

    protected ParticleSystem fireSystem;
    protected Light pointLight;
    protected Light spotLight;
    protected bool triggered = false;

    private void Awake()
    {
        fireSystem = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        pointLight = this.transform.GetChild(2).GetComponent<Light>();
        spotLight = this.transform.GetChild(3).GetComponent<Light>();
        fireSystem.Stop();
    }
    
    public virtual void Consume()
    {
        fireSystem.Play();
        triggered = true;
        if(pointLight && spotLight)
        {
            pointLight.enabled = true;
            spotLight.enabled = true;
        }
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
        if(hit.type.Equals(AbstractSpell.Element.Fire))
            Consume();
    }

    protected abstract void Update();

}
