using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    public Vector3 targetPosition { get; private set; }
    private RaycastHit hit;
    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
       
        int layers = 1 << HarmonyLayers.LAYER_GROUND;
        layers += 1 << HarmonyLayers.LAYER_DEFAULT;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layers))
        {         
            targetPosition = hit.point;
            transform.position = hit.point;          
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
