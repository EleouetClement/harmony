using UnityEngine;

public class WheelHeavyDoor : AbstractWheelSystem
{
    private Vector3 initialSpawnScale;
    private Vector3 finalSpawnScale;

    private void Awake()
    {
        rigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();
        currentRotation = transform.parent.gameObject.transform.rotation;

        // Store the prefab scale to make it expand (initial and final scale values for expanding the platform)
        Vector3 scale = doorToOpen.transform.localScale;
        finalSpawnScale = new Vector3(scale.x, 0f, scale.z);
        initialSpawnScale = new Vector3(scale.x, scale.y, scale.z);
        doorToOpen.transform.localScale = initialSpawnScale;
    }

    public override void OpenDoor()
    {
        if (!isTotallyOpened)
        {
            isTotallyOpened = false;
            isTotallyClosed = false;
            timer += Time.fixedDeltaTime;
            doorToOpen.transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer * speedToOpen);

            // If the door is totally opened
            if (doorToOpen.transform.localScale.y <= 0)
            {
                isTotallyOpened = true;
            }
        }
    }

    public override void CloseDoor()
    {
        if (!isTotallyClosed)
        {
            isTotallyClosed = false;
            isTotallyOpened = false;
            timer -= Time.fixedDeltaTime;
            doorToOpen.transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer * speedToOpen);

            // If the door is totally closed
            if (doorToOpen.transform.localScale.y >= initialSpawnScale.y)
            {
                isTotallyClosed = true;
            }
        }
    }
}
