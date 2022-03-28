using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Harmony.AI
{
    public class AIAgent : MonoBehaviour, IDamageable
    {
        [Header("Components")] 
        public BehaviourTreeRunner treeRunner;
        public Animator animator;
        [Space]
        public bool aiActive = true;
        public float health;
        public float maxHealth = 30;

        public bool patrolling = false;
        public float patrolSpeed = 1.25f;
        public float minWaitProbability = 5;
        public float maxWaitProbability = 15;
        public float minWaitDuration = 0;
        public float maxWaitDuration = 5;
        public float maxPointDistance = 2;
        public Vector3[] wayPointList;
        public int nextWayPoint;

        private float patrolUntilWait = 0;
        private float patrolWaitTimer = 0;

        void Awake()
        {
            if (!treeRunner) treeRunner = GetComponent<BehaviourTreeRunner>();
            health = maxHealth;
            if (patrolling)
            {
                treeRunner.context.agent.SetDestination(wayPointList[nextWayPoint]);
                patrolUntilWait = Random.Range(minWaitProbability, maxWaitProbability);
            }
        }

        protected  virtual void Update()
        {
            if (!aiActive)
                return;

            if (patrolling)
            {
                
                if (Vector3.SqrMagnitude(wayPointList[nextWayPoint] - transform.position) <
                    maxPointDistance * maxPointDistance)
                {
                    nextWayPoint = (nextWayPoint + 1) % wayPointList.Length;
                    treeRunner.context.agent.SetDestination(wayPointList[nextWayPoint]);
                }

                if (patrolWaitTimer <= 0)
                {
                    if (patrolUntilWait > 0)
                        patrolUntilWait -= Time.deltaTime;
                    else
                    {
                        treeRunner.context.agent.SetDestination(transform.position);
                        patrolUntilWait = Random.Range(minWaitProbability, maxWaitProbability);
                        patrolWaitTimer = Random.Range(minWaitDuration, maxWaitDuration);
                    }
                }
                else
                {
                    patrolWaitTimer -= Time.deltaTime;
                    if (patrolWaitTimer <= 0)
                    {
                        treeRunner.context.agent.SetDestination(wayPointList[nextWayPoint]);
                    }
                }
            }

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

        public void StartPatrolling()
        {
            if (!patrolling)
            {
                treeRunner.context.agent.SetDestination(wayPointList[nextWayPoint]);
                patrolUntilWait = Random.Range(minWaitProbability, maxWaitProbability);
                patrolWaitTimer = 0;
                patrolling = true;
            }
            
        }

        public void StopPatrolling()
        {
            patrolling = false;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (treeRunner && treeRunner.context != null)
            {
                if (treeRunner.context.agent.hasPath)
                {
                    Gizmos.color = treeRunner.context.agent.pathStatus == NavMeshPathStatus.PathComplete ? Color.green : Color.red;

                    for (int i = 1; i < treeRunner.context.agent.path.corners.Length; i++)
                    {
                        var path = treeRunner.context.agent.path;
                        Gizmos.DrawLine(path.corners[i-1], path.corners[i]);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            float height = 0.1f;

            /*if (Application.isPlaying && wayPointList != null && wayPointList.Length > 0)
            {
                for (int i = 0; i < wayPointList.Length; i++)
                {
                    Handles.color = Color.white;
                    if (wayPointList.Length > 1 && i != wayPointList.Length - 1)
                    {
                        Handles.DrawLine(wayPointList[i], wayPointList[i + 1], 2);
                    }
                    else if (i == wayPointList.Length - 1)
                    {
                        Handles.DrawLine(wayPointList[i], wayPointList[0], 2);
                    }

                    bool isCurrent = nextWayPoint == i;

                    Handles.color = isCurrent ? Color.red : Color.blue;
                    Handles.SphereHandleCap(0, wayPointList[i], Quaternion.identity, 0.1f, EventType.Repaint);

                    Handles.Label(wayPointList[i], i.ToString());
                }
            }*/

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

#if UNITY_EDITOR
    [CustomEditor(typeof(AIAgent),true)]
    public class AIAgentDrawer : Editor
    {
        private int currentHandle = 0;
        private SerializedProperty pointList;

        private void OnEnable()
        {
            pointList = serializedObject.FindProperty("wayPointList");
        }

        void OnSceneGUI()
        {
            serializedObject.Update();

            AIAgent agent = (AIAgent)target;
            Handles.CapFunction handleCap = Handles.SphereHandleCap;

            if (pointList != null && pointList.arraySize > 0)
            {
                for (int i = 0; i < pointList.arraySize; i++)
                {
                    Handles.color = Color.white;
                    if (pointList.arraySize > 1 && i != pointList.arraySize - 1)
                    {
                        Handles.DrawLine(pointList.GetArrayElementAtIndex(i).vector3Value, pointList.GetArrayElementAtIndex(i+1).vector3Value, 2);
                    }else if (i == pointList.arraySize - 1)
                    {
                        Handles.DrawLine(pointList.GetArrayElementAtIndex(i).vector3Value, pointList.GetArrayElementAtIndex(0).vector3Value, 2);
                        if (pointList.arraySize > 1 && pointList.GetArrayElementAtIndex(i).vector3Value == pointList.GetArrayElementAtIndex(i-1).vector3Value)
                        {
                            pointList.GetArrayElementAtIndex(i).vector3Value = agent.transform.position-Vector3.up;
                        }
                    }

                    bool isCurrent = currentHandle == i;

                    if (isCurrent)
                    {
                        if (!Application.isPlaying && Event.current.type == EventType.MouseUp)
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                        pointList.GetArrayElementAtIndex(i).vector3Value = Handles.PositionHandle(pointList.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);

                        Handles.color = Color.red;
                        Handles.SphereHandleCap(0, pointList.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity, 0.1f, EventType.Repaint);
                    }
                    else
                    {

                        Handles.color = Color.blue;
                        if (Handles.Button(pointList.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity, 0.1f, 0.1f, handleCap))
                        {
                            currentHandle = i;
                        }
                    }

                    Handles.Label(pointList.GetArrayElementAtIndex(i).vector3Value, i.ToString());
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}