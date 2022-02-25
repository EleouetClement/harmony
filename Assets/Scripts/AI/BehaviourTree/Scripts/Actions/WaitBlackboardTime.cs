using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class WaitBlackboardTime : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Float)]
        public string durationParameter;

        public float duration;
        float startTime;

        protected override void OnStart()
        {
            startTime = Time.time;
            blackboard.GetParameter(durationParameter, out duration);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}