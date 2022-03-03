using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class SetFloatParameter : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Float)]
        public string parameterName = "";

        public float value;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            blackboard.SetParameter(parameterName, value);
            return State.Success;
        }
    }
}