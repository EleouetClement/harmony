using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;

public class WariorAI : AIAgent
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    void Update()
    {
        base.Update();
        animator.SetFloat(Speed, context.agent.velocity.magnitude);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void FinishedAttack()
    {
        tree.blackboard.currentState = "Chase"; 
    }
}
