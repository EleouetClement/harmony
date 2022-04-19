using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI{
    public class CheckHealth : DecoratorNode
    {
        public ComparisonType comparison;
        public float value;
        private AIAgent agent;

        protected override void OnStart()
        {
            agent = context.gameObject.GetComponent<AIAgent>();
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (agent)
            {
                switch (comparison)
                {
                    case ComparisonType.Equal: return agent.health == value ? State.Success : State.Failure;
                    case ComparisonType.Greater: return agent.health > value ? State.Success : State.Failure;
                    case ComparisonType.GreaterOrEqual: return agent.health >= value ? State.Success : State.Failure;
                    case ComparisonType.Less: return agent.health < value ? State.Success : State.Failure;
                    case ComparisonType.LessOrEqual: return agent.health <= value ? State.Success : State.Failure;
                    case ComparisonType.NotEqual: return agent.health != value ? State.Success : State.Failure;
                    default: return State.Failure;
                }
            }
            return State.Failure;
        }
    }
}