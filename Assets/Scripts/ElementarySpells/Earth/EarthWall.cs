using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    private enum Status
    {
        pillar,
        platform
    }
    public GameObject PosMarkerPrefab;
    public GameObject earthPillar;
    public GameObject earthPlatform;
    public ParticleSystem groundMovingEffect;
    [Range(0, 50)]
    public float maxDistance;
    public LayerMask layersCollision;

    private ElementaryController elementaryController;
    private CinemachineCameraController cinemachineCameraController;
    private RaycastHit hit;
    private Vector3 lastMarkerPosition = Vector3.zero; // store the last position of the marker before aiming in the void
    private Vector3 lastMarkerNormal = Vector3.zero; // store the last normal of the hit point from the marker before aiming in the void

    // Defines the value of the normal for which a pillar can be built above (between 0 and 1)
    [SerializeField] private float thresholdGroundToWall = 0.7f;
    // Defines the value of the normal for which above (but below the value for the pillar) a platform can be built (between 0 and 1)
    [SerializeField] private float thresholdWallToCeiling = 0f;

    [Header("Dev")]
    [SerializeField] private GameObject[] markerPrefabs;
    private GameObject visuReference;
    private Status newStatus;
    private Status currentStatus;


    private void Awake()
    {
        if (markerPrefabs == null || markerPrefabs.Length == 0)
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

    public void LateUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cinemachineCameraController.GetViewDirection, cinemachineCameraController.transform.position);                       
            marker.transform.LookAt(cinemachineCameraController.transform);

            hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;
            Previsualization(hit);
            // Avoid to have no point/normal where the pillar/platform has to spawn
            if (hit.point == Vector3.zero || hit.normal.y < thresholdWallToCeiling)
            {
                hit.point = lastMarkerPosition;
                hit.normal = lastMarkerNormal;
            }
            else
            {
                lastMarkerPosition = hit.point;
                lastMarkerNormal = hit.normal;
            }
            groundMovingEffect.transform.position = marker.transform.position;

            // The rotation of the particles system is depending on the spawn point of the object object (ground or wall)
            if(lastMarkerNormal.y > thresholdGroundToWall)
            {
                groundMovingEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            }
            else if(lastMarkerNormal.y > thresholdWallToCeiling)
            {
                groundMovingEffect.transform.LookAt(cinemachineCameraController.transform);
            }
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        cinemachineCameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        elementaryController = elemRef.GetComponent<ElementaryController>();

        // Allows to get collision with the raycast depending on the thresholds of the earth wall spell values
        //marker.GetComponent<PositionningMarker>().layersCollisionWithRaycast = layersCollision;
        //marker.GetComponent<PositionningMarker>().thresholdGroundToWall = thresholdGroundToWall;
        //marker.GetComponent<PositionningMarker>().thresholdWallToCeiling = thresholdWallToCeiling;

        groundMovingEffect.transform.position = marker.transform.position;
        groundMovingEffect.Play();

        marker.Init(maxDistance, PosMarkerPrefab);
    }

    public override void Terminate()
    {
        elementaryController.Reset();
        if (marker != null)
            Destroy(marker.gameObject);
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        PositionningMarker pouet = (PositionningMarker)marker;
        // If the normal.y is < 0, the player can not spawn any object (the wall/ceiling do not allow to spawn objects) 
        if (lastMarkerNormal.y > thresholdGroundToWall) // If the slope is not too hard
        {
            Debug.Log("SPAWN PILLAR at location : " + lastMarkerPosition);

            // Avoid to rotate the pillar on X axis when it spawns
            Vector3 v = cinemachineCameraController.transform.position - lastMarkerPosition;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);

            //Instantiate(earthPillar, marker.transform.position, rot);          
            Instantiate(earthPillar, pouet.targetPosition, rot);
        }
        else if (lastMarkerNormal.y >= thresholdWallToCeiling)
        {
            Debug.Log("SPAWN PLATFORM at location : " + lastMarkerPosition);

            // Avoid to rotate the platform on X axis when it spawns
            Vector3 v = cinemachineCameraController.transform.position - lastMarkerPosition;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);
            
            Instantiate(earthPlatform, pouet.targetPosition, rot);
        }
        groundMovingEffect.Stop();
        Destroy(marker.gameObject);
        Destroy(visuReference);
        Terminate();
    }

    
    private void Previsualization(RaycastHit hit)
    {
        if (hit.normal.y > thresholdGroundToWall)
        {
            newStatus = Status.pillar;
        }
        else
        {
            if (hit.normal.y >= thresholdWallToCeiling)
            {
                newStatus = Status.platform;
            }
        }
        if (newStatus != currentStatus && newStatus == Status.pillar)
        {
            Destroy(visuReference);
            visuReference = Instantiate(markerPrefabs[0], hit.point, Quaternion.identity);
            currentStatus = newStatus;
        }
        else
        {
            if (newStatus != currentStatus && newStatus == Status.platform)
            {
                Destroy(visuReference);
                visuReference = Instantiate(markerPrefabs[1], hit.point, Quaternion.identity);
                currentStatus = newStatus;
            }
        }
        visuReference.transform.position = hit.point;
        Vector3 positionForRotation = GameModeSingleton.GetInstance().GetPlayerReference.transform.position;
        positionForRotation.y = visuReference.transform.position.y;
        visuReference.transform.LookAt(positionForRotation);
    }
}
