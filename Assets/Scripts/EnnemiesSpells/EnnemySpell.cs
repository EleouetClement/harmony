using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines ennemy spell structure according to the AI architecture
/// </summary>
public abstract class EnnemySpell : MonoBehaviour
{
    public AbstractSpell.Element element;
    public Damages damagesInfos { get; private set; }

    protected Transform summonerPosition;

    public GameObject Target;

    private float chargeTimer;

    private float castObjective;
    /// <summary>
    /// true if the spell is done charging
    /// </summary>
    public bool charged { get;  protected set; } = false;

    /// <summary>
    /// Comes after the Charge(float chargeTime) method. Defines the spell behaviour
    /// </summary>
    public abstract void Launch();

    /// <summary>
    /// Starts the spell charge timer after instanciating the spell visual
    /// </summary>
    /// <param name="chargeTime"> maximum duration for the spell charge</param>
    public virtual void Charge(float chargeTime, Transform spellOrigin)
    {
        castObjective = chargeTime;
        summonerPosition = spellOrigin;
    }

    /// <summary>
    /// Spell base Behaviour
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (chargeTimer < castObjective && !charged)
        {
            chargeTimer += Time.fixedDeltaTime;
        }
        else
        {
            charged = true;
        }
    }


}
