using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class StopMove : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate()
        {
            context.agent.SetDestination(context.transform.position);
            return State.Success;
        }
    }
}