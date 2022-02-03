using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Harmony.AI
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(AIAgent controller);

#if UNITY_EDITOR
        public virtual void DrawGizmos(AIAgent controller) {}
#endif
    }
}
