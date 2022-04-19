using UnityEngine;

public class WheelDrawbridge : AbstractWheelSystem
{
    public float maxAngle;

    private Vector3 initialRotation;
    private Vector3 finalRotation;

    private void Awake()
    {
        rigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();

        initialRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        finalRotation = new Vector3(maxAngle, transform.rotation.y, transform.rotation.z);
    }

    public override void OpenDoor()
    {
        if(!isTotallyOpened)
        {
            isTotallyClosed = false;

            timer += Time.deltaTime;
            //doorToOpen.transform.localRotation = Quaternion.RotateTowards(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), Mathf.Pow(timer, speedToOpen));
            doorToOpen.transform.localRotation = Quaternion.Euler(Vector3.Lerp(initialRotation, finalRotation, timer * speedToOpen));
            if (doorToOpen.transform.rotation.eulerAngles.x >= maxAngle)
            {
                DrawbridgeHasFallen();
            }

            if (doorToOpen.transform.rotation.eulerAngles.x >= maxAngle)
            {
                isTotallyOpened = true;

                if (!canBeClosedAgain)
                {
                    DrawbridgeHasFallen();
                }
            }
        }
    }

    public override void CloseDoor()
    {
        if (!isTotallyClosed)
        {
            isTotallyOpened = false;

            timer -= Time.deltaTime;
            //doorToOpen.transform.localRotation = Quaternion.RotateTowards(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), Mathf.Pow(timer, speedToOpen));
            doorToOpen.transform.localRotation = Quaternion.Euler(Vector3.Lerp(initialRotation, finalRotation, timer * speedToOpen));

            if (doorToOpen.transform.rotation.eulerAngles.x <= 0)
            {
                isTotallyClosed = true;
            }
        }
    }

    public void DrawbridgeHasFallen()
    {
        isTotallyOpened = true;

        // The layer of the Bridge is transformed into "Ground" layer
        // Foreach gameObject in the Drawbridge, the layer is transformed into "Ground" layer
        foreach (Transform item in doorToOpen.transform.gameObject.GetComponentsInChildren<Transform>())
        {
            item.gameObject.layer = HarmonyLayers.LAYER_GROUND;
        }

        // Avoid another interactions
        //Destroy(doorToOpen.GetComponent<BoxCollider>());
        //Destroy(this);
    }
}
