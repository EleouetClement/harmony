using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetAnimatorTrigger : ActionNode
{
    public string parameterName;

    private int parameterHash;

    protected override void OnStart()
    {
        parameterHash = Animator.StringToHash(parameterName);
    }

    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        if (context.animator)
        {
            context.animator.SetTrigger(parameterHash);
            return State.Success;
        }

        return State.Failure;
    }
}
