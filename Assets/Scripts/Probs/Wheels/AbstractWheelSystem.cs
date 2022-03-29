using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWheelSystem : MonoBehaviour
{
    public float spinForce;
    public GameObject doorToOpen;
    protected Vector3 hitPoint;

    public abstract void OpenDoor();

    public void SetHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }
}
