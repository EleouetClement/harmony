using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class SetAgentSpeed : ActionNode
    {
        public float speed = 3.5f;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate()
        {
            if (!context.agent) return State.Failure;
            context.agent.speed = speed;
            return State.Success;
        }

        public override string GetName()
        {
            return $"Set Speed to {speed}";
        }
    }
}