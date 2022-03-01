using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Content of a hit against an IdamageableEntity, with additionnal information about the hit.<br>
/// Reactional behavior should be managed by the entity implementing IDamageable, this class only contains information.
/// </summary>
public class DamageHit
{
    /// <summary>
    /// Damage of the hit, in healthpoints.
    /// </summary>
    public float damage = 0f;
    /// <summary>
    /// Type of the damage hit. Default is physical.
    /// </summary>
    public AbstractSpell.Element type = AbstractSpell.Element.Physical;
    /// <summary>
    /// Hinted direction of the hit. This hints at a direction for knockback, visual effects such as destruction...<br>
    /// Defaults to Vector3.zero.
    /// </summary>
    public Vector3 direction = Vector3.zero;

    public DamageHit(float damage)
    {
        this.damage = damage;
    }

    public DamageHit(float damage, AbstractSpell.Element type)
    {
        this.damage = damage;
        this.type = type;
    }

    public DamageHit(float damage, Vector3 direction)
    {
        this.damage = damage;
        this.direction = direction;
    }

    public DamageHit(float damage, AbstractSpell.Element type, Vector3 direction)
    {
        this.damage = damage;
        this.type = type;
        this.direction = direction;
    }

    public override string ToString()
    {
        return "[Damage : " + damage + " / Type : " + type + " / Direction : " + direction + "]";
    }
}
