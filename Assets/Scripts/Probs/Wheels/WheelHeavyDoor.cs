using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelHeavyDoor : AbstractWheelSystem, IDamageable
{
    [Range(0, 1)]public float speedToOpen;
    public bool canBeClosedAgain;
    public bool isAffectedByGravity;
    
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
        Vector3 scale = objectToOpen.transform.localScale;
        finalSpawnScale = new Vector3(scale.x, 0f, scale.z);
        initialSpawnScale = new Vector3(scale.x, scale.y, scale.z);
        objectToOpen.transform.localScale = initialSpawnScale;
    }

    public void OnDamage(DamageHit hit)
    {
        rigidBody.velocity = hit.direction * spinForce * Time.deltaTime;
        rigidBody.AddForceAtPosition(hit.direction * spinForce * Time.deltaTime, hitPoint);

        previousRotation = currentRotation;
        currentRotation = transform.parent.gameObject.transform.rotation;

        Debug.Log("previous = " + previousRotation.eulerAngles.z);
        Debug.Log("current = " + currentRotation.eulerAngles.z);

        if (currentRotation.eulerAngles.z < previousRotation.eulerAngles.z)
        {
            Debug.Log("SENS HORAIIIIIIIIIIIRE");
            OpenDoor();
        }
        else if (currentRotation.eulerAngles.z > previousRotation.eulerAngles.z)
        {
            Debug.Log("SENS ANTI HORAIIIIIIIIRE");
            if (canBeClosedAgain)
            {
                CloseDoor();
            }
        }

        //Debug.Log("Position before = " + hitPoint);
        //Vector3 localPos = hitPointTransform.InverseTransformPoint(transform.position);
        //Debug.Log("Position after = " + localPos);
        //rigidBody.isKinematic = true;

        // Si la direction du beam va dans un sens (sens de la roue), on ouvre la porte, sinon on la referme

    }

    public override void OpenDoor()
    {
        if (!isTotallyOpened)
        {
            isTotallyOpened = false;
            isTotallyClosed = false;
            timer += Time.fixedDeltaTime;
            objectToOpen.transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer * speedToOpen);

            if (objectToOpen.transform.localScale.y <= 0)
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
            objectToOpen.transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer * speedToOpen);

            if (objectToOpen.transform.localScale.y >= initialSpawnScale.y)
            {
                isTotallyClosed = true;
            }
        }
    }



    //public override void OpenDoor(GameObject sourceObject, RaycastHit sourceRaycast)
    //{
    //    Debug.Log("OPEN THE DOOR");
    //    // si le currentDistancePoint du collider (water beam)
    //    if (sourceObject.CompareTag("WaterBeam"))
    //    {
    //        hitPoint = sourceObject.GetComponent<WaterBeam>().GetCurrentDistancePoint();
    //    }

    //    float y = transform.localPosition.y;
    //    float z = transform.localPosition.z;

    //    // Pour avoir les coordonnées de chaque carré, il faut prendre les coordonnées du transform de base
    //    // et y ajouter l'offset (pour obtenir le 2ème point)
    //    // Si le hitPoint est en haut à gauche, ou en en bas à droite, on tourne dans un sens (horaire)
    //    // Sinon si le hit point est en bas à gauche ou en haut à droite, on tourne dans l'autre sens (anti horaire)

    //    if ((hitPoint.y > y && hitPoint.z < z) || (hitPoint.y < y && hitPoint.z > z))
    //    {
    //        Debug.Log("SENS ANTI HORAIIIIIIIIIIIRE");
    //        transform.Rotate(transform.localRotation.x - speedToOpen * Time.deltaTime, 0, 0);
    //    }
    //    else
    //    {
    //        Debug.Log("SENS HORAIIIIIIIIIIIIIIIRE");

    //        // 0 degré + speedToOpen * Time.deltaTime

    //        //transform.localRotation = Quaternion.RotateTowards(Quaternion.Euler(initialRotation), Quaternion.Euler(finalRotation), Mathf.Pow(timer, speedFalling));
    //        //Vector3 angle = 
    //        transform.Rotate(transform.localRotation.x + speedToOpen * Time.deltaTime, 0, 0);
    //        //Debug.Log("Rotation x = " + transform.rotation.x + " / y = " + transform.rotation.y + "z = " + transform.rotation.z);
    //        //Vector3 rot = new Vector3(transform.rotation.x + (speedToOpen * Time.deltaTime), transform.rotation.y, transform.rotation.z);
    //        //transform.rotation = Quaternion.Euler(rot);

    //    }
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    Debug.Log("TRIGGER");
    //    if(collider.CompareTag("WaterBeam"))
    //    {
    //        // si le currentDistancePoint du collider (water beam)
    //        hitPoint = collider.gameObject.GetComponent<WaterBeam>().GetCurrentDistancePoint();
    //        float y = transform.position.y;
    //        float z = transform.position.z;

    //        // Pour avoir les coordonnées de chaque carré, il faut prendre les coordonnées du transform de base
    //        // et y ajouter l'offset (pour obtenir le 2ème point)
    //        // Si le hitPoint est en haut à gauche, ou en en bas à droite, on tourne dans un sens (horaire)
    //        // Sinon si le hit point est en bas à gauche ou en haut à droite, on tourne dans l'autre sens (anti horaire)

    //        if ((hitPoint.y > y && hitPoint.z < z) || (hitPoint.y < y && hitPoint.z > z))
    //        {
    //            Debug.Log("SENS HORAIIIIIIIIIIIRE");
    //        }
    //        else
    //        {
    //            Debug.Log("SENS ANTI HORAIIIIIIIIRE");
    //        }

    //    }
    //}


}
