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
            if (isNot)
                return blackboard.currentState != aiState ? State.Success : State.Failure;
            return blackboard.currentState == aiState ? State.Success : State.Failure;
        }
    }
}