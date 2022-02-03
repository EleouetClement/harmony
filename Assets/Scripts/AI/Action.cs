using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(AIAgent controller);

#if UNITY_EDITOR
        public virtual void DrawGizmos(AIAgent controller) {}
#endif
    }
}

