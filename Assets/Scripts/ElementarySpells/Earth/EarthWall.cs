using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : AbstractSpell
{
    public GameObject PosMarkerPrefab;
    private CameraController cameraController;

    public GameObject earthPillar;
    public GameObject earthPlatform;
    public float maxDistance;

    void Start()
    {
        
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.forward * maxDistance, Color.red);

        //RaycastHit hit;
        //Ray ray = new Ray(transform.position, Vector3.forward);

        //int layer_mask_wall = LayerMask.GetMask("Wall");
        //int layer_mask_ground = LayerMask.GetMask("Ground");

        //if(Physics.Raycast(ray, out hit, maxDistance, layer_mask_wall, QueryTriggerInteraction.Ignore))
        //{
        //    print("Spawn platform on " + hit.transform.name + " at " + hit.distance + " meters.");
        //    //Inst
        //}
        //if (Physics.Raycast(ray, out hit, maxDistance, layer_mask_ground, QueryTriggerInteraction.Ignore))
        //{
        //    print("Spawn pillar on " + hit.transform.name + " at " + hit.distance + " meters.");
        //}
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
        Destroy(marker.gameObject);
    }
}
