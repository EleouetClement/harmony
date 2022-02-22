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

    public override void Init(float maxRayCastDistance, GameObject prefab)
	{
        base.Init(maxRayCastDistance, prefab);
    }
	public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        
    }


    /// <summary>
    /// Returns the position where to launch the earth ball and draws markers
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTarget()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, maxRayCastDistance));
        Vector3 origin = ray.origin + 0.1f * ray.direction;
        if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out hit, maxRayCastDistance, (1 << HarmonyLayers.LAYER_DEFAULT) + (1 << HarmonyLayers.LAYER_GROUND)))
            {
            if (markerInstance == null)
            {
                markerInstance = Instantiate(markerPrefab, hit.point + hit.normal * 0.1f, Quaternion.FromToRotation(markerPrefab.transform.up, hit.normal) * markerPrefab.transform.rotation);
                markerInstance.GetComponent<MeshRenderer>().enabled = true;
            }

            markerInstance.GetComponent<MeshRenderer>().enabled = true;
            markerInstance.transform.position = hit.point + hit.normal * 0.1f;
            markerInstance.transform.rotation = Quaternion.FromToRotation(markerInstance.transform.up, hit.normal) * markerInstance.transform.rotation;

            trajectoryCalculator.CalculateTrajectory();
            trajectoryCalculator.DisplayTrajectory(true);
            
            return hit.point;
        }
        //out of range
		else
		{
            trajectoryCalculator.DisplayTrajectory(false);
            if (markerInstance != null)
                markerInstance.GetComponent<MeshRenderer>().enabled = false;

            Vector3 target = origin + ray.direction * maxRayCastDistance;
            target.y = transform.position.y;
            return target;
        }
        
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
