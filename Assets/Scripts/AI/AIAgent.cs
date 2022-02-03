using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI
{
    public class AIAgent : MonoBehaviour
    {
        public Transform eyes;
        public bool aiActive = true;
        [Range(0, 180)] public float lookAngle;
        [Min(0)] public float lookRange;
        public State startState;
        public State remainState;
        public List<Transform> wayPointList;

        public State CurrentState { get; private set; }


        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public int nextWayPoint;
        [HideInInspector] public Transform chaseTarget;
        [HideInInspector] public float stateTimeElapsed;


        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            CurrentState = startState;
        }

        void Update()
        {
            if (!aiActive)
                return;
            CurrentState.UpdateState(this);
        }

        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                CurrentState = nextState;
                OnExitState();
            }
        }

        public bool CheckIfCountDownElapsed(float duration)
        {
            stateTimeElapsed += Time.deltaTime;
            return (stateTimeElapsed >= duration);
        }

        private void OnExitState()
        {
            stateTimeElapsed = 0;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (CurrentState != null && eyes != null)
            {
                Gizmos.color = CurrentState.sceneGizmoColor;
                Handles.color = CurrentState.sceneGizmoColor;
                Handles.Label(transform.position+Vector3.up,CurrentState.name);

                foreach (Transition currentStateTransition in CurrentState.transitions)
                {
                    currentStateTransition.decision.DrawGizmos(this);
                }

                foreach (Action currentStateAction in CurrentState.actions)
                {
                    currentStateAction.DrawGizmos(this);
                }

            }
        }
#endif


    }
}