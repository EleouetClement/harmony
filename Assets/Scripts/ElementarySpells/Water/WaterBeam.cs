using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeam : AbstractSpell
{
    [Min(1)] public float maxDistance;
    [Min(1)] public float acceleration;

    public GameObject PosMarkerPrefab;
    public CameraController cameraController;

    private RaycastHit hit;


    void Update()
    {


    }

    public void LateUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection, cameraController.GetViewPosition);
            marker.transform.LookAt(cameraController.transform);

            hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;

            // If the player does not hit anything or not the good layer, he can still be aimed at
            if (hit.point == Vector3.zero)
            {
                Vector3 virtualPointDistanceMax = cameraController.GetViewPosition + cameraController.GetViewDirection * maxDistance;
                hit.point = virtualPointDistanceMax;
            }

            transform.LookAt(hit.point);
        }


    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target.normalized);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        cameraController = elementary.GetComponent<ElementaryController>().playerCameraController;
        marker.Init(maxDistance, PosMarkerPrefab);

    }

    public override void Terminate()
    {
        ElementaryController elemCtrl = elementary.GetComponent<ElementaryController>();
        //elemCtrl.currentSpell = null;
        //elemCtrl.computePosition = true;
        elementary.GetComponent<ElementaryController>().currentSpell = null;
        elementary.GetComponent<ElementaryController>().computePosition = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        Destroy(marker.gameObject);
        Terminate();
    }
}
