using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableLedgeBehavior : MonoBehaviour
{

    /// <summary>
    /// Box collider for the sourcce of this ledge grab.
    /// </summary>
    GameObject launchBox;
    [HideInInspector]
    public bool cantrigger = false;

    public float TotalclimbTime = 1.3f;
    public float animationCutoff = 0.7f;
    public float heightOverbuffer = 0.5f;
    public Vector3 direction = Vector3.forward;

    /// <summary>
    /// Time of the animation in seconds, between 0 and cimbTime. -1 if the animation has not started yet.
    /// </summary>
    private float animationTimeLocale = -1f;
    private Vector3 playerinitPos;


    void LateUpdate()
    {
        if (animationTimeLocale >= 0f)
        {
            Transform playertransform = GameModeSingleton.GetInstance().GetPlayerReference.transform;
            animationTimeLocale += Time.deltaTime;
            // TODO : trigger animations
            if (animationTimeLocale <= animationCutoff)
            { // Grab animation
                float timeratio = Mathf.Clamp(animationTimeLocale / animationCutoff, 0, 1);
                Vector3 newpos = playerinitPos * (1 - timeratio) + computeCutoffPosition() * timeratio;
                playertransform.position = newpos;
                Debug.Log(newpos);
            }
            else
            { // Pull over animation
                float timeratio = Mathf.Clamp((animationTimeLocale - animationCutoff) / (TotalclimbTime - animationCutoff), 0, 1);
                Vector3 newpos = computeCutoffPosition() * (1 - timeratio) + computeLandingPosition() * timeratio;
                playertransform.position = newpos;
            }
            

            // Stops the animation if timing has ended.
            if (animationTimeLocale >= TotalclimbTime)
                TriggerEnd();
        }
    }

    /// <summary>
    /// Triggers this ClimbableLedge to move the player charactet for climbTime seconds.
    /// </summary>
    public void Trigger()
    {
        Debug.Log("Started climbing ledge");
        animationTimeLocale = 0f;
        playerinitPos = GameModeSingleton.GetInstance().GetPlayerReference.transform.position;
        // TODO : set the player state in motion controller
    }

    private void TriggerEnd()
    {
        Debug.Log("Stopped climbing ledge");
        animationTimeLocale = -1f;
        // TODO: reset the motion controller
    }

    public Vector3 computeLandingPosition()
    {
        return transform.position + Vector3.up * heightOverbuffer + direction.normalized;
    }
    public Vector3 computeCutoffPosition()
    {
        return transform.position - direction.normalized / 3;
    }

    private void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * heightOverbuffer);

        Vector3 top = CapsuleMath.ComputeEdges(GetComponent<CapsuleCollider>(), true);
        Vector3 bot = CapsuleMath.ComputeEdges(GetComponent<CapsuleCollider>(), false);
        Gizmos.DrawLine(top, bot);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.up * heightOverbuffer, computeLandingPosition());
        Gizmos.DrawLine(computeCutoffPosition(), computeCutoffPosition() + Vector3.up * 0.2f);
    }

}
