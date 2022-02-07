using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    public Vector3 targetPosition { get; private set; }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, 1<<6))
        {
            if(hit.collider.gameObject.layer.Equals(6))
            {
                targetPosition = hit.point;
                transform.position = hit.point;
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * maxRayCastDistance, Color.red, 10);
            Debug.Log("No valid target");
            
        }
    }

    public override void Init(float maxRayCastDistance, GameObject prefab)
    {
        base.Init(maxRayCastDistance, prefab);
    }

    public override void OnDestroy()
    {
    }
}
