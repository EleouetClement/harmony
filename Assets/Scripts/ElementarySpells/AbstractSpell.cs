using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define elementary behavior while casting an instance of this spell class
/// </summary>
public abstract class AbstractSpell : MonoBehaviour
{

    /// <summary>
    /// The target location of the spell. Contains an arbitrary value that may differ spell to spell, but usually corresponds to where the spell is aimed at.
    /// </summary>
    public Vector3 target { get; protected set; }

    /// <summary>
    /// Reference to the player orbitine elementary
    /// </summary>
    public GameObject elementary { get; protected set; }

    /// <summary>
    /// Charge of the spell, in seconds.
    /// </summary>
    public float charge { get; private set; } = 0f;


    /// <summary>
    /// Maximum cast charge duration
    /// </summary>
    protected float maxCastTime = 4f;


    /// <summary>
    /// Maximum living time for the spell
    /// </summary>
    public float maxLivingTime { get; private set; } = 3f;

    private float currentCastTime = 0f;

    private float currentLivingTime = 0f;


    private bool chargeend = false;

    public virtual void FixedUpdate()
    {
        if (!chargeend)
            charge += Time.fixedDeltaTime;
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
    protected abstract void onChargeEnd(float chargetime);




}
