using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a plaftform used for any any enigma on the game
/// </summary>
public abstract class AbstractPlatform : MonoBehaviour
{
    protected bool triggered = false;

    private void Update()
    {
        if(triggered)
        {
            TriggeredAction();
        }
    }

    /// <summary>
    /// Defines the behaviour of the platform once activated
    /// </summary>
    protected abstract void TriggeredAction();
}
