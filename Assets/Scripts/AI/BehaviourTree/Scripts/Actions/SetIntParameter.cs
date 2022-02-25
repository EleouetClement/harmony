using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class SetIntParameter : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Int)]
        public string parameterName = "";

        public int value;

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