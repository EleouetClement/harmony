using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class SetAnimatorInt : ActionNode
    {
        public string parameterName;
        public int value;

        private int parameterHash;

        protected override void OnStart()
        {
            parameterHash = Animator.StringToHash(parameterName);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (context.animator)
            {
                context.animator.SetInteger(parameterHash, value);
                return State.Success;
            }

            return State.Failure;
        }
    }
}