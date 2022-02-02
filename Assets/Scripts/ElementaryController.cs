using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryController : MonoBehaviour
{

    [SerializeField][Range(-2, 2)] private float verticalOffset = 0;
    [SerializeField][Range(-2, 2)] private float horizontalOffset = 0;
    [SerializeField][Range(-2, 2)] private float forwardOffset = 0;
    [SerializeField][Min(0)]  private float lerpInterpolationValue= 4;


    private bool isAway = false;
    private bool hasShoulder = false;
    private Transform shoulder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAway)
        {
            Orbit();
        }
    }

    /// <summary>
    /// The elementary throw itself straigth forward
    /// </summary>
    public void AttackForward(Vector3 target)
    {
        isAway = true;
    }

    /// <summary>
    /// Recall the elementary next to the player
    /// </summary>
    public void Return()
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
}
