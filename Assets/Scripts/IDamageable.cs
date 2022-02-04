using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the damage behavior and focus mode of that entity.<br>
/// Entities having a script implementing IDamageable will be considered valid targets for elementary spells.
/// </summary>
public interface IDamageable
{

    /// <summary>
    /// Defines the damage effect of a hit on this entity.
    /// </summary>
    /// <param name="hit">Information about the hit</param>
    public abstract void OnDamage(DamageHit hit);

}
