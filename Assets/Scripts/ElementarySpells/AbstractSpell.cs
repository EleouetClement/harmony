using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define elementary behavior while casting an instance of this spell class
/// </summary>
public abstract class AbstractSpell : MonoBehaviour
{

    public Vector3 target { get; protected set; }

    public GameObject elementary { get; protected set; }

    public float charge { get; private set; } = 0f;

    public virtual void FixedUpdate() {
        charge += Time.fixedDeltaTime;
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

    }

    /// <summary>
    /// Event triggered by The player controller to notify that the spell cast button has been released. 
    /// Do note that there is no garentee that this event is ever called, as the player might not ever release the key.
    /// </summary>
    /// <param name="chargetime">The time this spell has been charged for, in seconds. Because this is abstract, this is the duration of the key press.</param>
    protected abstract void onChargeEnd(float chargetime);

}
