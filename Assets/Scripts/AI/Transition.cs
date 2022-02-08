using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI
{
    [System.Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;

        public bool ComputeDecision(AIAgent controller)
        {
            return decision.Decide(controller);
        }
    }
}
