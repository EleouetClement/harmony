using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurnableItem : MonoBehaviour
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
        Consume();
    }

    protected abstract void Update();

}
