using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneDoor : MonoBehaviour
{
    public GameObject doorForInteraction; // it is possible to make an array to have multiple opened doors
    public bool isPermanentlyOpen;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == HarmonyLayers.LAYER_MOVABLE)
        {
            if(isPermanentlyOpen)
            {
                Destroy(doorForInteraction);
                Destroy(gameObject);
            }
            else
            {
                // Door opening animation
                doorForInteraction.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == HarmonyLayers.LAYER_MOVABLE)
        {
            if(!isPermanentlyOpen)
            {
                // Door closing animation
                doorForInteraction.SetActive(true);
            }
        }
    }
}
