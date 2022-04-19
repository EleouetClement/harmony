using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI{
    public class CallEvent : ActionNode
    {
        public string eventName;

        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!context.treeRunner.customEvents.ContainsKey(eventName)) return State.Failure;

            context.treeRunner.customEvents[eventName].Invoke();
            return State.Success;
        }

        public override string GetName()
        {
            return $"Call {eventName} Event";
        }
    }
}

#if UNITY_EDITOR
namespace Harmony.AI
{
    [CustomEditor(typeof(CallEvent))]
    public class CallEventEditor : Editor
    {
        SerializedProperty description;
        SerializedProperty drawGizmos;
        SerializedProperty eventName;

        void OnEnable()
        {
            description = serializedObject.FindProperty("description");
            drawGizmos = serializedObject.FindProperty("drawGizmos");
            eventName = serializedObject.FindProperty("eventName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(description);
            EditorGUILayout.PropertyField(drawGizmos);

            if (Selection.activeGameObject != null)
            {
                BehaviourTreeRunner treeRunner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (treeRunner != null)
                {
                    List<string> eventList = new List<string>();
                    eventList.Add("none");
                    int selectedEvent = 0;

                    foreach (string customEventsKey in treeRunner.customEvents.Keys)
                    {
                        if (customEventsKey == eventName.stringValue)
                            selectedEvent = eventList.Count;
                        eventList.Add(customEventsKey);
                    }

                    EditorGUI.BeginChangeCheck();
                    selectedEvent = EditorGUILayout.Popup("Event", selectedEvent, eventList.ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selectedEvent == 0)
                            eventName.stringValue = "";
                        else
                            eventName.stringValue = eventList[selectedEvent];
                    }
                    
                    serializedObject.ApplyModifiedProperties();

                    return;
                }
            }

            EditorGUILayout.HelpBox("Select the Gameobject with the tree assigned to see available events", MessageType.Info);
            EditorGUILayout.PropertyField(eventName);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif