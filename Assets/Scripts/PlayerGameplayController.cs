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
    private Transform playerMesh;
    private CinemachineCameraController cinemachineCamera;

    public float castingTurnSpeed = 5f;

    [Header("Health settings")]
    [SerializeField] [Min(0)] private int maxHitsNumber = 1;
    [SerializeField] [Min(0)] private float hitResetTimer = 10;

    private float hitTimer = 0.0f;
    private int hitAmount = 0;

    public bool InFight { get; private set; } = false;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InitializeElementary();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
        cinemachineCamera = GameModeSingleton.GetInstance().GetCinemachineCameraController;
    }

    private void Update()
    {
        #region HealthManagement
        if (hitAmount > 0)
        {
            if (hitTimer >= hitResetTimer)
            {
                hitAmount = 0;
                Debug.Log("hit reset");
            }
            else
            {
                hitTimer += Time.deltaTime;
            }
        }
        #endregion
    }

    // Update is called once per frame
    void LateUpdate()
	{
		cameraCheck();
	}


	/// <summary>
	/// Makes sure the right camera is active depending on elementary state
	/// </summary>
	private void cameraCheck()
	{
		if (elementaryController.inCombat && elementaryController.isAiming)
		{
			playerCinemachineCameraController.ZoomIn();
		}
		else if (elementaryController.inCombat)
		{
			playerCinemachineCameraController.CombatCam();
		}
		else
		{
			playerCinemachineCameraController.ExploCam();
		}
	}

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
				CastWaterBeam(spell);
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
				playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, cinemachineCamera.rotation.y, 0), Time.deltaTime * castingTurnSpeed);
				AbstractSpell spell = Instantiate(elementaryController.GetExploratorySpell(), elementaryController.transform.position, Quaternion.identity);
				switch (elementaryController.currentElement)
				{
					case AbstractSpell.Element.Fire:
						CastFireOrb(spell);
						break;
					case AbstractSpell.Element.Water:
						CastWaterBeam(spell);
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
        //Debug.Log("Blocking");
        if (elementaryController.currentSpell == null)
        {
            Debug.Log("shield activation");
            AbstractSpell spell = Instantiate(elementaryController.shieldPrefab, elementaryController.transform.position, Quaternion.identity);
            spell.init(elementaryController.gameObject, Vector3.zero);
            elementaryController.currentSpell = spell;
        }
		else
        {
			if(!elementaryController.currentSpell.isReleased())
            {
				Debug.Log("Annulation par shield");
				elementaryController.currentSpell.Terminate();
				AbstractSpell spell = Instantiate(elementaryController.shieldPrefab, elementaryController.transform.position, Quaternion.identity);
				spell.init(elementaryController.gameObject, Vector3.zero);
				elementaryController.currentSpell = spell;
			}		
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

	private void CastWaterBeam(AbstractSpell spell)
    {
		Debug.LogWarning(GameModeSingleton.GetInstance().GetCinemachineCameraController);
		spell.init(elementaryController.gameObject, Vector3.zero);
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

    private void OnAim(InputValue value)
    {
        elementaryController.isAiming = value.isPressed;
    }

    private void OnSwitchPlayMode(InputValue value)
    {
        elementaryController.inCombat = !elementaryController.inCombat;
    }

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

    /// <returns>A representation of the player current recovery in health value, between 0f and 1f</returns>
    public float getDisplayHP()
    {
        if (hitAmount <= 0)
            return 1f;
        float lifeperhit = 1 / maxHitsNumber;
        float toreturn = (maxHitsNumber - hitAmount) * lifeperhit;
        float regen = lifeperhit * (hitTimer/hitResetTimer);
        return toreturn + regen;
    }

    public void OnDamage(DamageHit hit)
    {
        CinemachineImpulseSource source = GetComponent<CinemachineImpulseSource>();
        source.GenerateImpulse();
        hitAmount++;
        if (hitAmount > maxHitsNumber)
        {
            Debug.Log("Player dead");
        }
        //DEAD SCENE TO LOAD...
    }
}
