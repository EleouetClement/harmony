using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class SetVectorParameter : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Vector)]
        public string parameterName = "";

        public Vector3 value;

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