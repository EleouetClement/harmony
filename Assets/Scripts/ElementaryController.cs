using System;
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

    [Header("Spells (One spell per Array for each element)")]
    [SerializeField] public AbstractSpell shieldPrefab;
    [SerializeField] private AbstractSpell[] spellsLeft;
    [SerializeField] private AbstractSpell[] spellsRight;

    [Header("Dev")]
    [SerializeField][Range(0, 1)] float sphereCastRadius;
    [SerializeField][Range(0, 1)] float sphereCastMaxDistance;

    //Contains 1 spell per element
    public Dictionary<AbstractSpell.Element, AbstractSpell> spells1;

    //Idem
    public Dictionary<AbstractSpell.Element, AbstractSpell> spells2;

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
        InitDict();
    }

    private void InitDict()
    {
        spells1 = new Dictionary<AbstractSpell.Element, AbstractSpell>();
        spells2 = new Dictionary<AbstractSpell.Element, AbstractSpell>();
        if(spellsLeft == null || spellsLeft.Length ==0)
        {
            Debug.LogError("InitDict : offensiveSpells not initialized");
        }
        else
        {
            foreach (AbstractSpell s in spellsLeft)
            {
                spells1.Add(s.element, s);
            }
        }
        if(spellsRight == null || spellsRight.Length == 0)
        {
            Debug.LogError("InitDict : exploratorySpells not initialized");
        }
        else
        {
            foreach (AbstractSpell s in spellsRight)
            {
                spells2.Add(s.element, s);
            }
        }     
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
            RaycastHit hitinfo;
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hitinfo, sphereCastMaxDistance))
            {
                AvoidObstacle(hitinfo);
            }
            else
            {
                Orbit();
            }
            
        }
    }

	private void FixedUpdate()
    {
        
    }

    public void SetElement(AbstractSpell.Element element)
    {
        currentElement = element;
    }

    /// <summary>
    /// Returns the spell 1 linked to the current element
    /// </summary>
    /// <returns></returns>
    public AbstractSpell GetSpell1()
    {
        AbstractSpell s;
        if (spells1.TryGetValue(currentElement, out s))
            return s;
        Debug.LogError("GetOffensiveSpell : no spell1 found for this element");
        return null;
    }

    /// <summary>
    /// Returns the spell 2 linked to the current element
    /// </summary>
    /// <returns></returns>
    public AbstractSpell GetSpell2()
    {
        AbstractSpell s;
        if(spells2.TryGetValue(currentElement, out s))
            return s;
        Debug.LogError("GetExploratorySpell : no spell2 found for this element");
        return null;
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
            transform.position = Vector3.Lerp(transform.position, virtualShoulder.transform.position, Time.deltaTime * lerpInterpolationValue);
        }     
    }

    /// <summary>
    /// Calculate the direction the elem needs to take to avoid an obstacle
    /// </summary>
    /// <param name="hit"></param>
    public void AvoidObstacle(RaycastHit hit)
    {

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

    /// <summary>
    /// Restart back the position compute and set the currentSpell to null 
    /// </summary>
    public void Reset()
    {
        Recall();
        currentSpell = null;
        readyToCast = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.layer.Equals(HarmonyLayers.LAYER_ENEMYSPELL) && !collision.gameObject.layer.Equals(HarmonyLayers.LAYER_PLAYERSPELL) && computePosition)
        {
            Debug.Log("Changement de position");
            computePosition = false;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        float dist = Vector3.Distance(virtualShoulder.transform.position, GameModeSingleton.GetInstance().GetPlayerReference.transform.position);
        float playerObstacle = Vector3.Distance(GameModeSingleton.GetInstance().GetPlayerReference.transform.position, collision.transform.position);
        Debug.Log("distance epaule/personnage " + dist + " Distance personnage obstacle " + playerObstacle);
        if (dist < playerObstacle)
        {
            Debug.Log("Compute retablie");
            computePosition = true;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }
}
