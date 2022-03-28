using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class StopPatrolling : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            context.gameObject.GetComponent<AIAgent>().StopPatrolling();
            return State.Success;
        }
    }
}