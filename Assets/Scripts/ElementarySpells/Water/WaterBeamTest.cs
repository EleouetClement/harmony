using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeamTest : MonoBehaviour
{
    public Camera cam;
    public GameObject firePoint;
    public LineRenderer lr;

    public float maximumLength;

    void Update()
    {
        lr.SetPosition(0, firePoint.transform.position);

        RaycastHit hit;

        var mousePos = Input.mousePosition;
        var rayMouse = cam.ScreenPointToRay(mousePos);

        if(Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumLength))
        {
            if (hit.collider)
                lr.SetPosition(1, hit.point);
        }
        else
        {
            var pos = rayMouse.GetPoint(maximumLength);
            lr.SetPosition(1, pos);
        }

    }
}
