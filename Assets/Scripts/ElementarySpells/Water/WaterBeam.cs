using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeam : AbstractSpell
{
    [Min(1)] public float maxDistance;
    [Min(1)] public float acceleration;

    public GameObject PosMarkerPrefab;
    public CinemachineCameraController cameraController;
    public ElementaryController elemCtrl;
    public ParticleSystem impactEffect;
    private RaycastHit hit;

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

            //impactEffect.transform.rotation = Quaternion.LookRotation()
            impactEffect.transform.position = hit.point;
            impactEffect.transform.LookAt(elemCtrl.transform.position);
            transform.LookAt(hit.point);
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        //base.init(elemRef, target.normalized);
        base.init(elemRef, target);
        GameObject tmp = Instantiate(PosMarkerPrefab, Vector3.zero, Quaternion.identity);
        marker = tmp.GetComponent<PositionningMarker>();
        elemCtrl = elemRef.GetComponent<ElementaryController>();
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        marker.Init(maxDistance, PosMarkerPrefab);
        impactEffect.Play();
    }

    public override void Terminate()
    {
        elemCtrl.currentSpell = null;
        elemCtrl.computePosition = true;
        elemCtrl.readyToCast = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        Destroy(marker.gameObject);
        impactEffect.Stop();
        Terminate();
    }
}
