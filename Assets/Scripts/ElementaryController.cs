using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryController : MonoBehaviour
{

    [Header("Elementary positionning")]
    [SerializeField][Range(-2, 2)] private float verticalOffset = 0;
    [SerializeField][Range(-2, 2)] private float horizontalOffset = 0;
    [SerializeField][Range(-2, 2)] private float forwardOffset = 0;
    [SerializeField][Min(0)]       private float lerpInterpolationValue= 4;
    [Header("Elementary stats")]
    [SerializeField] [Range(0, 50)] private float maxDistance = 10;
    [SerializeField] [Range(0, 50)] private float travellingSpeed = 5;
    [SerializeField] private int layerMask;

    /// <summary>
    /// true if the element handles itself
    /// </summary>
    private bool computePosition = false;
    private bool hasShoulder = false;

    /// <summary>
    /// false is the elementary is 
    /// </summary>
    private bool attack;
    private Transform shoulder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!computePosition)
        {
            Orbit();
        }
        else
        {
            
        }
    }

    /// <summary>
    /// The elementary throw itself straigth forward
    /// </summary>
    public void AttackForward(Vector3 target)
    {   
        //TO MOVE -> SPELL CLASS
        RaycastHit hit;
        if(Physics.Raycast(transform.position, target,out hit ,maxDistance, layerMask))
        {
            computePosition = true;
            attack = true;

        }
    }

    /// <summary>
    /// Recall the elementary next to the player
    /// </summary>
    public void Recall()
    {

    }

    /// <summary>
    /// Fly near the player using a lerp.
    /// The Origin is the elementary current position
    /// the destination is calculated with the shoulder position and the x, y and z offset
    /// </summary>
    public void Orbit()
    {
        if(hasShoulder)
        {
            Vector3 newPosition = new Vector3(shoulder.position.x + horizontalOffset, shoulder.position.y + verticalOffset, shoulder.position.z + forwardOffset);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime*lerpInterpolationValue);
        }     
    }

    /// <summary>
    /// Set the elementary Anchor
    /// </summary>
    /// <param name="shouldersTransform"></param>
    public void SetShoulderOrigin(Transform shouldersTransform)
    {
        if (shouldersTransform == null)
        {
            Debug.LogError("SetShoulderOrigin : shoulder ref is null");
            return;
        }
        
        hasShoulder = true;
        shoulder = shouldersTransform;
    }

    //private void MoveToTarget()
    //{
    //    gameObject.GetComponent<Rigidbody>().MovePosition(target);     
    //}
    
}
