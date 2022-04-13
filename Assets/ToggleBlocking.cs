using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDesignTools
{   
    public class ToggleBlocking : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponentsInChildren<MeshRenderer>().ToList().ForEach(m => m.enabled = true);
        }
        private void OnDisable()
        {
            GetComponentsInChildren<MeshRenderer>().ToList().ForEach(m => m.enabled = false);
        }
    }
}

