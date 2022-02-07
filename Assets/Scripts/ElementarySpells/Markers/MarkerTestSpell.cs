using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Demo class to show how to use the positionning marker
/// </summary>
public class MarkerTestSpell : AbstractSpell
{
    public GameObject PosMarkerPrefab;
    private CameraController cameraController;
    //private PositionningMarker posMark;
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();     
    }

    public void LateUpdate()
    {
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection, cameraController.GetViewPosition);
            marker.transform.LookAt(cameraController.transform);
        }
    }
    protected override void onChargeEnd(float chargetime)
    {
        PositionningMarker posMark= (PositionningMarker)marker;
        Debug.Log("Position locked : " + posMark.targetPosition);
        Destroy(marker.gameObject);
    }
}
