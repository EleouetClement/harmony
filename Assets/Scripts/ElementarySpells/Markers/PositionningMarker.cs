using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    private GameObject currentMarkerInstance;
    public Vector3 targetPosition { get; private set; }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, 1<<6))
        {
            if(hit.collider.gameObject.layer.Equals(6))
            {
                targetPosition = hit.point;
                currentMarkerInstance.transform.position = hit.point;
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
        currentMarkerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
    }

    public override void OnDestroy()
    {
        Destroy(currentMarkerInstance);
    }
}
