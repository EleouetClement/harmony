using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class LookTowardTransform : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string parameterName = "target";

        private Transform target;

        protected override void OnStart() {
            target = null;
            if (blackboard.GetParameter(parameterName, out target))
                context.agent.destination = target.position;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 lookPos = target.position - context.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            context.transform.rotation = rotation;
            return State.Success;
        }
    }
}