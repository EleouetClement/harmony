using UnityEngine;

public class WaterBeam : AbstractSpell
{
    [Min(1)] public float maxDistance; // Maximum distance if the beam does not hit anything
    [Min(1)] public float beamAcceleration; // Extension speed of the beam

    public GameObject PosMarkerPrefab;
    public ParticleSystem impactEffect;
    public LayerMask layersCollision;

    private ElementaryController elementaryController;
    private Vector3 elementaryPosition;
    private CinemachineCameraController cameraController;

    private RaycastHit hit; // Raycast to get the hit.point of the marker for the character's aim
    private RaycastHit raycastFromElementary; // Raycast to get the direction of the beam 
    private Vector3 beamDirection;
    private bool isAccelerationBeamFinished = false; // Has the beam finished expanding (false if the beam continues to expand)
    private Vector3 acceleration = Vector3.zero; // Extension vector of the beam
    private Vector3 currentDistancePoint = Vector3.zero; // Current distance to the end of the beam
    private Vector3 possibleDistancePoint = Vector3.zero; // Maximum distance depending on the obstacles on the path

    public override void FixedUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection, cameraController.GetViewPosition);
            marker.transform.LookAt(cameraController.transform);

            // Get the target of the beam
            hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;

            // If the player does not hit anything or not the good layer, he can still be aimed at (with a virtual hit point)
            if (hit.point == Vector3.zero)
            {
                Vector3 virtualPointDistanceMax = elementaryPosition + cameraController.GetViewDirection * maxDistance;
                hit.point =  virtualPointDistanceMax;
            }

            beamDirection = hit.point - elementaryPosition;

            // Raycast from the elementary to the hit.point (direction of the beam)
            if (Physics.Raycast(elementaryPosition, beamDirection, out raycastFromElementary, maxDistance, layersCollision))
            {
                // If there is an obstacle, the max distance of the beam is the intersection with this obstacle
                possibleDistancePoint = raycastFromElementary.point;
            }
            else
            {
                // If there is no obstacle in the way of the beam, the beam can reach up to the hit point
                possibleDistancePoint = hit.point;
            }

            currentDistancePoint = elementaryPosition + beamDirection.normalized * transform.localScale.z;

            // If there is an obstacle, the beam stops expanding, else it continues
            if (Vector3.Distance(elementaryPosition, currentDistancePoint) >= Vector3.Distance(elementaryPosition, possibleDistancePoint))
            {
                isAccelerationBeamFinished = true;
            }
            else
            {
                isAccelerationBeamFinished = false;
            }

            //Affecting manaCost
            GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerGameplayController>()?.OnManaSpend(GetManaCost() * Time.fixedDeltaTime);
            
            /***** BEAM EXTENSION *****/
            // If the beam does not hit an obstacle, its extension continues
            if (!isAccelerationBeamFinished)
            {
                acceleration += new Vector3(0, 0, beamAcceleration * Time.fixedDeltaTime);
                transform.localScale = acceleration;

                // If the beam has reached an obstacle, it stops expanding
                if (acceleration.z >= Vector3.Distance(possibleDistancePoint, elementaryPosition))
                {
                    isAccelerationBeamFinished = true;
                    acceleration.z = Vector3.Distance(possibleDistancePoint, elementaryPosition);
                }
            }
            else
            {
                // The length of the beam depends on the hit point :
                // The end point of the beam is the hit point (and its extension length is readjusted)
                currentDistancePoint = possibleDistancePoint;
                acceleration.z = Vector3.Distance(elementaryPosition, possibleDistancePoint);
                transform.localScale = acceleration;

                /***** Collision *****/
                // If the beam hits a movable object, it pushes the object in the direction of the beam
                if (raycastFromElementary.collider.gameObject.layer == HarmonyLayers.LAYER_MOVABLE)
                {
                    IDamageable item = raycastFromElementary.collider.gameObject.GetComponent<IDamageable>();

                    if (item == null)
                    {
                        Debug.LogError("MovableObject is Not Damageable");
                    }
                    else
                    {
                        DamageHit damage = new DamageHit(0f, beamDirection.normalized);
                        item.OnDamage(damage);
                    }
                }
                //if(raycastFromElementary.collider.gameObject.layer ==)
            }

            /***** BEAM IMPACT *****/
            // Put the impact effect on the impact area with a slight offset to see this area well
            impactEffect.transform.position = currentDistancePoint - beamDirection.normalized * 0.2f;
            impactEffect.transform.LookAt(elementaryPosition);
            transform.LookAt(hit.point);
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        elementaryController = elemRef.GetComponent<ElementaryController>();
        elementaryPosition = elementaryController.transform.position;
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;

        marker.GetComponent<PositionningMarker>().layersCollisionWithRaycast = layersCollision;

        // Init the visual
        impactEffect.transform.position = elementaryController.transform.position;
        marker.GetComponent<UnityEngine.Rendering.Universal.DecalProjector>().enabled = false; // Hide the marker

        marker.Init(maxDistance, PosMarkerPrefab);
    }

    public override void Terminate()
    {
        elementaryController.Reset();
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        Destroy(marker.gameObject);
        Terminate();
    }
}
