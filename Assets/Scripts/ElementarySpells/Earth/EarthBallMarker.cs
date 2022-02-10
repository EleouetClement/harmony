using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBallMarker : AbstractMarker
{
    /// <summary>
    /// Actual marker shown in game
    /// </summary>
    GameObject markerInstance;

	public override void Init(float maxRayCastDistance, GameObject prefab)
	{
        base.Init(maxRayCastDistance, prefab);
    }
	public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        
    }

    public Vector3 GetTarget(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin + direction * 0.1f, direction, out hit, maxRayCastDistance))
        {
            markerPrefab.transform.position = hit.point + hit.normal * 0.1f;
        }

        Debug.DrawRay(origin + direction * 0.1f, hit.point,Color.red,5);

        return hit.point;
    }

    public Vector3 GetTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, maxRayCastDistance));
        Vector3 origin = ray.origin + 0.1f * ray.direction;
        if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out hit, maxRayCastDistance))
        {
            if (markerInstance == null)
            {
                markerInstance = Instantiate(markerPrefab, hit.point + hit.normal * 0.1f, Quaternion.FromToRotation(markerPrefab.transform.up, hit.normal) * markerPrefab.transform.rotation);
                markerInstance.GetComponent<MeshRenderer>().enabled = true;
            }
                
            markerInstance.transform.position = hit.point + hit.normal*0.1f;
            markerInstance.transform.rotation = Quaternion.FromToRotation(markerInstance.transform.up, hit.normal) * markerInstance.transform.rotation;
            print("target : "+hit.collider);
            //Debug.DrawRay(origin, hit.point - ray.origin, Color.red, 5);
            return hit.point;
        }
		else
		{
            if (markerInstance != null)
                markerInstance.GetComponent<MeshRenderer>().enabled = false;

            Vector3 target = origin + ray.direction * maxRayCastDistance;
            target.y = transform.position.y;
            return target;
        }

        

        
    }

    public void SetMarkerRadius(float radius)
    {
        Vector3 currentScale = markerInstance.transform.localScale;
        markerInstance.transform.localScale = new Vector3(2f * radius, currentScale.y,2f * radius);
    }

    public void DestroyMarker()
    {
        Destroy(markerInstance);
    }

	public override void OnDestroy()
	{
		
	}
}
