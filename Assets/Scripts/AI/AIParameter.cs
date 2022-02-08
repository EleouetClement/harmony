using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AIParameter : ISerializationCallbackReceiver
{
    public enum TypeSelection
    {
        None,
        Bool,
        Int,
        Float,
        Vector
    }

    [SerializeField, HideInInspector]
    private string serializedValue = "n";

    public TypeSelection ParameterType
    {
        get
        {
            if (paramValue == null) return TypeSelection.None;
            if (paramValue is bool) return TypeSelection.Bool;
            if (paramValue is int) return TypeSelection.Int;
            if (paramValue is float) return TypeSelection.Float;
            if (paramValue is Vector3) return TypeSelection.Vector;
            return TypeSelection.None;
        }
    }

    private object paramValue;

    public T ReadValue<T>()
    {
        if(typeof(T) == paramValue.GetType())
            return (T)paramValue;
        return default;
    }

    public void SetValue<T>(T newValue)
    {
        paramValue = newValue;
    }

    public static string SerializeValue(object paramValue)
    {
        if (paramValue == null)
            return "n";
        
        var type = paramValue.GetType();

        if (type == typeof(bool))
            return "b" + paramValue;

        if (type == typeof(int))
            return "i" + paramValue;
        
        if (type == typeof(float))
            return "f" + paramValue;
        
        if (type == typeof(Vector3))
        {
            Vector3 v = (Vector3)paramValue;
            return "v" + v.x + "|" + v.y + "|" + v.z;
        }

        return null;
    }

    public void OnBeforeSerialize()
    {
        serializedValue = SerializeValue(paramValue);
    }

    public static Type DeserializeValue(string serializedValue, out object outValue)
    {
        outValue = null;
        if (string.IsNullOrEmpty(serializedValue))
            return null;
        char type = serializedValue[0];

        switch (type)
        {
            case 'n':
                return null;
            case 'b':
                outValue = serializedValue.Substring(1) == "True";
                return typeof(bool);
            case 'i':
                outValue = int.Parse(serializedValue.Substring(1));
                return typeof(int);
            case 'f':
                outValue = float.Parse(serializedValue.Substring(1));
                return typeof(float);
            case 'v':
                string[] v = serializedValue.Substring(1).Split('|');
                outValue = new Vector3(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]));
                return typeof(Vector3);
            default:
                return null;
        }
    }

    public void OnAfterDeserialize()
    {
        DeserializeValue(serializedValue, out paramValue);
    }

    public override string ToString()
    {
        if (paramValue == null) return "null";
        return paramValue.ToString();
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AIParameter))]
public class AIParameterEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var serializedValue = property.FindPropertyRelative("serializedValue");
        object paramValue = null;

        Type valueType = AIParameter.DeserializeValue(serializedValue.stringValue, out paramValue);
        AIParameter.TypeSelection selectedType = AIParameter.TypeSelection.None;

        if (valueType == typeof(bool)) selectedType = AIParameter.TypeSelection.Bool;
        else if (valueType == typeof(int)) selectedType = AIParameter.TypeSelection.Int;
        else if (valueType == typeof(float)) selectedType = AIParameter.TypeSelection.Float;
        else if (valueType == typeof(Vector3)) selectedType = AIParameter.TypeSelection.Vector;

        EditorGUIUtility.wideMode = true;
        EditorGUIUtility.labelWidth = 40;
        EditorGUI.indentLevel = 0;
        position.width /= 2;
        
        selectedType = (AIParameter.TypeSelection)EditorGUI.EnumPopup(position,"Type" ,selectedType);
        position.x += position.width;

        switch (selectedType)
        {
            case AIParameter.TypeSelection.None:
                paramValue = null;
                break;
            case AIParameter.TypeSelection.Bool:
                if (paramValue is not bool) paramValue = default(bool);
                paramValue = EditorGUI.Toggle(position, "Value", (bool) paramValue);
                break;
            case AIParameter.TypeSelection.Int:
                if (paramValue is not int) paramValue = default(int);
                paramValue = EditorGUI.IntField(position, "Value", (int)paramValue);
                break;
            case AIParameter.TypeSelection.Float:
                if (paramValue is not float) paramValue = default(float);
                paramValue = EditorGUI.FloatField(position, "Value", (float)paramValue);
                break;
            case AIParameter.TypeSelection.Vector:
                if (paramValue is not Vector3) paramValue = default(Vector3);
                paramValue = EditorGUI.Vector3Field(position, "Value", (Vector3)paramValue);
                break;
        }

        serializedValue.stringValue = AIParameter.SerializeValue(paramValue);
    }
}
#endif