using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBallMarker : AbstractMarker
{
    /// <summary>
    /// Actual marker shown in game
    /// </summary>
    public GameObject markerInstance;

    public TrajectoryCalculator trajectoryCalculator;

    public RaycastHit hit;
    public Vector3 target;
    private Vector3 aimDirection;
    public bool freeAim;

    private Ray ray;
    private Vector3 screenPoint;
    private Vector3 worldPoint;

    public override void Init(float maxRayCastDistance, GameObject prefab)
	{
        base.Init(maxRayCastDistance, prefab);
    }
	public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        
    }


    public Vector3 Aim()
    {
         screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, maxRayCastDistance);
         worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
         ray = Camera.main.ScreenPointToRay(screenPoint);

        //aiming at environment
        if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out hit, maxRayCastDistance, (1 << HarmonyLayers.LAYER_DEFAULT) + (1 << HarmonyLayers.LAYER_GROUND)))
        {
            return GetTarget();
        }
        //aiming out of range
        else
        {
            return GetDirection();
        }
    }

    private Vector3 GetTarget()
    {
        freeAim = false;
        if (markerInstance == null)
        {
            markerInstance = Instantiate(markerPrefab, hit.point + hit.normal * 0.1f, Quaternion.FromToRotation(markerPrefab.transform.up, hit.normal) * markerPrefab.transform.rotation);
            markerInstance.GetComponent<MeshRenderer>().enabled = true;
        }

        markerInstance.GetComponent<MeshRenderer>().enabled = true;
        markerInstance.transform.position = hit.point + hit.normal * 0.1f;
        markerInstance.transform.rotation = Quaternion.FromToRotation(markerInstance.transform.up, hit.normal) * markerInstance.transform.rotation;

        //trajectoryCalculator.CalculateTrajectory();

        target = hit.point;
        return hit.point;
    }

    private Vector3 GetDirection()
    {
        freeAim = true;
        if (markerInstance != null)
            markerInstance.GetComponent<MeshRenderer>().enabled = false;
        aimDirection = (worldPoint - GameModeSingleton.GetInstance().GetElementaryReference.transform.position).normalized;
        print(" direction :" + aimDirection);

        //trajectoryCalculator.CalculateTrajectory();
        
        return aimDirection;
    }

    public void SetMarkerRadius(float radius)
    {
        if (markerInstance != null)
        {
            Vector3 currentScale = markerInstance.transform.localScale;
            markerInstance.transform.localScale = new Vector3(2f * radius, currentScale.y , 2f * radius);
        }
    }

    public void DestroyMarker()
    {
        Destroy(markerInstance);
    }

	public override void OnDestroy()
	{
		
	}
}
