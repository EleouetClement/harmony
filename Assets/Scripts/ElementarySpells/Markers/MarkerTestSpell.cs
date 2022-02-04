using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerTestSpell : AbstractSpell
{

    private CameraController cameraController;

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        marker = new PositionningMarker();
        cameraController = elementary.GetComponent<ElementaryController>().playerCameraController;
    }

    public override void Terminate()
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isReleased())
        {
            marker.DisplayTarget(cameraController.GetViewDirection);
        }
    }
    protected override void onChargeEnd(float chargetime)
    {
        Destroy(marker);
    }
}
