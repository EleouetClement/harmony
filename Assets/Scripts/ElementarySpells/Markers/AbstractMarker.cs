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
    protected Transform origin;
    /// <summary>
    /// Marker behaviour, how it should display itself in the scene
    /// and how it returns the target position/reference
    /// </summary>
    public abstract void DisplayTarget(Vector3 direction);

    /// <summary>
    /// pseudo constructor
    /// </summary>
    /// <param name="maxRayCastDistance"></param>
    public virtual void Init(float maxRayCastDistance, GameObject prefab, Transform origin)
    {
        this.maxRayCastDistance = maxRayCastDistance;
        markerPrefab = prefab;
        this.origin = origin;
    }

    public abstract void OnDestroy();

}
