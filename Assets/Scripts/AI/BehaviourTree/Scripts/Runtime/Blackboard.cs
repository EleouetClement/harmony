using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard
    {
        public enum ParameterType
        {
            Bool,
            Int,
            Float,
            Vector,
            Transform
        }

        public List<string> states = new List<string>();
        public string currentState = "";
        public UDictionary<string, bool> boolParameters = new UDictionary<string, bool>();
        public UDictionary<string, int> intParameters = new UDictionary<string, int>();
        public UDictionary<string, float> floatParameters = new UDictionary<string, float>();
        public UDictionary<string, Vector3> vectorParameters = new UDictionary<string, Vector3>();
        public UDictionary<string, Transform> transformParameters = new UDictionary<string, Transform>();

        //bool param
        public void SetParameter(string paramName, bool value)
        {
            if (boolParameters.ContainsKey(paramName))
                boolParameters[paramName] = value;
            else
                Debug.LogError($"Parameter \"{paramName}\" doesn't exists");
        }

        public bool GetParameter(string paramName, out bool value)
        {
            if (boolParameters.TryGetValue(paramName, out value))
                return true;
            Debug.LogWarning($"Parameter \"{paramName}\" doesn't exists");
            return false;
        }

        //int param
        public void SetParameter(string paramName, int value)
        {
            if (intParameters.ContainsKey(paramName))
                intParameters[paramName] = value;
            else
                Debug.LogError($"Parameter \"{paramName}\" doesn't exists");
        }

        public bool GetParameter(string paramName, out int value)
        {
            if (intParameters.TryGetValue(paramName, out value))
                return true;
            Debug.LogWarning($"Parameter \"{paramName}\" doesn't exists");
            return false;
        }

        //float param
        public void SetParameter(string paramName, float value)
        {
            if (floatParameters.ContainsKey(paramName))
                floatParameters[paramName] = value;
            else
                Debug.LogError($"Parameter \"{paramName}\" doesn't exists");
        }

        public bool GetParameter(string paramName, out float value)
        {
            if (floatParameters.TryGetValue(paramName, out value))
                return true;
            Debug.LogWarning($"Parameter \"{paramName}\" doesn't exists");
            return false;
        }

        //vector param
        public void SetParameter(string paramName, Vector3 value)
        {
            if (vectorParameters.ContainsKey(paramName))
                vectorParameters[paramName] = value;
            else
                Debug.LogError($"Parameter \"{paramName}\" doesn't exists");
        }

        public bool GetParameter(string paramName, out Vector3 value)
        {
            if (vectorParameters.TryGetValue(paramName, out value))
                return true;
            Debug.LogWarning($"Parameter \"{paramName}\" doesn't exists");
            return false;
        }

        //Transform param
        public void SetParameter(string paramName, Transform value)
        {
            if (transformParameters.ContainsKey(paramName))
                transformParameters[paramName] = value;
            else
                Debug.LogError($"Parameter \"{paramName}\" doesn't exists");
        }

        public bool GetParameter(string paramName, out Transform value)
        {
            if (transformParameters.TryGetValue(paramName, out value))
                return true;
            Debug.LogWarning($"Parameter \"{paramName}\" doesn't exists");
            return false;
        }
    }
}