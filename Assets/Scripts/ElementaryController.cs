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
    [SerializeField] GameObject debugSpherePrefab;
    [SerializeField][Range(0, 4)] float sphereCastRadius;
    [SerializeField][Range(0, 1)] float sphereCastMaxDistance;
    [SerializeField] [Range(2, 5)] float yAxisNewValue;
    [SerializeField] LayerMask layersToIgnore;


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
    private bool isReseting = false;

    /// <summary>
    /// true if the element handles itself
    /// </summary>
    public bool computePosition = true;
    private bool hasShoulder = false;

    public Transform shoulder {get; private set;}
    private Transform playerMesh;
    private GameObject debugSphereReference;

    public GameObject crossAirExt;
    /// <summary>
    /// true if the elementary is away from the player after a spell or inside something
    /// </summary>
    public bool isAway { get; private set; } = false;

    [HideInInspector]
    public AbstractSpell currentSpell = null;

    /// <summary>
    /// True if the elementary has return next to the player
    /// </summary>
    public bool readyToCast = true;
    public AbstractSpell.Element currentElement;

	private void Awake()
	{
		shoulderOffset = new Vector3(horizontalOffset, verticalOffset, forwardOffset);
        if(GameModeSingleton.GetInstance().debug)
            debugSphereReference = Instantiate(debugSpherePrefab, Vector3.zero, Quaternion.identity);
        
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
        if(GameModeSingleton.GetInstance().debug)
        {
            debugSphereReference.transform.parent = playerMesh;
            debugSphereReference.transform.localPosition = shoulderOffset;
        }      
        Vector3 shoulderXZ = new Vector3(virtualShoulder.transform.position.x, 0f, virtualShoulder.transform.position.z);
        Vector3 transformXZ = new Vector3(shoulderOffset.x, 0f, shoulderOffset.z);
        safetyDistance = Vector3.Distance(shoulderXZ, transformXZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!readyToCast && currentSpell == null)
        {      
            readyToCast = !IsElementaryAway();
            //Debug.Log("readyToCast aprs: " + readyToCast);
        }
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
        if (computePosition || (currentSpell != null && currentSpell.elementaryfollow))
        {
            
            Collider[] obstacles = Physics.OverlapSphere((shoulder.position + shoulderOffset), sphereCastRadius, layersToIgnore);
            if(GameModeSingleton.GetInstance().debug)
            {
                string debugs = "";
                foreach (Collider c in obstacles)
                {
                    debugs += c.gameObject.name;
                    debugs += " ";
                }
                Debug.Log(debugs);
            }         
            if (obstacles != null && obstacles.Length > 0)
            {
                //Debug.Log("Changement de position d'epaule nb d'obstacles : " + obstacles.Length);

                virtualShoulder.transform.localPosition = new Vector3(0, yAxisNewValue, 0);
            }
            else
            {
                virtualShoulder.transform.localPosition = shoulderOffset;
            }
            Orbit();
            
        }
    }

	private void FixedUpdate()
    {
        isAway = IsElementaryAway();
        if (!isAway && isReseting && currentSpell == null)
        {
            readyToCast = true;
            isReseting = false;
        }
    }

    public void SetElement(AbstractSpell.Element element)
    {
        if(readyToCast)
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
        isAway = true;
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
        readyToCast = !IsElementaryAway();
        isReseting = true;
    }
}
