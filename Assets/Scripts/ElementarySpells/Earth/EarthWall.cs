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
        marker.Init(maxDistance, PosMarkerPrefab);
    }

    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().currentSpell = null;
        elementary.GetComponent<ElementaryController>().computePosition = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        RaycastHit hit = marker.GetComponent<PositionningMarker>().GetRayCastInfo;
        Debug.Log("HIT SPELL = " + hit.normal);
        if (hit.normal.y > 0.70)
        {
            Debug.Log("SPAWN PILLAR");
            Instantiate(earthPillar, marker.transform.position, Quaternion.identity);
        }
        else if (hit.normal.y >= 0)
        {
            Debug.Log("SPAWN PLATFORM");
            Instantiate(earthPlatform, marker.transform.position, Quaternion.identity);
        }
        
        Destroy(marker.gameObject);
        Terminate();
    }
}
