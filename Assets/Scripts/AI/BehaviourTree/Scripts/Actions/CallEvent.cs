using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class CallEvent : ActionNode
    {
        public string eventName;

        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!context.treeRunner.customEvents.ContainsKey(eventName)) return State.Failure;

            context.treeRunner.customEvents[eventName].Invoke();
            return State.Success;
        }
    }
}