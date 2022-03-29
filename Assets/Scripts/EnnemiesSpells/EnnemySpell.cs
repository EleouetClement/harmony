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

    public GameObject Target;

    private float chargeTimer;

    /// <summary>
    /// true if the spell is done charging
    /// </summary>
    public bool charged { get;  protected set; } = false;

    /// <summary>
    /// Starts the spell charge timer after instanciating the spell visual
    /// </summary>
    /// <param name="chargeTime"> maximum duration for the spell charge</param>
    public abstract void Charge(float chargeTime);

    /// <summary>
    /// Comes after the Charge(float chargeTime) method. Defines the spell behaviour
    /// </summary>
    public abstract void Launch();



}
