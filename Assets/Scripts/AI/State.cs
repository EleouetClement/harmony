using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI
{
    [CreateAssetMenu(menuName = "Harmony/AI/State")]
    public class State : ScriptableObject
    {
        public Action[] enterActions;
        public Action[] updateActions;
        public Action[] exitActions;
        public Transition[] transitions;
        public Color sceneGizmoColor = Color.grey;

        public void EnterState(AIAgent controller)
        {
            for (int i = 0; i < enterActions.Length; i++)
            {
                enterActions[i].Act(controller);
            }
        }

        public void UpdateState(AIAgent controller)
        {
            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Act(controller);
            }

            CheckTransitions(controller);
        }

        public void ExitState(AIAgent controller)
        {
            for (int i = 0; i < exitActions.Length; i++)
            {
                exitActions[i].Act(controller);
            }
        }

        private void CheckTransitions(AIAgent controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].ComputeDecision(controller);

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
