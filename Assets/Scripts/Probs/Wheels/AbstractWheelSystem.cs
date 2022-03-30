using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWheelSystem : MonoBehaviour, IDamageable
{
    public float spinForce; // Force of the water beam to make the wheel rotate
    public GameObject doorToOpen;
    /*[Range(0, 1)]*/ public float speedToOpen; // Opening speed of the door
    public bool canBeClosedAgain;
    //public bool isAffectedByGravity;

    protected Vector3 hitPoint; // Hit point of the beam on the wheel
    protected Rigidbody rigidBody;
    protected float timer = 0f;

    protected bool isTotallyOpened = false;
    protected bool isTotallyClosed = true;

    protected Quaternion previousRotation;
    protected Quaternion currentRotation;

    public abstract void OpenDoor();
    public abstract void CloseDoor();

    public void OnDamage(DamageHit hit)
    {
        rigidBody.AddForceAtPosition(hit.direction * spinForce * Time.deltaTime, hitPoint);

        //Debug.Log("Rotation : " + rigidBody.angularVelocity);

        // Check the wheel rotation to know the rotation direction
        previousRotation = currentRotation;
        currentRotation = transform.parent.gameObject.transform.rotation;

        // If the beam direction goes in a direction, the wheel opens the door, if it goes in the other direction, it closes the door
        // If clockwise direction --> open --- else if anti-clockwise direction --> close
        if (currentRotation.eulerAngles.z < previousRotation.eulerAngles.z)
        {
            OpenDoor();
        }
        else if (currentRotation.eulerAngles.z > previousRotation.eulerAngles.z)
        {
            if (canBeClosedAgain)
            {
                CloseDoor();
            }
        }
    }

    public void SetHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }
}
