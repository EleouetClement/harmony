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
        layers += 1 << HarmonyLayers.LAYER_WALL_ENABLE;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layers))
        {
            if(hit.normal.y > 0.70)
            {
                targetPosition = hit.point;
                transform.position = hit.point;

                Debug.DrawRay(origin, direction * maxRayCastDistance, Color.green, 10);
                Debug.Log("Spawn of Pillar is possible --> hit.normal : " + hit.normal);
            }
            else if(hit.normal.y >= 0)
            {
                targetPosition = hit.point;
                transform.position = hit.point;

                Debug.DrawRay(origin, direction * maxRayCastDistance, Color.blue, 10);
                Debug.Log("Spawn of Platform is possible --> hit.normal : " + hit.normal);
            }
            else
            {
                Debug.DrawRay(origin, direction * maxRayCastDistance, Color.yellow, 10);
                Debug.Log("You can not spawn any object --> hit.normal : " + hit.normal);
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
