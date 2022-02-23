using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToPosition : ActionNode
{
    [BlackboardParam(Blackboard.ParameterType.Vector)]
    public string parameterName = "moveToPosition";
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;

        if (blackboard.vectorParameters.TryGetValue(parameterName, out Vector3 destination))
            context.agent.destination = destination;

        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.agent.pathPending) {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            return State.Failure;
        }

        return State.Running;
    }
}
