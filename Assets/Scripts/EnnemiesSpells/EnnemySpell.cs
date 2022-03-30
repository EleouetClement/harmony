using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines ennemy spell structure according to the AI architecture
/// </summary>
public abstract class EnnemySpell : MonoBehaviour
{
    public AbstractSpell.Element element;

    public DamageHit damagesDeal{ get; protected set; }

    protected Transform summonerPosition;

    //public GameObject Target;

    public Vector3 target;

    private float chargeTimer;

    private float castObjective;
    /// <summary>
    /// true if the spell is done charging
    /// </summary>
    public bool charged { get;  protected set; } = false;

    protected bool customTarget = false;

    /// <summary>
    /// Starts the spell charge timer after instanciating the spell visual
    /// </summary>
    /// <param name="chargeTime"> maximum duration for the spell charge</param>
    public virtual void Charge(float chargeTime, Transform spellOrigin)
    {
        castObjective = chargeTime;
        summonerPosition = spellOrigin;
    }

    public virtual void Charge(float chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        Charge(chargeTime, spellOrigin);
        target = targetPosition;
        customTarget = true;
    }

    /// <summary>
    /// Spell base Behaviour
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if(!charged)
        {
            if (chargeTimer < castObjective)
            {
                chargeTimer += Time.fixedDeltaTime;
            }
            else
            {
                charged = true;
                OnChargeEnd();
            }
        }    
    }

    /// <summary>
    /// Setup the spell before launching it.  
    /// </summary>
    protected abstract void OnChargeEnd();
}
