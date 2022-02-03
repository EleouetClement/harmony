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

    [SerializeField] public AbstractSpell[] spells;

    /// <summary>
    /// true if the element handles itself
    /// </summary>
    public bool computePosition = true;
    private bool hasShoulder = false;

    private Transform shoulder;

    public AbstractSpell currentSpell;

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
        if (computePosition)
        {
            Orbit();
        }
    }

    /// <summary>
    /// The elementary throw itself straigth forward
    /// </summary>
    public void CastSpell(AbstractSpell spell)
    {
        currentSpell = spell;
        computePosition = false;
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
    /// Set player position for this elementary
    /// </summary>
    /// <param name="shouldersTransform"></param>
    public void SetPLayerOrigin(Transform shouldersTransform)
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
