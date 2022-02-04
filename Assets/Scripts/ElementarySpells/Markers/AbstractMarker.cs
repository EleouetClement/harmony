using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a market that is used by the player to aim his spells
/// </summary>
public abstract class AbstractMarker
{
    /// <summary>
    /// Marker behaviour, how it should display itself in the scene
    /// and how it returns the target position/reference
    /// </summary>
    public abstract void DisplayTarget();
}
