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
        public UDictionary<string, AIParameter> parameters;
        public List<Transform> wayPointList;

        public State CurrentState { get; private set; }


        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public int nextWayPoint;
        [HideInInspector] public Transform chaseTarget;
        [HideInInspector] public float stateTimeElapsed;

        private Dictionary<int, float> timers = new Dictionary<int, float>();


        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            CurrentState = startState;
        }

        void Update()
        {
            if (!aiActive)
                return;

            foreach (int key in timers.Keys) { timers[key] -= Time.deltaTime; }

            CurrentState.UpdateState(this);
        }

        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                if(CurrentState) CurrentState.ExitState(this);
                CurrentState = nextState;
                if(CurrentState) CurrentState.EnterState(this);
            }
        }

        public void OnDamage(DamageHit hit)
        {
            Debug.Log(hit);
        }

        public void RegisterTimer(int id, float duration)
        {
            if(!timers.ContainsKey(id))
                timers.Add(id,duration);
        }

        public bool CheckTimer(int id)
        {
            if (!timers.ContainsKey(id) || timers[id] > 0)
                return false;

            timers.Remove(id);
            return true;
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

            for (int i = 0; i < parameters.Count; i++)
            {
                height += 0.075f;
                switch (parameters[parameters.Keys[i]].ParameterType)
                {
                    case AIParameter.TypeSelection.None:
                        GUI.color = Color.white;
                        break;
                    case AIParameter.TypeSelection.Bool:
                        GUI.color = Color.red;
                        break;
                    case AIParameter.TypeSelection.Int:
                        GUI.color = Color.green;
                        break;
                    case AIParameter.TypeSelection.Float:
                        GUI.color = Color.blue;
                        break;
                    case AIParameter.TypeSelection.Vector:
                        GUI.color = Color.magenta;
                        break;
                    default:
                        GUI.color = Color.white;
                        break;
                }

                Handles.Label(transform.position + Vector3.up + new Vector3(0, height, 0), parameters.Keys[i] + " : " +parameters[parameters.Keys[i]]);
            }
        }
#endif

    }
}