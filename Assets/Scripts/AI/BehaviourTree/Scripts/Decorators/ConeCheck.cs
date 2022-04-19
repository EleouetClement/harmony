using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI
{
    public class ConeCheck : DecoratorNode
    {
        [BlackboardParam(Blackboard.ParameterType.Transform)]
        public string targetParameter = "target";

        [Layer] public int searchLayer;
        public LayerMask blockMask;
        public float eyesHeight = 1.75f;
        [Range(0, 180)] public float lookAngle = 45;
        [Min(0)] public float lookRange = 8;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            bool targetVisible = Look();
            if (targetVisible)
                return child.Update();
            return State.Failure;
        }

        private bool Look()
        {
            RaycastHit hit;

            Vector3 eyes = context.transform.position + Vector3.up * eyesHeight;
            Vector3 eyesDir = context.transform.forward;

            float radius = lookRange * Mathf.Tan(Mathf.Deg2Rad * lookAngle);

            if (Physics.BoxCast(eyes, new Vector3(radius, radius, 0.1f), eyesDir, out hit,
                Quaternion.LookRotation(eyesDir), lookRange, 1 << searchLayer))
            {
                Vector3 targetDirection = (hit.collider.transform.position - eyes).normalized;

                if (Mathf.Acos(Vector3.Dot(eyesDir, targetDirection)) * Mathf.Rad2Deg <= lookAngle)
                {
                    Debug.DrawLine(eyes, hit.collider.transform.position, Color.yellow);
                    if (Physics.Raycast(eyes, targetDirection, out hit,
                            lookRange, blockMask.value | (1 << searchLayer)) &&
                        hit.collider.gameObject.layer == searchLayer)
                    {
                        Debug.DrawLine(eyes, hit.collider.transform.position, Color.green);
                        blackboard.SetParameter(targetParameter, hit.transform);
                        return true;
                    }
                }
            }

            return false;
        }

        public override string GetName()
        {
            return $"See {LayerMask.LayerToName(searchLayer)}?";
        }

#if UNITY_EDITOR
        public override void OnDrawGizmos()
        {
            if (context != null)
            {
                Vector3 eyes = context.transform.position + Vector3.up * eyesHeight;
                Vector3 eyesDir = context.transform.forward;
                Vector3 eyesUp = context.transform.up;
                Vector3 eyesRight = context.transform.right;

                Vector3 viewDir = eyesDir * lookRange;

                Vector3 leftSide = eyes +
                                   Quaternion.AngleAxis(lookAngle, Vector3.up) * viewDir;
                Vector3 rightSide = eyes +
                                    Quaternion.AngleAxis(-lookAngle, Vector3.up) * viewDir;

                Vector3 circleCenter = leftSide + (rightSide - leftSide) / 2.0f;

                Handles.DrawWireDisc(circleCenter, eyesDir, (rightSide - leftSide).magnitude / 2);

                Handles.DrawWireArc(eyes, eyesUp, eyesDir, lookAngle, lookRange);
                Handles.DrawWireArc(eyes, eyesUp, eyesDir, -lookAngle, lookRange);
                Handles.DrawWireArc(eyes, eyesRight, eyesDir, lookAngle, lookRange);
                Handles.DrawWireArc(eyes, eyesRight, eyesDir, -lookAngle, lookRange);

                Handles.DrawLine(eyes, leftSide);
                Handles.DrawLine(eyes, rightSide);
                Handles.DrawLine(eyes, eyes + Quaternion.AngleAxis(lookAngle, eyesRight) * viewDir);
                Handles.DrawLine(eyes, eyes + Quaternion.AngleAxis(-lookAngle, eyesRight) * viewDir);
            }
        }
#endif
    }
}