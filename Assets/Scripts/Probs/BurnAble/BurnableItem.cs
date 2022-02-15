using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurnableItem : MonoBehaviour
{

    protected ParticleSystem fireSystem;
    private bool triggered = false;

    private void Awake()
    {
        fireSystem = GetComponent<ParticleSystem>();
        fireSystem.Stop();
    }
    
    public virtual void Consume()
    {
        fireSystem.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Consume();
    }

}
