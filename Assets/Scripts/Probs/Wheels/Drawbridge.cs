using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        // If the bridge hits something (the ground in this case), it stops falling
        if (collider.gameObject.layer == HarmonyLayers.LAYER_GROUND)
        {
            transform.parent.gameObject.GetComponentInChildren<WheelDrawbridge>().DrawbridgeHasFallen();
        }
    }
}
