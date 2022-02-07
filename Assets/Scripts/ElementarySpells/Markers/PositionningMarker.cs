using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    private GameObject currentMarkerInstance;
    public Vector3 targetPosition { get; private set; }

    public override void DisplayTarget(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, direction, out hit, maxRayCastDistance, 3))
        {
            if(hit.collider.gameObject.layer.Equals(6))
            {
                targetPosition = hit.point;
                currentMarkerInstance.transform.position = hit.point;
            }
        }
        else
        {
            Debug.DrawRay(origin.position, direction, Color.red);
            Debug.Log("No valid target");
            
        }
    }

    public override void Init(float maxRayCastDistance, GameObject prefab, Transform origin)
    {
        base.Init(maxRayCastDistance, prefab, origin);
        currentMarkerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
    }

    public override void OnDestroy()
    {
        Destroy(currentMarkerInstance);
    }
}
