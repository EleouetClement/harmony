using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetAnimatorBool : ActionNode
{
    public string parameterName;
    public bool value;

    private int parameterHash;

    protected override void OnStart()
    {
        parameterHash = Animator.StringToHash(parameterName);
    }

    protected override void OnStop() { }

    protected override State OnUpdate() {
        if (context.animator)
        {
            context.animator.SetBool(parameterHash, value);
            return State.Success;
        }

        return State.Failure;
    }
}
