using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    public GameObject PosMarkerPrefab;
    public CinemachineCameraController cameraController;

    public GameObject earthPillar;
    public GameObject earthPlatform;
    public ParticleSystem groundMovingEffect;
    [Range(0, 50)]
    public float maxDistance;

    private RaycastHit hit;
    private Vector3 lastMarkerPosition = Vector3.zero; // store the last position of the marker before aiming in the void
    private Vector3 lastMarkerNormal = Vector3.zero; // store the last normal of the hit point from the marker before aiming in the void
    private float possibleSlopeForFloor = 0.7f; // Defines the value of the normal for which a pillar can be built above
    private float possibleSlopeForWall = 0f; // Defines the value of the normal for which above (but below the value for the pillar) a platform can be built

    public void LateUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection, cameraController.transform.position);                       
            marker.transform.LookAt(cameraController.transform);

            hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;

            // Avoid to have no point/normal where the pillar/platform has to spawn
            if (hit.point == Vector3.zero || hit.normal.y < 0)
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
            if(lastMarkerNormal.y > possibleSlopeForFloor)
            {
                groundMovingEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            }
            else if(lastMarkerNormal.y > possibleSlopeForWall)
            {
                groundMovingEffect.transform.LookAt(cameraController.transform);
            }
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;

        groundMovingEffect.transform.position = marker.transform.position;
        groundMovingEffect.Play();

        marker.Init(maxDistance, PosMarkerPrefab);
    }

    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().currentSpell = null;
        elementary.GetComponent<ElementaryController>().computePosition = true;
        elementary.GetComponent<ElementaryController>().readyToCast = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        // If the normal.y is < 0, the player can not spawn any object (the wall/ceiling do not allow to spawn objects) 
        if (lastMarkerNormal.y > possibleSlopeForFloor) // If the slope is not too hard
        {
            Debug.Log("SPAWN PILLAR");

            // Avoid to rotate the pillar on X axis when it spawns
            Vector3 v = cameraController.transform.position - lastMarkerPosition;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);

            Instantiate(earthPillar, marker.transform.position, rot);
        }
        else if (lastMarkerNormal.y >= possibleSlopeForWall)
        {
            Debug.Log("SPAWN PLATFORM");

            // Avoid to rotate the platform on X axis when it spawns
            Vector3 v = cameraController.transform.position - lastMarkerPosition;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);

            Instantiate(earthPlatform, marker.transform.position, rot);
        }

        groundMovingEffect.Stop();
        Destroy(marker.gameObject);
        Terminate();
    }

    protected override void SetDamages()
    {
        throw new System.NotImplementedException();
    }
}
