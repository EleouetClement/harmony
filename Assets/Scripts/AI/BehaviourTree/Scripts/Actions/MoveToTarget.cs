using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class MoveToTarget : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string parameterName = "target";

        public float speed = 5;
        public float stoppingDistance = 0.1f;
        public bool updateRotation = true;
        public float acceleration = 40.0f;
        public float tolerance = 1.0f;

        private Transform target;

        protected override void OnStart()
        {
            context.agent.stoppingDistance = stoppingDistance;
            context.agent.speed = speed;

            target = null;
            if (blackboard.GetParameter(parameterName, out target))
                context.agent.destination = target.position;

            context.agent.updateRotation = updateRotation;
            context.agent.acceleration = acceleration;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (!target) return State.Failure;

            if (Vector3.Distance(target.position, context.agent.destination) > tolerance)
                context.agent.destination = target.position;

            if (context.agent.pathPending) return State.Running;

            if (context.agent.remainingDistance < tolerance) return State.Success;

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) return State.Failure;

            return State.Running;
        }

        public override string GetName()
        {
            return $"Move Toward {parameterName}";
        }
    }
}