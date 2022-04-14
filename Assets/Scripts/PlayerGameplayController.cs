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

    [Header("Mana settings")]
    [SerializeField] [Min(0)] private float maxMana = 100f;
    [SerializeField] private float mana = 0;
    [SerializeField] [Min(0)] private float ManaRegenCooldown = 2f;
    [SerializeField] [Min(0)] private float ManaRegenPerSecond = 20f;
    [SerializeField] [Min(0)] private float ManaRegenPerSecondWhileBurnout = 15f;
    private float CurrentManaCooldown = 0;
    private Boolean manaburnout = false;

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
        #region ManaManagement
        // Regens mana when not casting
        if(!manaburnout)
            if (CurrentManaCooldown >= 0)
                CurrentManaCooldown -= Time.deltaTime;
            else
                mana = Mathf.Max(0, mana - (ManaRegenPerSecond * Time.deltaTime));
        // Enables mana burnout if oom
        if (mana > maxMana)
        {
            mana = maxMana;
            manaburnout = true;
            if(elementaryController.currentSpell)
            {
                elementaryController.currentSpell?.OnRelease();
            }
        }
        if (manaburnout)
        {
            mana = Mathf.Max(0, mana - (ManaRegenPerSecondWhileBurnout * Time.deltaTime));
            if (mana <= 0) manaburnout = false;
        }
        //Debug.LogWarning($"{mana} / {maxMana} : {mana / maxMana}, {manaburnout}");
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
        if (elementaryController.isAiming)
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
        //print("Element sélectionné : " + elementaryController.currentElement);
    }
    #region Spell casting

    private void OnSpellLeft(InputValue value)
    {
        if (elementaryController.readyToCast && !manaburnout)
        {
            if (value.isPressed)
            {
                elementaryController.readyToCast = false;
                AbstractSpell spell = Instantiate(
                        elementaryController.GetSpell1(),
                        elementaryController.transform.position,
                        Quaternion.identity);
                if (elementaryController.currentElement == AbstractSpell.Element.Water)
                {
                    CastWaterMissiles(spell);
                }
                else
                {
                    spell.init(elementaryController.gameObject, Vector3.zero);
                }
                elementaryController.CastSpell(spell);
            }
        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
            elementaryController.currentSpell?.OnRelease();
    }

    private void OnSpellRight(InputValue value)
    {
        if (elementaryController.readyToCast && !manaburnout)
        {
            if (value.isPressed)
            {
                playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, cinemachineCamera.rotation.y, 0), Time.deltaTime * castingTurnSpeed);
                AbstractSpell spell = Instantiate(
                    elementaryController.GetSpell2(), 
                    elementaryController.transform.position, 
                    Quaternion.identity);
                spell.init(elementaryController.gameObject, Vector3.zero);
                elementaryController.CastSpell(spell);
            }
        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
            elementaryController.currentSpell?.OnRelease();
    }

    /// <summary>
    /// Input reserved for the shield that always needs to be available as a spell
    /// </summary>
    /// <param name="value"></param>
    private void OnBlock(InputValue value)
    {
        //Debug.Log("Blocking");
        if (!manaburnout && value.isPressed)
        {
            if (elementaryController.currentSpell == null)
            {
                Debug.Log("shield activation");
                AbstractSpell spell = Instantiate(elementaryController.shieldPrefab, elementaryController.transform.position, Quaternion.identity);
                spell.init(elementaryController.gameObject, Vector3.zero);
                elementaryController.CastSpell(spell);
            }
            else
            {
                bool tmp = elementaryController.currentSpell is Shield;
                if (!elementaryController.currentSpell.isReleased() && !(elementaryController.currentSpell is Shield))
                {
                    Debug.Log("Annulation par shield");
                    elementaryController.currentSpell.canceled = true;
                    OnManaRegain(elementaryController.currentSpell.GetManaRegainAmount());
                    elementaryController.currentSpell.Terminate();
                    AbstractSpell spell = Instantiate(elementaryController.shieldPrefab, elementaryController.transform.position, Quaternion.identity);
                    spell.init(elementaryController.gameObject, Vector3.zero);
                    elementaryController.CastSpell(spell);
                }
            }         
        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
        {
            Debug.Log("leve shield");
            elementaryController.currentSpell?.OnRelease();
        }
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
    #endregion
    private void OnAim(InputValue value)
    {
        elementaryController.isAiming = value.isPressed;
    }

    private void OnInventory(InputValue value)
    {
        InventoryManager.instance.OpenInventory();
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

    #region Health management
    /// <returns>A representation of the player current recovery in health value, between 0f and 1f</returns>
    public float getDisplayHP()
    {
        if (hitAmount <= 0)
            return 1f;
        float lifeperhit = 1 / maxHitsNumber;
        float toreturn = (maxHitsNumber - hitAmount) * lifeperhit;
        float regen = lifeperhit * (hitTimer / hitResetTimer);
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
    #endregion
    #region mana management
    /// <summary>
    /// Event called when the player spends mana
    /// </summary>
    public void OnManaSpend(float m)
    {
        if (m > 0)
            CurrentManaCooldown = ManaRegenCooldown;
        mana += m;
    }

    /// <summary>
    /// Event calld when a spell is cancel and part of the mana needs to be regained
    /// </summary>
    /// <param name="m"></param>
    public void OnManaRegain(float m)
    {
        mana = (mana - m <= 0) ? mana : mana - m;       
    }

    public float getDisplayMana() {
        return 1 - (mana / maxMana);
    }

    public float GetMana()
    {
        return mana / 100;
    }

    /// <summary>
    /// Add the amount givent to the maximum mana the player can use
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseMana(int amount)
    {
        maxMana += amount;
    }
    #endregion
}
