using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IDamageable
{
    public float movingSpeed;
    private bool isMoving = false;
    private bool hasMoved = false; // To know if this gameObject was pushed at the previous frame
    private bool colorChanged = false;
    private Vector3 currentPosition;
    private Vector3 previousPosition;

    private void Start()
    {
        currentPosition = transform.position;
        previousPosition = currentPosition;
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public void OnDamage(DamageHit hit)
    {
        previousPosition = currentPosition;
        isMoving = true;

        if (!colorChanged)
        {
            colorChanged = true;
            GetComponent<MeshRenderer>().material.color = Color.green;
            Debug.Log("GREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEN");
        }

        float move = movingSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transform.position + hit.direction, move);
        currentPosition = transform.position;
        isMoving = false;
    }

    private void Update()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;

        if (currentPosition == previousPosition && !isMoving && colorChanged)
        {
            colorChanged = false;
            GetComponent<MeshRenderer>().material.color = Color.blue;
            Debug.Log("BLUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUE");
        }

        Debug.Log("UPDATE : previousPosition : " + previousPosition + " / currentPosition" + currentPosition);
    }
}
