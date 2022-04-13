using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFireBall : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject hugeExplosion;
    public bool hitted = false;
    public bool explosionOn = false;

    public GameObject objectHitted {get; private set;}

    private void OnCollisionEnter(Collision collision)
    {
        if(!hitted)
        {           
            if (explosionOn)
            {
                Instantiate(hugeExplosion, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            objectHitted = collision.gameObject;
            hitted = true;
        }      
    }
}
