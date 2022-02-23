using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RandomPosition : ActionNode
{
    [BlackboardParam(Blackboard.ParameterType.Vector)]
    public string parameterName = "moveToPosition";
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        Vector3 destination = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.y, max.y));
        if (blackboard.vectorParameters.ContainsKey(parameterName))
            blackboard.vectorParameters[parameterName] = destination;
        return State.Success;
    }
}
