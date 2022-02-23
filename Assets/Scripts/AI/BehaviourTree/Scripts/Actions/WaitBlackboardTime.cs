using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WaitBlackboardTime : ActionNode
{
    public string durationParameter;
    public float duration;
    float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
        blackboard.GetParameter(durationParameter, out duration);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
