using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class LookTowardTransform : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string parameterName = "target";

        public float speed = 3;

        private Transform target;

        protected override void OnStart() {
            target = null;
            blackboard.GetParameter(parameterName, out target);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 lookPos = target.position - context.transform.position;
            lookPos.y = 0;

            Quaternion rotation = Quaternion.Lerp(context.transform.rotation,Quaternion.LookRotation(lookPos),Time.deltaTime*speed);
            context.transform.rotation = rotation;

            if (Vector3.Dot(lookPos.normalized, context.transform.forward) < 0.95f)
            {
                return State.Running;
            }

            return State.Success;
        }

        public override string GetName()
        {
            return $"Look Toward {parameterName}";
        }
    }
}