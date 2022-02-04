using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{

    private GameObject positionMarkerPrefab;
    private GameObject currentMarker;
    public Vector3 targetPosition { get; private set; }

    public override void DisplayTarget(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxRayCastDistance, 3))
        {
            if(hit.collider.gameObject.layer.Equals(6))
            {
                targetPosition = hit.point;
                currentMarker.transform.position = hit.point;
            }
        }
        else
        {
            Debug.Log("No valid target");
        }
    }

    public override void Init(float maxRayCastDistance)
    {
        base.Init(maxRayCastDistance);
        currentMarker = Instantiate(positionMarkerPrefab, Vector3.zero, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Destroy(currentMarker);
    }
}
