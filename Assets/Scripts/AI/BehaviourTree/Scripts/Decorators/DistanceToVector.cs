using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class DistanceToVector : DecoratorNode
{
    [BlackboardParam(Blackboard.ParameterType.Vector)]
    public string parameterName;
    public bool greaterThan;
    public float distance;

    protected override void OnStart() { }
    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        blackboard.GetParameter(parameterName, out Vector3 target);
        bool isLess = Vector3.Distance(context.transform.position, target) < distance;

        if (isLess && !greaterThan || !isLess && greaterThan)
        {
            return child.Update();
        }

        return State.Failure;
    }
}
