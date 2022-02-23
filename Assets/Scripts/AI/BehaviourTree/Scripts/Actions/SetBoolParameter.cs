using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetBoolParameter : ActionNode
{
    public string parameterName = "moveToPosition";
    public bool value;

    protected override void OnStart() { }
    protected override void OnStop() { }

    protected override State OnUpdate() {
        blackboard.SetParameter(parameterName,value);
        return State.Success;
    }
}
