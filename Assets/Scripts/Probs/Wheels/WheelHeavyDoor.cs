using UnityEngine;

public class WheelHeavyDoor : AbstractWheelSystem, IDamageable
{
    [Range(0, 1)] public float speedToOpen;
    public bool canBeClosedAgain;
    //public bool isAffectedByGravity;
    
    private float timer = 0f;
    private Rigidbody rigidBody;
    private bool isTotallyOpened = false;
    private bool isTotallyClosed = true;
    private Vector3 initialSpawnScale;
    private Vector3 finalSpawnScale;
    private Quaternion previousRotation;
    private Quaternion currentRotation;

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

    public void OnDamage(DamageHit hit)
    {
        rigidBody.velocity = hit.direction * spinForce * Time.deltaTime;
        rigidBody.AddForceAtPosition(hit.direction * spinForce * Time.deltaTime, hitPoint);

        //Debug.Log("Rotation : " + rigidBody.angularVelocity); 

        // Check the wheel rotation to know the rotation direction
        previousRotation = currentRotation;
        currentRotation = transform.parent.gameObject.transform.rotation;

        // If the beam direction goes in a direction, the wheel opens the door, if it goes in the other direction, it closes the door
        // If clockwise direction --> open --- else if anti-clockwise direction --> close
        if (currentRotation.eulerAngles.z < previousRotation.eulerAngles.z)
        {
            OpenDoor();
        }
        else if (currentRotation.eulerAngles.z > previousRotation.eulerAngles.z)
        {
            if (canBeClosedAgain)
            {
                CloseDoor();
            }
        }
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

    public void CloseDoor()
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
