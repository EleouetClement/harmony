using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFireBall : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    public bool hitted = false;
    public bool explosionOn = false;
    GameObject explosionInstance;

    public GameObject objectHitted {get; private set;}

    private void OnCollisionEnter(Collision collision)
    {
        if (explosionOn)
            explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        objectHitted = collision.gameObject;
        hitted = true;
    }
}
