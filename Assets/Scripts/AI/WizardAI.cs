using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;

public class WizardAI : AIAgent
{
    [SerializeField] EnnemySpell spell;
    [SerializeField] Transform castPosition;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private EnnemySpell currentSpell;
    private bool attacking = false;

    protected override void Update()
    {
        base.Update();
        animator.SetFloat(Speed, treeRunner.context.agent.velocity.magnitude);
        castPosition.LookAt(GameModeSingleton.GetInstance().GetPlayerReference.transform);

        if (attacking && currentSpell && currentSpell.charged)
        {
            FinishedAttack();
        }
    }

    public void Attack()
    {
        if (!attacking)
        {
            currentSpell = Instantiate(spell, castPosition.position, Quaternion.identity);
            currentSpell.Charge(EnnemySpell.CastType.quick, castPosition);
            animator.SetTrigger("Attack");
            attacking = true;
        }
        
    }

    public void AttackCharged()
    {
        if (!attacking)
        {
            currentSpell = Instantiate(spell, castPosition.position, Quaternion.identity);
            currentSpell.Charge(EnnemySpell.CastType.charge, castPosition);
            animator.SetTrigger("Attack");
            attacking = true;
        }
    }

    public void FinishedAttack()
    {
        attacking = false;
        treeRunner.tree.blackboard.currentState = "Chase"; 
    }
}
