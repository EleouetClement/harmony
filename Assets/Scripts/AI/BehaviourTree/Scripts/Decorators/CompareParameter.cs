using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony.AI;

namespace Harmony.AI
{
    public class CompareParameter : DecoratorNode
    {
        public Blackboard.ParameterType parameterType;
        public string parameterName;
        public Blackboard.ParameterType otherParameterType;
        public string otherParameterName;
        public float floatConstant;
        public Vector3 vectorConstant;

        private bool useOtherParam;

        protected override void OnStart()
        {
            useOtherParam = !string.IsNullOrEmpty(otherParameterName);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
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
                    else return State.Failure;
                    break;
                case Blackboard.ParameterType.Int:
                    if (blackboard.GetParameter(parameterName, out int intValue))
                        paramValueFloat = intValue;
                    else return State.Failure;
                    break;
                case Blackboard.ParameterType.Float:
                    if (blackboard.GetParameter(parameterName, out float floatValue))
                        paramValueFloat = floatValue;
                    else return State.Failure;
                    break;
                case Blackboard.ParameterType.Vector:
                    if (blackboard.GetParameter(parameterName, out Vector3 vectorValue))
                        paramValueVector = vectorValue;
                    else return State.Failure;
                    break;
                case Blackboard.ParameterType.Transform: return State.Failure;
            }

            if (useOtherParam)
            {
                switch (parameterType)
                {
                    case Blackboard.ParameterType.Bool:
                        if (blackboard.GetParameter(otherParameterName, out bool boolValue))
                            otherValueFloat = boolValue ? 1 : 0;
                        else return State.Failure;
                        break;
                    case Blackboard.ParameterType.Int:
                        if (blackboard.GetParameter(otherParameterName, out int intValue))
                            otherValueFloat = intValue;
                        else return State.Failure;
                        break;
                    case Blackboard.ParameterType.Float:
                        if (blackboard.GetParameter(otherParameterName, out float floatValue))
                            otherValueFloat = floatValue;
                        else return State.Failure;
                        break;
                    case Blackboard.ParameterType.Vector:
                        if (blackboard.GetParameter(otherParameterName, out Vector3 vectorValue))
                            otherValueVector = vectorValue;
                        else return State.Failure;
                        break;
                    case Blackboard.ParameterType.Transform: return State.Failure;
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
                if (paramValueVector != otherValueVector) return State.Failure;
            }
            else
            {
                if (Mathf.Abs(paramValueFloat - otherValueFloat) > 0.01f) return State.Failure;
            }

            return child.Update();
        }
    }
}