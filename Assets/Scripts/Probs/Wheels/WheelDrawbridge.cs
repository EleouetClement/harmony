using UnityEngine;

public class WheelDrawbridge : AbstractWheelSystem
{
    public float maxAngle;

    private Vector3 initialRotation;
    private Vector3 finalRotation;

    private void Awake()
    {
        rigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();

        initialRotation = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
        finalRotation = new Vector3(maxAngle, transform.localRotation.y, transform.localRotation.z);
    }

    public override void OpenDoor()
    {
        Debug.Log("Open door");

        timer += Time.deltaTime;
        // Mathf.Pow for the exponential speed falling
        doorToOpen.transform.localRotation = Quaternion.RotateTowards(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), Mathf.Pow(timer, speedToOpen));
        //doorToOpen.transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer * speedToOpen);

        if (transform.localRotation.eulerAngles.x >= maxAngle)
        {
            DrawbridgeHasFallen();
        }
    }

    public override void CloseDoor()
    {
        Debug.Log("Close door");
    }

    public void DrawbridgeHasFallen()
    {
        isTotallyOpened = true;

        // The layer of the door is transformed into "Ground" layer
        doorToOpen.gameObject.layer = HarmonyLayers.LAYER_GROUND;

        // Avoid another interactions
        Destroy(doorToOpen.GetComponent<BoxCollider>());
        Destroy(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If the bridge hits something (the ground in this case), it stops falling
        if (collider.gameObject.layer == HarmonyLayers.LAYER_GROUND)
        {
            DrawbridgeHasFallen();
        }
    }

}
