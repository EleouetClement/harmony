using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour
{
    public float speedFalling = 7f;
    public float maxAngle = 150f;

    [HideInInspector]
    public bool isFalling = false;
    
    //public LayerMask layersToStopFallingBridge;
    private Vector3 initialRotation;
    private Vector3 finalRotation;
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
            timer += Time.deltaTime;
            // Mathf.Pow for the exponential speed falling
            transform.localRotation = Quaternion.RotateTowards(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), Mathf.Pow(timer, speedFalling));

            if (transform.rotation.eulerAngles.x >= maxAngle)
            {
                TreeHasFallen();
            }
        }
    }

    public void TreeHasFallen()
    {
        isFalling = false;

        // Foreach gameObject in the FallingBridge, the layer is transformed into "Ground" layer
        foreach (Transform item in transform.parent.gameObject.GetComponentsInChildren<Transform>())
        {
            item.gameObject.layer = HarmonyLayers.LAYER_GROUND;
        }
        
        // Avoid another interactions
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If the bridge hits something (the ground in this case), it stops falling
        if (collider.gameObject.layer == HarmonyLayers.LAYER_GROUND)
        {
            TreeHasFallen();
        }
    }
}
