using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetAnimatorFloat : ActionNode
{
    public string parameterName;
    public float value;

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
            context.animator.SetFloat(parameterHash, value);
            return State.Success;
        }

        return State.Failure;
    }
}
