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
    public class AIAgent : BehaviourTreeRunner, IDamageable
    {
        [Header("Components")]
        public Animator animator;
        [Space]
        public bool aiActive = true;
        public float health;
        public float maxHealth = 30;
        public List<Transform> wayPointList;
        
        [HideInInspector] public int nextWayPoint;

        protected override void Awake()
        {
            base.Awake();
            health = maxHealth;
        }

        void Update()
        {
            if (!aiActive)
                return;
            base.Update();
        }

        public void OnDamage(DamageHit hit)
        {
            if (health > 0)
            {
                health -= hit.damage;
                Debug.Log(hit);

                if (health <= 0)
                {
                    aiActive = false;
                    if(animator) animator.SetBool("Dead", true);
                    Invoke(nameof(Death), 5);
                }
            }
        }

        public void Death()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (context != null)
            {
                if (context.agent.hasPath)
                {
                    Gizmos.color = context.agent.pathStatus == NavMeshPathStatus.PathComplete ? Color.green : Color.red;

                    for (int i = 1; i < context.agent.path.corners.Length; i++)
                    {
                        var path = context.agent.path;
                        Gizmos.DrawLine(path.corners[i-1], path.corners[i]);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            float height = 0.1f;

            /*for (int i = 0; i < parameters.Count; i++)
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
            }*/
        }
#endif

    }
}