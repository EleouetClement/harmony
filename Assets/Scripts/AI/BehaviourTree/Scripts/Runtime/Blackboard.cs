using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard
    {
        public UDictionary<string, bool> boolParameters = new UDictionary<string, bool>();
        public UDictionary<string, int> intParameters = new UDictionary<string, int>();
        public UDictionary<string, float> floatParameters = new UDictionary<string, float>();
        public UDictionary<string, Vector3> vectorParameters = new UDictionary<string, Vector3>();
    }
}