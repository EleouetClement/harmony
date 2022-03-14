using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionningMarker : AbstractMarker
{

    private enum Status
    {
        pillar,
        platform
    }
    public Vector3 targetPosition { get; private set; }
    [SerializeField] private GameObject [] markerPrefabs;
    private RaycastHit hit;

    private GameObject visuReference;
    private Status newStatus;
    private Status currentStatus;

    private float slopeLowerTreshold;
    private float slopeUpperTreshold;
    private float currentSlope;

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
            newStatus = currentStatus = Status.pillar;
        }
    }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        int layers = 1 << HarmonyLayers.LAYER_GROUND;
        layers += 1 << HarmonyLayers.LAYER_WALL_ENABLE;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layers))
        {
            if(hit.normal.y > slopeLowerTreshold)
            {
                newStatus = Status.pillar;
            }
            else
            {
                if(hit.normal.y >= slopeUpperTreshold)
                {
                    newStatus = Status.platform;
                }
            }
            if(newStatus != currentStatus && newStatus == Status.pillar)
            {
                Destroy(visuReference);
                currentSlope = hit.normal.y;
                //Debug.Log("DisplayTarget : hit.normal.y : " + hit.normal.y);
                visuReference = Instantiate(markerPrefabs[0], hit.point, Quaternion.identity);
                currentStatus = newStatus;
            }
            else
            {
                if(newStatus != currentStatus && newStatus == Status.platform)
                {
                    currentSlope = hit.normal.y;
                    //Debug.Log("DisplayTarget : hit.normal.y : " + hit.normal.y);
                    Destroy(visuReference);
                    visuReference = Instantiate(markerPrefabs[1], hit.point, Quaternion.identity);
                    currentStatus = newStatus;
                }
            }
            targetPosition = hit.point;
            visuReference.transform.position = hit.point;
            Vector3 positionForRotation = GameModeSingleton.GetInstance().GetPlayerReference.transform.position;
            positionForRotation.y = visuReference.transform.position.y;
            visuReference.transform.LookAt(positionForRotation);
            
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
    /// <summary>
    /// Allows to Set the treshold for angled surfaces if there is a need of distinction between to types of slope
    /// </summary>
    /// <param name="treshold"></param>
    public void SetSlopeLowerTreshold(float treshold)
    {
        slopeLowerTreshold = treshold;
    }
    public void SetSlopeUpperTreshold(float treshold)
    {
        slopeUpperTreshold = treshold;
    }

    public override void OnDestroy()
    {
        Destroy(visuReference);
    }
}
