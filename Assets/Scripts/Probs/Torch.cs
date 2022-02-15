using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{

    [SerializeField] GameObject fire;

    private bool triggered = false;
    private ParticleSystem fireSystem;

    // Start is called before the first frame update
    private void Awake()
    {
        fireSystem = fire.GetComponent<ParticleSystem>();
        fireSystem.Stop();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        fireSystem.Play();
    }
}
