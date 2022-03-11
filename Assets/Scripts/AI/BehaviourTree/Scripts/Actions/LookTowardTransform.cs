using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class LookTowardTransform : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string parameterName = "target";

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return State.Success;
        }
    }
}