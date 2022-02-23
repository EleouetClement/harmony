using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepeatOnParameter : DecoratorNode
{
    public Blackboard.ParameterType parameterType;
    public string parameterName;
    public Blackboard.ParameterType otherParameterType;
    public string otherParameterName;
    public float floatConstant;
    public Vector3 vectorConstant;
    public bool restartOnSuccess = true;
    public bool restartOnFailure = false;

    private bool useOtherParam;

    protected override void OnStart()
    {
        useOtherParam = !string.IsNullOrEmpty(otherParameterName);
    }

    protected override void OnStop()
    {

    }

    private bool Compare()
    {
        float paramValueFloat = 0;
        Vector3 paramValueVector = Vector3.zero;

        float otherValueFloat = 0;
        Vector3 otherValueVector = Vector3.zero;

        switch (parameterType)
        {
            case Blackboard.ParameterType.Bool:
                if (blackboard.GetParameter(parameterName, out bool boolValue))
                    paramValueFloat = boolValue ? 1 : 0;
                else return false;
                break;
            case Blackboard.ParameterType.Int:
                if (blackboard.GetParameter(parameterName, out int intValue))
                    paramValueFloat = intValue;
                else return false;
                break;
            case Blackboard.ParameterType.Float:
                if (blackboard.GetParameter(parameterName, out float floatValue))
                    paramValueFloat = floatValue;
                else return false;
                break;
            case Blackboard.ParameterType.Vector:
                if (blackboard.GetParameter(parameterName, out Vector3 vectorValue))
                    paramValueVector = vectorValue;
                else return false;
                break;
            case Blackboard.ParameterType.Transform: return false;
        }

        if (useOtherParam)
        {
            switch (parameterType)
            {
                case Blackboard.ParameterType.Bool:
                    if (blackboard.GetParameter(otherParameterName, out bool boolValue))
                        otherValueFloat = boolValue ? 1 : 0;
                    else return false;
                    break;
                case Blackboard.ParameterType.Int:
                    if (blackboard.GetParameter(otherParameterName, out int intValue))
                        otherValueFloat = intValue;
                    else return false;
                    break;
                case Blackboard.ParameterType.Float:
                    if (blackboard.GetParameter(otherParameterName, out float floatValue))
                        otherValueFloat = floatValue;
                    else return false;
                    break;
                case Blackboard.ParameterType.Vector:
                    if (blackboard.GetParameter(otherParameterName, out Vector3 vectorValue))
                        otherValueVector = vectorValue;
                    else return false;
                    break;
                case Blackboard.ParameterType.Transform: return false;
            }
        }
        else
        {
            otherValueFloat = floatConstant;
            otherValueVector = vectorConstant;
        }

        if (parameterType == Blackboard.ParameterType.Vector ||
            (useOtherParam && otherParameterType == Blackboard.ParameterType.Vector))
        {
            if (paramValueVector != otherValueVector) return false;
        }
        else
        {
            if (Mathf.Abs(paramValueFloat - otherValueFloat) > 0.01f) return false;
        }

        return true;
    }

    protected override State OnUpdate()
    {
        bool result = Compare();

        if (result && restartOnSuccess || !result && restartOnFailure)
        {
            child.Update();
            return State.Running;
        }

        return State.Failure;
    }
}
