using UnityEngine;

public class WaterBeam : AbstractSpell
{
    [Min(1)] public float maxDistance; // Maximum distance if the beam does not hit anything
    [Min(1)] public float beamAcceleration; // Extension speed of the beam

    public GameObject PosMarkerPrefab;
    public ParticleSystem impactEffect;

    private ElementaryController elementaryController;
    private CinemachineCameraController cameraController;
    private RaycastHit hit;
    private bool isAccelerationBeamFinished = false; // Has the beam finished expanding (false if the beam continues to expand)
    private Vector3 acceleration = Vector3.zero; // Extension of the beam
    private Vector3 currentDistance = Vector3.zero; // Current distance to the end of the beam
    private Vector3 possibleDistance = Vector3.zero; // Maximum distance depending on the obstacles on the path

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
                Vector3 virtualPointDistanceMax = elementaryController.transform.position + cameraController.GetViewDirection * maxDistance;
                hit.point = virtualPointDistanceMax;
            }

            possibleDistance = hit.point;
            currentDistance = elementaryController.transform.position + cameraController.GetViewDirection * transform.localScale.z;

            // If there is an obstacle, the end point of the beam is the hit point (and its extension length is readjusted), else the beam continues to expand
            if(Vector3.Distance(transform.position, currentDistance) >= Vector3.Distance(transform.position, possibleDistance))
            {
                currentDistance = possibleDistance;
                acceleration.z = Vector3.Distance(transform.position, possibleDistance);
                isAccelerationBeamFinished = true;
            }
            else
            {
                isAccelerationBeamFinished = false;
            }

            // Put the impact effect on the impact area with a slight offset to see this area well
            impactEffect.transform.position = currentDistance - cameraController.GetViewDirection.normalized * 0.2f;
            impactEffect.transform.LookAt(elementaryController.transform.position);
            transform.LookAt(hit.point);

            //Affecting manaCost
            GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerGameplayController>()?.OnManaSpend(GetManaCost() * Time.fixedDeltaTime);

            // If the beam does not hit an obstacle, its extension continues
            if (!isAccelerationBeamFinished)
            {
                acceleration += new Vector3(0, 0, beamAcceleration * Time.fixedDeltaTime);
                transform.localScale = acceleration;

                // If the beam has reached an obstacle, it stops expanding
                if (acceleration.z >= Vector3.Distance(possibleDistance, transform.position))
                {
                    isAccelerationBeamFinished = true;
                    acceleration.z = Vector3.Distance(possibleDistance, transform.position);
                }
            }
            else
            {
                // The length of the beam depends on the hit point
                transform.localScale = acceleration;
            }
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        elementaryController = elemRef.GetComponent<ElementaryController>();
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;

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
