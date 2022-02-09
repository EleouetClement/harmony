using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    public GameObject PosMarkerPrefab;
    public CameraController cameraController;

    public GameObject earthPillar;
    public GameObject earthPlatform;
    public float maxDistance;

    private PositionningMarker posMark;

    void Start()
    {
        posMark = (PositionningMarker)marker;
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
        marker.Init(2000, PosMarkerPrefab);
    }

    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().currentSpell = null;
        elementary.GetComponent<ElementaryController>().computePosition = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        Instantiate(earthPlatform, marker.transform.position, Quaternion.identity);
        Destroy(marker.gameObject);
        Terminate();
    }
}
