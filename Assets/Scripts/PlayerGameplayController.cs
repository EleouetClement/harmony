using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplayController : MonoBehaviour, IDamageable
{
    [Header("Elementary")]
    [SerializeField] GameObject elementaryObjectReference;
    [SerializeField] GameObject playerMeshReference;
    [SerializeField] private CinemachineCameraController playerCinemachineCameraController;
    private ElementaryController elementaryController;

    [Header("Health stats")]
    [SerializeField][Min(0)] private int maxHitNumber = 2;
    [SerializeField][Min(0)] private float timeBeforeLifeReset = 10;

    private GameModeSingleton gm;

    public bool InFight { get; private set; } = false;

    private int hits = 0;
    private float hitTimer = Mathf.Epsilon;
    private bool wounded = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gm = GameModeSingleton.GetInstance();
        InitializeElementary();
    }
    // Start is called before the first frame update
    void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        #region Health timer managment
        if (wounded)
        {
            if(hitTimer < timeBeforeLifeReset)
            {
                hitTimer += Time.deltaTime;
            }
            else
            {
                wounded = false;
                hits = 0;
            }
        }
        #endregion
		if (elementaryController.inCombat)
		{
			playerCinemachineCameraController.CombatCam();
		}
		else
		{
			playerCinemachineCameraController.ExploCam();
		}
	}
	// Update is called once per frame

	/// <summary>
	/// Modify the current spell of the elementary based on input pressed
	/// 1 : fire || 2 : water || 3 : earth
	/// </summary>
	/// <param name="value"></param>
	private void OnElementSelect(InputValue value)
	{
		if (value.Get<Vector2>() == Vector2.left)
		{
			elementaryController.SetElement(AbstractSpell.Element.Fire);
			elementaryController.transform.GetChild(0).gameObject.GetComponent<Light>().color = Color.red;
		}
		if (value.Get<Vector2>() == Vector2.up)
		{
			elementaryController.SetElement(AbstractSpell.Element.Water);
			elementaryController.transform.GetChild(0).gameObject.GetComponent<Light>().color = Color.blue;
		}
		if (value.Get<Vector2>() == Vector2.right)
		{
			elementaryController.SetElement(AbstractSpell.Element.Earth);
			elementaryController.transform.GetChild(0).gameObject.GetComponent<Light>().color = Color.yellow;
		}
		print("Element sélectionné : "+elementaryController.currentElement);
	}

	private void OnSpellLeft(InputValue value)
	{
		if (elementaryController.readyToCast)
		{
			if (value.isPressed)
			{
				AbstractSpell spell = null;
				if (elementaryController.inCombat)
				{
					spell = Instantiate(elementaryController.GetOffensiveSpell(), elementaryController.transform.position, Quaternion.identity);
					CastOffensiveSpell(spell);
				}
				else
				{
					spell = Instantiate(elementaryController.GetExploratorySpell(), elementaryController.transform.position, Quaternion.identity);
					CastExploratorySpell(spell);
				}

				
				elementaryController.CastSpell(spell);
			}
		}
		if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
			elementaryController.currentSpell?.OnRelease();

	}

	private void CastOffensiveSpell(AbstractSpell spell)
	{
		switch (elementaryController.currentElement)
		{
			case AbstractSpell.Element.Fire:
				CastFireBall(spell);
				break;
			case AbstractSpell.Element.Water:
				CastWaterMissiles(spell);
				break;
			case AbstractSpell.Element.Earth:
				CastEarthMortar(spell);
				break;
			default:
				break;
		}
	}

	private void CastExploratorySpell(AbstractSpell spell)
	{
		switch (elementaryController.currentElement)
		{
			case AbstractSpell.Element.Fire:
				CastFireOrb(spell);
				break;
			case AbstractSpell.Element.Water:

				break;
			case AbstractSpell.Element.Earth:
				CastEarthWall(spell);
				break;
			default:
				break;
		}
	}

	private void OnSpellRight(InputValue value)
	{
		if (elementaryController.readyToCast)
		{
			if (value.isPressed)
			{
				AbstractSpell spell = Instantiate(elementaryController.GetExploratorySpell(), elementaryController.transform.position, Quaternion.identity);
				switch (elementaryController.currentElement)
				{
					case AbstractSpell.Element.Fire:
						CastFireOrb(spell);
						break;
					case AbstractSpell.Element.Water:
						
						break;
					case AbstractSpell.Element.Earth:
						CastEarthWall(spell);
						break;
					default:
						break;
				}
				elementaryController.CastSpell(spell);
				Debug.Log("Spell cast : " + spell);
			}
		}
		if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
			elementaryController.currentSpell?.OnRelease();
	}

	private void CastFireOrb(AbstractSpell spell)
	{
		throw new NotImplementedException();
	}
    /// <summary>
    /// Input reserved for the shield that always needs to be available as a spell
    /// </summary>
    /// <param name="value"></param>
    private void OnBlock(InputValue value)
    {
        if (elementaryController.currentSpell == null)
        {
            Debug.Log("Blocking");
            AbstractSpell spell = Instantiate(elementaryController.shieldPrefab, elementaryController.transform.position, Quaternion.identity);
            spell.init(elementaryController.gameObject, Vector3.zero);
            elementaryController.currentSpell = spell;   
        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
            elementaryController.currentSpell?.OnRelease();
    }

	private void CastEarthWall(AbstractSpell spell)
	{
		spell.init(elementaryController.gameObject, Vector3.zero);
	}

	private void CastWaterMissiles(AbstractSpell spell)
	{
		spell.init(elementaryController.gameObject, Vector3.zero);
		if (spell is WaterMissiles)
		{
			Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 200f, 1 << HarmonyLayers.LAYER_TARGETABLE);
			if (enemies.Length >= 1)
				((WaterMissiles)spell).targetTransform = enemies[0].gameObject.transform;
		}
	}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(HarmonyLayers.LAYER_ENEMYSPELL))
        {

        }
    }

    /// <summary>
    /// Behaviour when hit by an EnnemySpell
    /// </summary>
    /// <param name="hit"></param>
    public void OnDamage(DamageHit hit)
    {
        CinemachineCameraController cam = gm.GetCinemachineCameraController;
        hits += 1;
        if(hits >= maxHitNumber)
        {
            Debug.Log("Player Death");
        }
        else
        {
            Debug.Log("Player hurt");
            wounded = true; 
            CinemachineImpulseSource source = GetComponent<CinemachineImpulseSource>();
            source.GenerateImpulse();
        }
    }


	private void CastFireBall(AbstractSpell spell)
	{
		if (playerCinemachineCameraController)
		{
			spell.init(elementaryController.gameObject, playerCinemachineCameraController.GetViewDirection);

		}
		else
		{
			spell.init(elementaryController.gameObject, playerCinemachineCameraController.GetViewDirection);

		}
	}

	private void CastEarthMortar(AbstractSpell spell)
	{
		spell.init(elementaryController.gameObject, Vector3.zero);
	}

	//private void OnSpellLeft(InputValue value)
	//{
	//	if (elementaryController.currentSpell == null)
	//	{
	//		if (value.isPressed)
	//		{
	//			// TODO : Find a smarter way to instanciate the right spell here.
	//			AbstractSpell s = Instantiate(elementaryController.spells[0], elementaryController.transform.position, Quaternion.identity);
	//			s.init(elementaryController.gameObject, Vector3.zero);
	//			if (s is WaterMissiles)
	//			{
	//				Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 200f, 1 << HarmonyLayers.LAYER_TARGETABLE);
	//				if (enemies.Length >= 1)
	//					((WaterMissiles)s).targetTransform = enemies[0].gameObject.transform;
	//			}
	//			elementaryController.CastSpell(s);
	//		}

	//	}
	//	if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
	//		elementaryController.currentSpell?.OnRelease();
	//}

	//private void OnSpellRight(InputValue value)
	//{
	//	if (elementaryController.currentSpell == null)
	//	{
	//		if (value.isPressed)
	//		{
	//			AbstractSpell spell = Instantiate(elementaryController.spells[1], elementaryController.transform.position, Quaternion.identity);
	//			if (playerCinemachineCameraController)
	//			{
	//				spell.init(elementaryController.gameObject, playerCinemachineCameraController.GetViewDirection);

	//			}
	//			else
	//			{
	//				spell.init(elementaryController.gameObject, playerCameraController.GetViewDirection);

	//			}
	//			elementaryController.CastSpell(spell);
	//		}
	//	}
	//	//Debug.Log(value.isPressed);

	//	if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
	//	{
	//		//Debug.Log("liberation tim�e");
	//		elementaryController.currentSpell?.OnRelease();
	//	}
	//}

	//private void OnInteract(InputValue value)
	//{
	//	if (elementaryController.currentSpell == null)
	//	{
	//		if (value.isPressed)
	//		{
	//			//Debug.Log("EarthMortar");
	//			AbstractSpell spell = Instantiate(elementaryController.spells[2], elementaryController.transform.position, Quaternion.identity);
	//			spell.init(elementaryController.gameObject, Vector3.zero);
	//			elementaryController.currentSpell = spell;
	//		}
	//	}
	//	if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
	//		elementaryController.currentSpell?.OnRelease();
	//}

	/// <summary>
	/// Set the Elementary shoulder reference
	/// </summary>
	private void InitializeElementary()
	{
		if (elementaryObjectReference == null)
		{
			Debug.LogError("PlayerMotionController : Missing Elementary reference");
		}
		else
		{
			elementaryController = elementaryObjectReference.GetComponent<ElementaryController>();
			if (elementaryController == null)
			{
				Debug.LogError("PlayerMotionController : Current elementary hasn't any ElementaryController component");
			}
			else
			{
				if (playerMeshReference == null)
				{
					Debug.LogError("PlayerMotionController : Missing Mesh reference");
				}
				else
				{
					elementaryController.SetPLayerOrigin(playerMeshReference.transform);
				}
			}

		}

	}
}
