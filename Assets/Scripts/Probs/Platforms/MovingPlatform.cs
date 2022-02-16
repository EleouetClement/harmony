using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : AbstractPlatform
{
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] [Min(0)] private float activationDelay;
    [SerializeField] [Min(0)] private float lerpInterpolationValue = 2f;
    private GameObject platform;
    private int direction = 1;
    Vector3 newPosition;

    private float borneMin;
    private float borneMax;
    private void Awake()
    {
        platform = transform.GetChild(0).gameObject;
        //borneMax = maxHeight - transform.position.y;
        //borneMin = minHeight + transform.position.y;
       
    }

    protected override void TriggeredAction()
    {
        if(direction == 1 && Mathf.Abs(platform.transform.localPosition.y - maxHeight) <= 0.1f)
        {
            direction = -1;
            Debug.Log("Max height reached");
        }
        else
        {
            if(direction == -1 && Mathf.Abs(platform.transform.localPosition.y - minHeight) <= 0.1f)
            {
                direction = 1;
                Debug.Log("Min height reached");
            }
        }

        if(direction == 1)
        {
            newPosition = new Vector3(platform.transform.localPosition.x, maxHeight, platform.transform.localPosition.z);
        }
        else
        {
            newPosition = new Vector3(platform.transform.localPosition.x, minHeight, platform.transform.localPosition.z);
        }

        platform.transform.localPosition = Vector3.Lerp(platform.transform.localPosition, newPosition, Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }
}
