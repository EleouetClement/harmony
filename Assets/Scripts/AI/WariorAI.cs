using System;
using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;

public class WariorAI : AIAgent
{
    public DamageBox swordBox;

    private bool hasHit = false;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        swordBox.OnDamage.AddListener(OnDamagePlayer);
        swordBox.OnShielded.AddListener(OnShielded);
        swordBox.OnPerfectShielded.AddListener(OnPerfectShielded);
    }

    protected override void Update()
    {
        base.Update();
        animator.SetFloat(Speed, treeRunner.context.agent.velocity.magnitude);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        swordBox.active = true;
        hasHit = false;
    }

    public void FinishedAttack()
    {
        if (hasHit)
        {
            hasHit = false;
            treeRunner.tree.blackboard.currentState = "Chase";
        }
        else
        {
            treeRunner.tree.blackboard.currentState = "Chase";
        }
    }

    private void OnDamagePlayer()
    {
        hasHit = true;
    }

    private void OnShielded()
    {
        animator.SetTrigger("Impact");
    }

    private void OnPerfectShielded()
    {
        OnDamage(new DamageHit(100000));
        animator.SetTrigger("Impact");
    }
}
