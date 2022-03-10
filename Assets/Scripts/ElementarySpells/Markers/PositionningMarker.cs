using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{
    public Vector3 targetPosition { get; private set; }
    [SerializeField] private GameObject [] markerPrefabs;
    private RaycastHit hit;
    private bool prefabChanged = false;

    private GameObject visuReference;
    private bool onGround = true;

    private void Awake()
    {
        if(markerPrefabs == null || markerPrefabs.Length == 0)
        {
            Debug.LogError("PositionningMarker.Awake : No Prefab");
            Destroy(gameObject);
        }
        else
        {
            visuReference = Instantiate(markerPrefabs[0], Vector3.zero, Quaternion.identity);
        }
    }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        int layers = 1 << HarmonyLayers.LAYER_GROUND;
        layers += 1 << HarmonyLayers.LAYER_WALL_ENABLE;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layers))
        {
            if(!hit.transform.gameObject.layer.Equals(HarmonyLayers.LAYER_GROUND) && onGround)
            {
                Destroy(visuReference);
                visuReference = Instantiate(markerPrefabs[1], hit.point, Quaternion.identity);
            }
            else
            {
                if(hit.transform.gameObject.layer.Equals(HarmonyLayers.LAYER_GROUND) && !onGround)
                {
                    Destroy(visuReference);
                    visuReference = Instantiate(markerPrefabs[0], hit.point, Quaternion.identity);
                }
            }
            if(hit.normal.y > 0.70)
            {
                targetPosition = hit.point;
                visuReference.transform.position = hit.point;
                //Debug.DrawRay(origin, direction * maxRayCastDistance, Color.green, 10);
                
            }
            else if(hit.normal.y >= 0)
            {
                targetPosition = hit.point;
                visuReference.transform.position = hit.point;
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
        Destroy(visuReference);
    }
}
