using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class DistanceToTransform : DecoratorNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string parameterName;

        public bool greaterThan;
        public float distance;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            blackboard.GetParameter(parameterName, out Transform target);
            bool isLess = Vector3.Distance(context.transform.position, target.position) < distance;

            if (isLess && !greaterThan || !isLess && greaterThan)
            {
                return child.Update();
            }

            return State.Failure;
        }

        public override string GetName()
        {
            return $"Distance to {parameterName} {(greaterThan?">":"<")} {distance}";
        }
    }
}