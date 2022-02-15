using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    public GameObject PosMarkerPrefab;
    public CameraController cameraController;

    public GameObject earthPillar;
    public GameObject earthPlatform;
    [Range(0, 50)]
    public float maxDistance;

    void Start()
    {

    }

    void Update()
    {

    }

    public void LateUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection, cameraController.GetViewPosition);                       
            marker.transform.LookAt(cameraController.transform);
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        cameraController = elementary.GetComponent<ElementaryController>().playerCameraController;
        print(cameraController);
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
        RaycastHit hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;

        // If the normal.y is < 0, the player can not spawn any object (the wall/ceiling do not allow to spawn objects) 
        if (hit.normal.y > 0.70) // If the slope is not too hard
        {
            Debug.Log("SPAWN PILLAR");

            // Avoid to rotate the pillar on X axis when it spawns
            Vector3 v = cameraController.GetViewPosition - hit.transform.position;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);

            Instantiate(earthPillar, marker.transform.position, rot);
        }
        else if (hit.normal.y >= 0)
        {
            Debug.Log("SPAWN PLATFORM");

            // Avoid to rotate the platform on X axis when it spawns
            Vector3 v = cameraController.GetViewPosition - hit.transform.position;
            v.y = 0f;
            v.Normalize();
            Quaternion rot = Quaternion.LookRotation(v);

            Instantiate(earthPlatform, marker.transform.position, rot);
        }

        Destroy(marker.gameObject);
        Terminate();
    }
}
