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

    [HideInInspector]
    public LayerMask layersCollisionWithRaycast;
    private GameObject visuReference;
    private Status newStatus;
    private Status currentStatus;

    private float slopeLowerTreshold;
    private float slopeUpperTreshold;
    private float currentSlope;

    private void Awake()
    {
        
    }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        int layers = 1 << HarmonyLayers.LAYER_GROUND;
        layers += 1 << HarmonyLayers.LAYER_WALL_ENABLE;
        if (Physics.Raycast(origin, direction, out hit, maxRayCastDistance, layersCollisionWithRaycast))
        {
            targetPosition = hit.point;
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
