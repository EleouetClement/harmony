using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public bool hitted { get; private set; } = false;

    public GameObject objectHitted { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
