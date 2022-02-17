using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : AbstractPlatform
{
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] [Min(0)] private float activationDelay;
    [SerializeField] [Min(0)] private float lerpInterpolationValue = 0.2f;
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float delay = 1f;
    private int direction = 1;
    Vector3 newPosition;
    
    private float borneMin;
    private float borneMax;
    private ElementaryController elem;
    private float coolDown = Mathf.Epsilon;
    protected override void TriggeredAction()
    {
        if(elem.transform.position != transform.position)
        {
            elem.transform.position = Vector3.MoveTowards(elem.transform.position, transform.position, Time.deltaTime);
        }
        if(direction == 1 && Mathf.Abs(platform.transform.localPosition.y - maxHeight) <= 0.1f)
        {
            direction = -1;
            coolDown = Mathf.Epsilon;
            //Debug.Log("Max height reached");
        }
        else
        {
            if(direction == -1 && Mathf.Abs(platform.transform.localPosition.y - minHeight) <= 0.1f)
            {
                direction = 1; 
                coolDown = Mathf.Epsilon;
                //Debug.Log("Min height reached");
            }
        }
        if(coolDown >= delay)
        {
            platform.transform.localPosition += Vector3.up * direction * speed * Time.deltaTime;
        }
        else
        {
            coolDown += Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag.Equals("Elementary"))
        {
            triggered = true;
            LockElementary(other.gameObject);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        this.elem = null;
        triggered = false;
    }

    private void LockElementary(GameObject elem)
    {
        Debug.Log("Stop spell");
        this.elem = elem.GetComponent<ElementaryController>();
        this.elem.currentSpell.Terminate();
        this.elem.computePosition = false;
    }
}
