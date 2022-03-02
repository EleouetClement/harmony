using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class SetBoolParameter : ActionNode
    {
        [BlackboardParam(Blackboard.ParameterType.Bool)]
        public string parameterName = "";

        public bool value;

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