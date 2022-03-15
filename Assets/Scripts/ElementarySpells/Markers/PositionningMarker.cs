using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    public Vector3 targetPosition { get; private set; }
    public LayerMask layersCollisionWithRaycast;

    [HideInInspector] public float thresholdGroundToWall = 0.7f;
    [HideInInspector] public float thresholdWallToCeiling = 0f;
    private RaycastHit hit;

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layersCollisionWithRaycast))
        {
            if(hit.normal.y > thresholdGroundToWall)
            {
                targetPosition = hit.point;
                transform.position = hit.point;
                //Debug.DrawRay(origin, direction * maxRayCastDistance, Color.green, 10);
            }
            else if(hit.normal.y >= thresholdWallToCeiling)
            {
                targetPosition = hit.point;
                transform.position = hit.point;
                //Debug.DrawRay(origin, direction * maxRayCastDistance, Color.blue, 10);
            }
            else
            {
                //Debug.DrawRay(origin, direction * maxRayCastDistance, Color.yellow, 10);
            }
        }
        else
        {
            //Debug.DrawRay(origin, direction * maxRayCastDistance, Color.red, 10);
            targetPosition = Vector3.zero;
            //Debug.Log("No valid target");
        }
    }

    public override void Init(float maxRayCastDistance, GameObject prefab)
    {
        base.Init(maxRayCastDistance, prefab);
    }

    public RaycastHit GetRayCastInfo
    {
        get
        {
            return hit;
        }
    }

    public override void OnDestroy()
    {
    }
}
