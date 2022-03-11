using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class CheckState : DecoratorNode
    {
        [BlackboardState]
        public string aiState;
        public bool isNot;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if ((isNot && blackboard.currentState != aiState) || (!isNot && blackboard.currentState == aiState))
            {
                child.Update();
                return State.Success;
            }

            return State.Failure;
        }

        public override string GetName()
        {
            if (isNot)
                return $"Is State Not {aiState}?";
            return $"Is State {aiState}?";
        }
    }
}