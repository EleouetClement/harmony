using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;

public class WariorAI : AIAgent
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    protected override void Update()
    {
        base.Update();
        animator.SetFloat(Speed, treeRunner.context.agent.velocity.magnitude);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void FinishedAttack()
    {
        treeRunner.tree.blackboard.currentState = "Chase"; 
    }
}
