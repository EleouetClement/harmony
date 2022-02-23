using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetVectorParameter : ActionNode
{
    public string parameterName = "moveToPosition";
    public Vector3 value;

    protected override void OnStart() { }
    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        blackboard.SetParameter(parameterName, value);
        return State.Success;
    }
}
