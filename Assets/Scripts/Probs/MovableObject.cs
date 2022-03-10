using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IDamageable
{
    public float movingSpeed;

    public void OnDamage(DamageHit hit)
    {
        float move = movingSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transform.position + hit.direction, move);
    }
}
