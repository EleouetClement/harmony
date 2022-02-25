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
public class BlackboardParamAttribute : PropertyAttribute
{
    public readonly Blackboard.ParameterType type;

    public BlackboardParamAttribute(Blackboard.ParameterType type)
    {
        this.type = type;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BlackboardParamAttribute))]
internal sealed class BlackboardParamDrawer : PropertyDrawer
{
    private static string s_InvalidTypeMessage = L10n.Tr("Error");

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BlackboardParamAttribute param = (BlackboardParamAttribute)attribute;

        var field = property.serializedObject.targetObject.GetType().GetField("blackboard");
        if (field != null)
        {
            Blackboard blackboard = field.GetValue(property.serializedObject.targetObject) as Blackboard;

            if (property.propertyType == SerializedPropertyType.String && blackboard != null)
            {
                List<string> parameters = null;

                switch (param.type)
                {
                    case Blackboard.ParameterType.Bool:
                        parameters = new List<string>(blackboard.boolParameters.Keys);
                        break;
                    case Blackboard.ParameterType.Int:
                        parameters = new List<string>(blackboard.intParameters.Keys);
                        break;
                    case Blackboard.ParameterType.Float:
                        parameters = new List<string>(blackboard.floatParameters.Keys);
                        break;
                    case Blackboard.ParameterType.Vector:
                        parameters = new List<string>(blackboard.vectorParameters.Keys);
                        break;
                    case Blackboard.ParameterType.Transform:
                        parameters = new List<string>(blackboard.transformParameters.Keys);
                        break;
                    default:
                        EditorGUI.LabelField(position, label.text, s_InvalidTypeMessage);
                        return;
                }

                parameters.Insert(0,"none");

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
                    if (index == 0)
                    {
                        property.stringValue = "";
                    }
                    else
                    {
                        property.stringValue = parameters[index];
                    }
                }

                return;
            }
        }

        EditorGUI.LabelField(position, label.text, s_InvalidTypeMessage);
    }
}
#endif