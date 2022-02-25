using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryController : MonoBehaviour
{

    [Header("Elementary positionning")]
    [SerializeField] private GameObject virtualShoulder;
    [SerializeField][Range(-2, 2)] private float horizontalOffset = 0;
    [SerializeField][Range(-2, 2)] private float verticalOffset = 0;
    [SerializeField][Range(-2, 2)] private float forwardOffset = 0;
    [SerializeField] [Min(0)] private float lerpInterpolationValue;
    [SerializeField][Min(0)]  private float awayInterpolationValue = 4;
    [SerializeField][Min(0)]  private float orbitingInterpolationValue = 4;

	[SerializeField] private float isAwayDistance;
	[Header("Elementary stats")]
    //[SerializeField] [Range(0, 50)] private float maxDistance = 10;
    //[SerializeField] [Range(0, 50)] private float travellingSpeed = 5;
    [SerializeField] private int layerMask;

    [SerializeField] public AbstractSpell shieldPrefab;
    [SerializeField] public AbstractSpell[] offensiveSpells;
    [SerializeField] public AbstractSpell[] exploratorySpells;

    private Vector3 shoulderOffset;
    [Min(0)] public float safetyDistance;
    [Min(1)] public float repulseStrength;
    private Vector3 orbitingVelocity;

    public bool inCombat = false;
    public bool isAiming = false;

    /// <summary>
    /// true if the element handles itself
    /// </summary>
    public bool computePosition = true;
    private bool hasShoulder = false;

    private Transform shoulder;
    private Transform playerMesh;

    public bool isAway { get; private set; } = false;

    [HideInInspector]
    public AbstractSpell currentSpell = null;
    public bool readyToCast = true;
    public AbstractSpell.Element currentElement;

	private void Awake()
	{
		shoulderOffset = new Vector3(horizontalOffset, verticalOffset, forwardOffset);
    }

	// Start is called before the first frame update
	void Start()
    {
        SetElement(AbstractSpell.Element.Fire);
        virtualShoulder.transform.localPosition = shoulderOffset;
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
        Vector3 shoulderXZ = new Vector3(virtualShoulder.transform.position.x, 0f, virtualShoulder.transform.position.z);
        Vector3 transformXZ = new Vector3(shoulderOffset.x, 0f, shoulderOffset.z);
        safetyDistance = Vector3.Distance(shoulderXZ, transformXZ);
    }

    // Update is called once per frame
    void Update()
    {

        //Testing purposes
        virtualShoulder.transform.localPosition = shoulderOffset;
        if (Input.GetKeyDown(KeyCode.C))
        {
            inCombat ^= true;
        }
    }

	private void LateUpdate()
	{
        shoulderOffset = new Vector3(horizontalOffset, verticalOffset, forwardOffset);
        if (computePosition)
        {
            
            Orbit();
        }
    }

	private void FixedUpdate()
    {
        
    }

    public void SetElement(AbstractSpell.Element element)
    {
        currentElement = element;
    }

    public AbstractSpell GetOffensiveSpell()
    {
        return offensiveSpells[(int)currentElement];
    }

    public AbstractSpell GetExploratorySpell()
    {
        return exploratorySpells[(int)currentElement];
    }

    /// <summary>
    /// The elementary throw itself straigth forward
    /// </summary>
    public void CastSpell(AbstractSpell spell)
    {
        currentSpell = spell;
        readyToCast = false;
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
            //Vector3 newPosition = new Vector3(shoulder.position.x + horizontalOffset, shoulder.position.y + verticalOffset, shoulder.position.z + forwardOffset);
            //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime*lerpInterpolationValue);

            //Vector3 shoulderPosition = virtualShoulder.transform.position + Random.insideUnitSphere * 5f;
            transform.position = Vector3.Lerp(transform.position, virtualShoulder.transform.position /*+ Repulse()*/, Time.deltaTime * lerpInterpolationValue);
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

    /// <summary>
    /// If the elementary is away from the character
    /// </summary>
    public void Recall()
    {
        computePosition = true;
    }

    /// <summary>
    /// returns true if the elementary isn't close to the summoner
    /// </summary>
    /// <returns></returns>
    public bool IsElementaryAway()
    {
        //Vector3 basePosition = new Vector3(shoulder.position.x + horizontalOffset, shoulder.position.y + verticalOffset, shoulder.position.z + forwardOffset);
        return Vector3.Distance(transform.position, virtualShoulder.transform.position) > isAwayDistance ? true : false;
    }

    private Vector3 Repulse()
    {
        if (readyToCast)
        {
            Vector3 playerRelativePos = new Vector3(playerMesh.position.x, 0f, playerMesh.position.z);
            Vector3 transformRelativePos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 repulseSource = transform.position - (transformRelativePos - playerRelativePos);
            float playerDistance = Vector3.Distance(repulseSource, transform.position);
            //print("playerRelativePos : "+playerRelativePos+" elem position : "+transform.position+" repulseSource : "+repulseSource+" distance : " + playerDistance);
            if (playerDistance < safetyDistance)
            {
                print("repulsio!");
                return (transform.position - repulseSource) * safetyDistance/playerDistance;    
            }
        }
        return Vector3.zero;
    }

	private void OnDrawGizmos()
	{
        Vector3 playerRelativePos = new Vector3(playerMesh.position.x, 0f, playerMesh.position.z);
        Vector3 transformRelativePos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 repulseSource = transform.position - (transformRelativePos - playerRelativePos);
        Gizmos.color = Color.red;
		Gizmos.DrawSphere(repulseSource, 0.05f);
		Gizmos.color = Color.green;
        Gizmos.DrawSphere(playerRelativePos, 0.05f);
    }

}
