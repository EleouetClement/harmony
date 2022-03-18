using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchbox : MonoBehaviour
{

    public GameObject ledge;

    void OnTriggerEnter(Collider collision)
    {
        if (ledge.GetComponent<ClimbableLedgeBehavior>()
            && collision.gameObject == GameModeSingleton.GetInstance().GetPlayerReference)
        {
            ledge.GetComponent<ClimbableLedgeBehavior>().cantrigger = true;
            Debug.Log("Entered climbable zone. Press jump (space) to climb.");
            GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerMotionController>().lastLedge = ledge.GetComponent<ClimbableLedgeBehavior>();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (ledge.GetComponent<ClimbableLedgeBehavior>()
            && collision.gameObject == GameModeSingleton.GetInstance().GetPlayerReference)
        {
            ledge.GetComponent<ClimbableLedgeBehavior>().cantrigger = false;
            Debug.Log("Exited climbable zone.");
        }
    }

}
