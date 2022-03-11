using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Harmony.AI {
    public class Wait : ActionNode {
        public float durationMin = 1;
        public float durationMax = 1;
        private float startTime;
        private float duration;

        protected override void OnStart() {
            startTime = Time.time;
            duration = Random.Range(durationMin, durationMax);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (Time.time - startTime > duration) {
                return State.Success;
            }
            return State.Running;
        }

        public override string GetName()
        {
            if (Math.Abs(durationMin - durationMax) < Mathf.Epsilon)
            {
                return "Wait " + durationMin + "s";
            }

            return $"Wait Between {durationMin}s and {durationMax}s";
        }
    }
}
