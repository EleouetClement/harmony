using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridge : MonoBehaviour
{
    public float speedFall;
    public float maxAngle = 150f;
    //public LayerMask layersToStopFallingBridge;
    private Vector3 initialRotation;
    private Vector3 finalRotation;
    public bool isFalling = false;
    private float timer = 0f;

    private void Start()
    {
        initialRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        finalRotation = new Vector3(maxAngle, transform.rotation.y, transform.rotation.z);
    }

    private void Update()
    {
        if(isFalling)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(finalRotation), speedFall * Time.deltaTime);
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), speedFall * timer);

            if (transform.rotation.eulerAngles.x >= maxAngle)
            {
                BridgeHasFallen();
            }
        }
    }

    public void BridgeHasFallen()
    {
        isFalling = false;

        foreach (Transform item in transform.parent.gameObject.GetComponentsInChildren<Transform>())
        {
            item.gameObject.layer = HarmonyLayers.LAYER_GROUND;
        }

        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<FallingBridge>());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == HarmonyLayers.LAYER_GROUND)
        {
            BridgeHasFallen();
        }
    }
}
