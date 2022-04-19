using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Harmony.AI;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class BlackboardStateAttribute : PropertyAttribute
{
    public BlackboardStateAttribute() { }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BlackboardStateAttribute))]
internal sealed class BlackboardStateDrawer : PropertyDrawer
{
    private static string s_InvalidTypeMessage = L10n.Tr("Error");

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var field = property.serializedObject.targetObject.GetType().GetField("blackboard");
        if (field != null)
        {
            Blackboard blackboard = field.GetValue(property.serializedObject.targetObject) as Blackboard;

            if (property.propertyType == SerializedPropertyType.String && blackboard != null)
            {
                List<string> parameters = new List<string>(blackboard.states);

                int index = parameters.FindIndex(p => p == property.stringValue);
                if (index == -1)
                {
                    index = 0;
                }

                Rect labelRect = position;
                labelRect.width = EditorGUIUtility.labelWidth+2;

                position.width -= labelRect.width;
                position.x += labelRect.width;

                EditorGUI.LabelField(labelRect, label);
                EditorGUI.BeginChangeCheck();
                index = EditorGUI.Popup(position, index, parameters.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = parameters[index];
                }

                return;
            }
        }

        EditorGUI.LabelField(position, label.text, s_InvalidTypeMessage);
    }
}
#endif