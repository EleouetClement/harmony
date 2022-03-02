using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define elementary behavior while casting an instance of this spell class
/// </summary>
public abstract class AbstractSpell : MonoBehaviour
{

    public enum Element
    {
        Fire,
        Water,
        Earth,
        Physical,
    }

    public Element element;
    public Damages damagesInfos;
    /// <summary>
    /// The target location of the spell. Contains an arbitrary value that may differ spell to spell, but usually corresponds to where the spell is aimed at.
    /// </summary>
    public Vector3 target { get; protected set; }

    /// <summary>
    /// Reference to the player orbitine elementary
    /// </summary>
    public GameObject elementary { get; protected set; }


    protected AbstractMarker marker;

    /// <summary>
    /// Charge of the spell, in seconds.
    /// </summary>
    public float charge { get; private set; } = 0f;


    /// <summary>
    /// Maximum cast charge duration
    /// </summary>
    protected float maxCastTime = 5f;

    /// <summary>
    /// if the spell input at the blinkTiming time, isBlinked is true
    /// </summary>
    protected float blinkTiming = 3f;

    /// <summary>
    /// Maximum living time for the spell
    /// </summary>
    public float maxLivingTime { get; private set; } = 3f;

    private float currentCastTime = 0f;

    private float currentLivingTime = 0f;

    /// <summary>
    /// true if the player release the input with the right timing
    /// </summary>
    protected bool isBlinked = false;

    /// <summary>
    /// True if the spell has been canceled by the shield cast
    /// </summary>
    [HideInInspector]
    public bool canceled = false;

    private bool chargeend = false;

    private Transform playerMesh;

    protected DamageHit damages;



	private void Update()
	{
        //smooth turning when charging a spell and not moving
        if (!chargeend)
            playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, GameModeSingleton.GetInstance().GetCinemachineCameraController.rotation.y, 0), Time.deltaTime * GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerMotionController>().turnSpeed);
    }

    public virtual void FixedUpdate()
    {
        if (!chargeend)
        {
            charge += Time.fixedDeltaTime;
            PlayerGameplayController player = GameModeSingleton.GetInstance()?.GetPlayerReference?.GetComponent<PlayerGameplayController>();
            if (player) {
                player.OnManaSpend(GetChannelCost() * Time.fixedDeltaTime);
            }
        }

        //Debug.Log(isReleased() + " " + currentLivingTime + " " + currentCastTime);
        if (isReleased())
        {
            //Debug.Log(isReleased() + " " + currentLivingTime + " " + currentCastTime);
            currentLivingTime += Time.fixedDeltaTime;
            if (currentLivingTime >= maxLivingTime)
            {
                // Debug.Log("Terminate");
                Terminate();
            }
        }
        else
        {
            currentCastTime += Time.fixedDeltaTime;
            if (currentCastTime >= maxCastTime)
            {
                //Debug.Log("Liberation forcee");
                OnRelease();
            }
        }
    }

    /// <summary>
    /// Common initializer for all player cast spells.
    /// </summary>
    /// <param name="elemRef">Reference to the elementaryControler instance</param>
    /// <param name="target">Hit to the target of the spell. Might or might not be used, and usually determines a direction</param>
    public virtual void init(GameObject elemRef, Vector3 target)
    {
        this.target = target;
        elementary = elemRef;
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
        PlayerGameplayController player = GameModeSingleton.GetInstance()?.GetPlayerReference?.GetComponent<PlayerGameplayController>();
        if (player)
        {
            player.OnManaSpend(GetManaCost());
        }
    }

    public void OnRelease()
    {
        chargeend = true;
        onChargeEnd(charge);
    }

    /// <returns>True if this spell has already been released</returns>
    public bool isReleased()
    {
        return chargeend;
    }

    /// <summary>
    /// Kills this spell instance, disposes all ressources used by this spell and returns elemntary to 
    /// the player.
    /// </summary>
    public abstract void Terminate();

    /// <summary>
    /// Event triggered by The player controller to notify that the spell cast button has been released. 
    /// Do note that there is no garentee that this event is ever called, as the player might not ever release the key.
    /// </summary>
    /// <param name="chargetime">The time this spell has been charged for, in seconds. Because this is abstract, this is the duration of the key press.</param>
    protected virtual void onChargeEnd(float chargetime)
    {
        float blink = Mathf.Abs(blinkTiming - chargetime);
        //Debug.Log(blink);
        if (blink <= 0.5f)
        {
            Debug.Log("Blink!");
            isBlinked = true;
        }
    }

    /// <summary>
    /// Computes and return this spell's initial mana cost. May be overriden by spells for complex and situational behavior.
    /// </summary>
    protected virtual float GetManaCost()
    {
        return 20f;
    }

    /// <summary>
    /// Computes and return this spell's chanelling mana cost per second. May be overriden by spells for complex and situational behavior.
    /// </summary>
    protected virtual float GetChannelCost()
    {
        return 5f;
    }

    /// <summary>
    /// Creates DamageHit instance According to the spell type and chargeTime
    /// DamageHit direction will be updated when the spell hits
    /// </summary>
    public virtual DamageHit SetDamages(Vector3 direction)
    {
        if(this.damages == null)
        {
            DamageHit damages = new DamageHit(damagesInfos.baseDamages, element, direction);
            if (isBlinked)
            {
                damages.damage *= damagesInfos.blinkMultiplier;
            }
            else
            {
                float multiplier = (damagesInfos.maxMultiplier / maxCastTime) * charge;
                damages.damage *= multiplier;
            }
            this.damages = damages;
            this.damages.type = element;
        }
        else
        {
            this.damages.direction = direction;
        } 
        return this.damages;
    }

}
