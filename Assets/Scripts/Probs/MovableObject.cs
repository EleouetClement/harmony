using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IDamageable
{
    public float movingSpeed;
    //public float forceToApply;

    //private Rigidbody rigidBody;

    private void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
    }

    public void OnDamage(DamageHit hit)
    {
        float move = movingSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transform.position + hit.direction, move);
        //rigidBody.AddForce(hit.direction * forceToApply);
    }
}
