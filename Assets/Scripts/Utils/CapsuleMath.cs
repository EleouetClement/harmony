using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMath
{

    /// <summary>
    /// Computes the two edge points of this capsule collider, in world space. 
    /// The two edge points are the points in the middle of the spheres formed by this capsule's bounds.
    /// </summary>
    /// <param name="capsule">The capsule to do the math on.</param>
    /// <param name="top">True to return the bottom point, false to return the top one.</param>
    /// <returns></returns>
    public static Vector3 ComputeEdges(CapsuleCollider capsule, bool top)
    {
        float hprime = (capsule.height / 2) - capsule.radius;
        Vector3 kekw = capsule.direction switch { 0 => capsule.transform.right, 1 => capsule.transform.up, 2 => capsule.transform.forward } * hprime;
        return capsule.transform.position + (top ? 1 : -1) * kekw;
    }
}
