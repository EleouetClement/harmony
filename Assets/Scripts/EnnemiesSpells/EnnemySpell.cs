using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines ennemy spell structure according to the AI architecture
/// </summary>
public abstract class EnnemySpell : MonoBehaviour
{

    public enum CastType
    {
        quick,
        charge
    }

    public AbstractSpell.Element element;

    public DamageHit damagesDeal{ get; protected set; }

    [Header("base timers")]
    [SerializeField] private float livingTime;
    [SerializeField] private float quickCastDuration;
    [SerializeField] private float chargeCastDuration;

    protected CastType currentCast;

    protected Transform summonerPosition;

    //public GameObject Target;

    [HideInInspector]
    public Vector3 target;

    protected float chargeTimer { get; private set; }
    private float lifeTimer;

    protected float CastObjective { get; private set; }
    /// <summary>
    /// true if the spell is done charging
    /// </summary>
    public bool charged { get;  protected set; } = false;

    protected bool customTarget = false;



    /// <summary>
    /// Starts the spell charge timer after instanciating the spell visual
    /// </summary>
    /// <param name="chargeTime"> maximum duration for the spell charge</param>
    public virtual void Charge(CastType chargeTime, Transform spellOrigin)
    {
        if(chargeTime.Equals(CastType.charge))
        {
            CastObjective = chargeCastDuration;
        }
        else
        {
            CastObjective = quickCastDuration;
        }     
        summonerPosition = spellOrigin;
        target = GameModeSingleton.GetInstance().GetPlayerReference.transform.position;
    }

    public virtual void Charge(CastType chargeTime, Transform spellOrigin, Vector3 targetPosition)
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
        if (!charged)
        {
            if (chargeTimer < CastObjective)
            {
                chargeTimer += Time.fixedDeltaTime;
            }
            else
            {
                charged = true;
                OnChargeEnd();
            }
        }
        else if(lifeTimer < livingTime)
        {
            lifeTimer += Time.fixedDeltaTime;
        }
        else
        {
            Terminate();
        }
    }
    /// <summary>
    /// Setup the spell before launching it.  
    /// </summary>
    protected abstract void OnChargeEnd();

    /// <summary>
    /// Clean the spell by destroying that needs to be.
    /// Additionnal operations should be added according to each spell.
    /// </summary>
    public abstract void Terminate();

    /// <summary>
    /// Check if the object is Idamageable then applies damages to it.
    /// </summary>
    /// <param name="objectHitted"></param>
    protected virtual void DealDamages(GameObject objectHitted)
    {
        objectHitted.GetComponent<IDamageable>().OnDamage(damagesDeal);
    }
}
