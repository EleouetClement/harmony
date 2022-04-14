using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    private enum Status
    {
        pillar,
        platform,
        unvalid,
        noTarget,
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
    private PlayerMotionController controller;
    private RaycastHit hit;
    private Vector3 lastMarkerPosition = Vector3.zero; // store the last position of the marker before aiming in the void
    private Vector3 lastMarkerNormal = Vector3.zero; // store the last normal of the hit point from the marker before aiming in the void
    [SerializeField] private float quickCastTimer = 0.2f;

    // Defines the value of the normal for which a pillar can be built above (between 0 and 1)
    [SerializeField] private float thresholdGroundToWall = 0.7f;
    // Defines the value of the normal for which above (but below the value for the pillar) a platform can be built (between 0 and 1)
    [SerializeField] private float thresholdWallToCeiling = 0f;

    [Header("Dev")]
    [SerializeField] private GameObject[] markerPrefabs;
    private GameObject visuReference;
    [SerializeField] float quickPillarOffset;
    [SerializeField] float rayCastRadius = 1;
    [SerializeField] LayerMask rayCastLayers;
    private Status newStatus = Status.noTarget;
    private Status currentStatus = Status.noTarget;
    private bool manaBurned = false;



    private void Awake()
    {
        if (markerPrefabs == null || markerPrefabs.Length == 0)
        {
            Debug.LogError("PositionningMarker.Awake : No Prefab");
            Destroy(gameObject);
        }
        
    }


	public void LateUpdate()
    {
        if (!isReleased() && charge > quickCastTimer)
        {
            BurnMana();
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
        
        this.target = target;
        elementary = elemRef;
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        cinemachineCameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        elementaryController = elemRef.GetComponent<ElementaryController>();
        controller = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerMotionController>();
        // Allows to get collision with the raycast depending on the thresholds of the earth wall spell values

        groundMovingEffect.transform.position = marker.transform.position;
        groundMovingEffect.Play();
        marker.GetComponent<PositionningMarker>().layersCollisionWithRaycast = layersCollision;
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
        PlayerMotionController motionControl = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerMotionController>();
        // If the normal.y is < 0, the player can not spawn any object (the wall/ceiling do not allow to spawn objects)
        DestroyOldPillar();
        if (chargetime < quickCastTimer)
        {
            if (IsOnPillar())
            {
                Terminate();
                return;
            }
            if (motionControl.onGround)
            {
                lastMarkerPosition = elementary.GetComponent<ElementaryController>().shoulder.position;
                if (motionControl.isMoving)
                {
                    //Offset creation to spawn pillar in front of the player and not below it
                    Vector3 offset = GameModeSingleton.GetInstance().GetPlayerMesh.transform.forward.normalized;
                    offset += new Vector3(0, 0, quickPillarOffset);
                    lastMarkerPosition += offset;
                }
                Vector3 v = cinemachineCameraController.transform.position - lastMarkerPosition;
                v.y = 0f;
                v.Normalize();
                Quaternion rot = Quaternion.LookRotation(v);
                Instantiate(earthPillar, lastMarkerPosition, rot);
                BurnMana();
            }
        }
        else if(currentStatus != Status.noTarget)
        {
            Vector3 v = cinemachineCameraController.transform.position - lastMarkerPosition;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);
            switch (currentStatus)
            {
                case Status.pillar:
                    Instantiate(earthPillar, lastMarkerPosition, rot);
                    break;

                case Status.platform:
                    Instantiate(earthPlatform, lastMarkerPosition, rot);
                    break;

                default:
                    Debug.Log("Earth wall : no valid target for wall/plaforms " + currentStatus);
                    break;
            }
        }  
        groundMovingEffect.Stop();
        Destroy(marker.gameObject);
        Destroy(visuReference);
        Terminate();
    }

    private void BurnMana()
    {
        if(!manaBurned)
        {
            PlayerGameplayController player = GameModeSingleton.GetInstance()?.GetPlayerReference?.GetComponent<PlayerGameplayController>();
            if (player)
            {
                player.OnManaSpend(GetManaCost());
            }
            manaBurned = true;
        }
        
    }

    private void Previsualization(RaycastHit hit)
    {

        if (hit.collider == null)
        {
            newStatus = Status.unvalid;
        }
        else
        {
            if (hit.normal.y > thresholdGroundToWall)
            {
                newStatus = Status.pillar;
                lastMarkerPosition = hit.point;
                lastMarkerNormal = hit.normal;
            }
            else if (hit.normal.y >= thresholdWallToCeiling)
            {
                newStatus = Status.platform;
                lastMarkerPosition = hit.point;
                lastMarkerNormal = hit.normal;
            }
            else if (hit.normal.y < thresholdWallToCeiling)
            {
                newStatus = Status.unvalid;
            }
        }   
        if (newStatus != currentStatus && newStatus == Status.pillar)
        {
            if (visuReference != null)
                Destroy(visuReference);
            visuReference = Instantiate(markerPrefabs[0], hit.point, Quaternion.identity);
            currentStatus = newStatus;
        }
        else
        {
            if (newStatus != currentStatus && newStatus == Status.platform)
            {
                if (visuReference != null)
                    Destroy(visuReference);
                visuReference = Instantiate(markerPrefabs[1], hit.point, Quaternion.identity);
                currentStatus = newStatus;
            }
        }
        if (visuReference)
        {          
            if ((newStatus == Status.unvalid))
            {
                visuReference.transform.position = lastMarkerPosition;
            }
            else
            {
                visuReference.transform.position = hit.point;
                Vector3 positionForRotation = GameModeSingleton.GetInstance().GetPlayerReference.transform.position;
                positionForRotation.y = visuReference.transform.position.y;
                visuReference.transform.LookAt(positionForRotation);
            }
        }    
    }

    private bool IsOnPillar()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerMesh.transform.position, Vector3.down, out hit, rayCastRadius, rayCastLayers))
        {
            return true;
        }
        else
        {
            Debug.DrawRay(playerMesh.transform.position, Vector3.down * rayCastRadius, Color.red, 10);
        }
        return false;
    }

    private void DestroyOldPillar()
    {
        if (EarthPillar.instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPillar in the scene, the old one is destroyed");
            Destroy(EarthPillar.instance.gameObject);
        }
        else if (EarthPlatform.instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPlatform in the scene, the old one is destroyed");
            Destroy(EarthPlatform.instance.gameObject);
        }
    }
}
