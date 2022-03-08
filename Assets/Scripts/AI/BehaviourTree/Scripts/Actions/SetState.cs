using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class SetState : ActionNode
    {
        [BlackboardState]
        public string aiState;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            blackboard.currentState = aiState;
            return State.Success;
        }
    }
}