using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI
{
    [CreateAssetMenu(menuName = "Harmony/AI/Actions/Chase")]
    public class ChaseAction : Action
    {
        public override void Act(AIAgent controller)
        {
            Chase(controller);
        }

        private void Chase(AIAgent controller)
        {
            controller.navMeshAgent.destination = controller.chaseTarget.position;
            controller.navMeshAgent.isStopped = false;
        }

#if UNITY_EDITOR
        public override void DrawGizmos(AIAgent controller)
        {
            Gizmos.DrawLine(controller.transform.position,controller.chaseTarget.position);
        }
#endif
    }
}
