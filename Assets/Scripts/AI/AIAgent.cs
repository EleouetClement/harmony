using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI
{
    public class AIAgent : MonoBehaviour, IDamageable
    {
        public Transform eyes;
        public bool aiActive = true;
        [Range(0, 180)] public float lookAngle;
        [Min(0)] public float lookRange;
        public State startState;
        public State remainState;
        public UDictionary<string, bool> boolParameters;
        public UDictionary<string, float> floatParameters;
        public UDictionary<string, int> intParameters;
        public UDictionary<string, Vector3> vectorParameters;
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
                if(CurrentState) CurrentState.ExitState(this);
                CurrentState = nextState;
                if(CurrentState) CurrentState.EnterState(this);
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

        public void OnDamage(DamageHit hit)
        {
            Debug.Log(hit);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (CurrentState != null && eyes != null)
            {
                Gizmos.color = CurrentState.sceneGizmoColor;
                Handles.color = CurrentState.sceneGizmoColor;
                Handles.Label(transform.position + Vector3.up, CurrentState.name);

                foreach (Transition currentStateTransition in CurrentState.transitions)
                {
                    currentStateTransition.decision.DrawGizmos(this);
                }

                foreach (Action currentStateAction in CurrentState.updateActions)
                {
                    currentStateAction.DrawGizmos(this);
                }

                if (navMeshAgent.hasPath)
                {
                    Gizmos.color = navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete ? Color.green : Color.red;

                    for (int i = 1; i < navMeshAgent.path.corners.Length; i++)
                    {
                        Gizmos.DrawLine(navMeshAgent.path.corners[i-1], navMeshAgent.path.corners[i]);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {

            float height = 0.1f;

            GUI.color = Color.red;
            for (int i = 0; i < boolParameters.Count; i++)
            {
                height += 0.075f;
                Handles.Label(transform.position + Vector3.up + new Vector3(0, height,0), boolParameters.Keys[i] + " : " + boolParameters.Values[i]);
            }

            GUI.color = Color.green;
            for (int i = 0; i < floatParameters.Count; i++)
            {
                height += 0.075f;
                Handles.Label(transform.position + Vector3.up + new Vector3(0, height, 0), floatParameters.Keys[i] + " : " + floatParameters.Values[i]);
            }

            GUI.color = Color.blue;
            for (int i = 0; i < intParameters.Count; i++)
            {
                height += 0.075f;
                Handles.Label(transform.position + Vector3.up + new Vector3(0, height, 0), intParameters.Keys[i] + " : " + intParameters.Values[i]);
            }

            GUI.color = Color.magenta;
            for (int i = 0; i < vectorParameters.Count; i++)
            {
                height += 0.075f;
                Handles.Label(transform.position + Vector3.up + new Vector3(0, height, 0), vectorParameters.Keys[i] + " : " + vectorParameters.Values[i]);
            }

            GUI.color = Color.white;
        }
#endif

    }
}