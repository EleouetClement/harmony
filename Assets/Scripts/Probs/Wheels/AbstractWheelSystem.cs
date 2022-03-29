using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWheelSystem : MonoBehaviour
{
    public GameObject objectToOpen;
    public float spinForce;
    protected Transform wheelPosition;
    protected Vector3 hitPoint;

    public abstract void OpenDoor();

    public void SetHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }
}
