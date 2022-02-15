using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private bool triggered = false;
    private ParticleSystem fireSystem;

    // Start is called before the first frame update
    private void Awake()
    {
        fireSystem = GetComponent<ParticleSystem>();
        fireSystem.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        fireSystem.Play();
    }
}
