using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a market that is used by the player to aim his spells
/// </summary>
public abstract class AbstractMarker : MonoBehaviour
{

    protected float maxRayCastDistance { get; private set; }
    protected GameObject markerPrefab;
    /// <summary>
    /// Marker behaviour, how it should display itself in the scene
    /// and how it returns the target position/reference
    /// </summary>
    public abstract void DisplayTarget(Vector3 direction, Vector3 origin);

    /// <summary>
    /// pseudo constructor
    /// </summary>
    /// <param name="maxRayCastDistance"></param>
    public virtual void Init(float maxRayCastDistance, GameObject prefab)
    {
        this.maxRayCastDistance = maxRayCastDistance;
        markerPrefab = prefab;
    }

    public abstract void OnDestroy();

}
