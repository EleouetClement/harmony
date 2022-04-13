using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private GameObject BigHugeMassiveExplosionOfTheDeath;

    public bool hitted { get; private set; } = false;

    public GameObject objectHitted { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hitted)
        {
            hitted = true;
            objectHitted = collision.gameObject;
            Instantiate(BigHugeMassiveExplosionOfTheDeath, transform.position, Quaternion.identity);
        }
    }
}
