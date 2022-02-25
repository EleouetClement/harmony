using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace Harmony.AI
{
    public class CallMethod : ActionNode
    {
        public string component;
        public string method;
        public float delay = 0;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (!context.components.ContainsKey(component)) return State.Failure;

            context.components[component].Invoke(method, delay);
            return State.Success;
        }
    }
}

#if UNITY_EDITOR
namespace Harmony.AI
{
    [CustomEditor(typeof(CallMethod))]
    public class CallMethodEditor : Editor
    {
        SerializedProperty component;
        SerializedProperty method;
        SerializedProperty delay;

        void OnEnable()
        {
            component = serializedObject.FindProperty("component");
            method = serializedObject.FindProperty("method");
            delay = serializedObject.FindProperty("delay");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (Selection.activeGameObject != null)
            {
                BehaviourTreeRunner treeRunner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (treeRunner != null)
                {
                    List<string> methodList = new List<string>();
                    methodList.Add("none");
                    Dictionary<string, MonoBehaviour> componentsList = new Dictionary<string, MonoBehaviour>();
                    int selectedMethod = 0;

                    MonoBehaviour[] components = Selection.activeGameObject.GetComponents<MonoBehaviour>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        if (!componentsList.ContainsKey(components[i].name))
                        {
                            componentsList.Add(components[i].name, components[i]);
                        }
                        else
                        {
                            int count = 2;
                            while (componentsList.ContainsKey(components[i].name + count))
                            {
                                count++;
                            }

                            componentsList.Add(components[i].name + count, components[i]);
                        }
                    }

                    foreach (var componentPair in componentsList)
                    {
                        MethodInfo[] methods = componentPair.Value.GetType().GetMethods();
                        foreach (var compMethod in methods)
                        {
                            if (compMethod.GetParameters().Length == 0)
                            {
                                string methodName = componentPair.Key + " - " + compMethod.Name;
                                methodList.Add(methodName);
                                if (methodName == (component.stringValue + " - " + method.stringValue))
                                    selectedMethod = methodList.Count - 1;
                            }
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    selectedMethod = EditorGUILayout.Popup("Method", selectedMethod, methodList.ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        string selectedMethodStr = methodList[selectedMethod];
                        string[] methodInfos = selectedMethodStr.Split(" - ");
                        if (methodInfos.Length == 2)
                        {
                            component.stringValue = methodInfos[0];
                            method.stringValue = methodInfos[1];
                        }
                    }

                    EditorGUILayout.PropertyField(delay);
                    serializedObject.ApplyModifiedProperties();

                    return;
                }
            }

            EditorGUILayout.PropertyField(component);
            EditorGUILayout.PropertyField(method);
            EditorGUILayout.PropertyField(delay);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif