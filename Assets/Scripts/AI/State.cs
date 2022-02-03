using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI
{
    [CreateAssetMenu(menuName = "Harmony/AI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transitions;
        public Color sceneGizmoColor = Color.grey;

        public void UpdateState(AIAgent controller)
        {
            DoActions(controller);
            CheckTransitions(controller);
        }

        private void DoActions(AIAgent controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        private void CheckTransitions(AIAgent controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    controller.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    controller.TransitionToState(transitions[i].falseState);
                }
            }
        }


    }
}
