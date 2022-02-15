using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI
{
    [CreateAssetMenu(menuName = "Harmony/AI/Decisions/Look")]
    public class LookDecision
    {
        [Layer] public int searchLayer;
        public LayerMask blockMask;

        public bool Decide(AIAgent controller)
        {
            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(AIAgent controller)
        {
            RaycastHit hit;

            float radius = controller.lookRange * Mathf.Tan(Mathf.Deg2Rad * controller.lookAngle);

            if (Physics.BoxCast(controller.eyes.position, new Vector3(radius,radius,0.1f), controller.eyes.forward, out hit, Quaternion.LookRotation(controller.eyes.forward), controller.lookRange, 1 << searchLayer))
            {
                Vector3 targetDirection = (hit.collider.transform.position - controller.eyes.position).normalized;

                if (Mathf.Acos(Vector3.Dot(controller.eyes.forward, targetDirection))*Mathf.Rad2Deg <= controller.lookAngle)
                {
                    Debug.DrawLine(controller.eyes.position, hit.collider.transform.position, Color.yellow);
                    if (Physics.Raycast(controller.eyes.position, targetDirection, out hit,
                        controller.lookRange, blockMask.value | searchLayer) && hit.collider.gameObject.layer == searchLayer)
                    {
                        Debug.DrawLine(controller.eyes.position, hit.collider.transform.position, Color.green);
                        controller.chaseTarget = hit.transform;
                        return true;
                    }
                }
            }
            return false;
        }

#if UNITY_EDITOR
        public void DrawGizmos(AIAgent controller)
        {
            float radius = controller.lookRange * Mathf.Tan(Mathf.Deg2Rad * controller.lookAngle);
            Vector3 viewDir = controller.eyes.forward * controller.lookRange;
            Vector3 viewEnd = controller.eyes.position + viewDir;

            Vector3 leftSide = controller.eyes.position +
                               Quaternion.AngleAxis(controller.lookAngle, Vector3.up) * viewDir;
            Vector3 rightSide = controller.eyes.position +
                               Quaternion.AngleAxis(-controller.lookAngle, Vector3.up) * viewDir;

            Vector3 circleCenter = leftSide + (rightSide - leftSide) / 2.0f;

            Handles.DrawWireDisc(circleCenter, controller.eyes.forward, (rightSide- leftSide).magnitude/2);

            Handles.DrawWireArc(controller.eyes.position, controller.eyes.up, controller.eyes.forward, controller.lookAngle, controller.lookRange);
            Handles.DrawWireArc(controller.eyes.position, controller.eyes.up, controller.eyes.forward, -controller.lookAngle, controller.lookRange);
            Handles.DrawWireArc(controller.eyes.position, controller.eyes.right, controller.eyes.forward, controller.lookAngle, controller.lookRange);
            Handles.DrawWireArc(controller.eyes.position, controller.eyes.right, controller.eyes.forward, -controller.lookAngle, controller.lookRange);

            Handles.DrawLine(controller.eyes.position, leftSide);
            Handles.DrawLine(controller.eyes.position, rightSide);
            Handles.DrawLine(controller.eyes.position,controller.eyes.position + Quaternion.AngleAxis(controller.lookAngle,controller.eyes.right)*viewDir);
            Handles.DrawLine(controller.eyes.position,controller.eyes.position + Quaternion.AngleAxis(-controller.lookAngle, controller.eyes.right) *viewDir);

        }
#endif
    }
}
