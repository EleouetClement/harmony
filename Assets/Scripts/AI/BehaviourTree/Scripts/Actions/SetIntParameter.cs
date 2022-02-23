using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetIntParameter : ActionNode
{
    public string parameterName = "moveToPosition";
    public int value;

    protected override void OnStart() { }
    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        blackboard.SetParameter(parameterName, value);
        return State.Success;
    }
}
