using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PlaySound : ActionNode
{
    public AudioClip clip;
    public float volume = 1;
    public bool attached = true;
    public Vector3 offset;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (attached)
            SpawnedSound.SpawnSoundAttached(clip, context.transform, offset, volume);
        else
            SpawnedSound.SpawnSound(clip, offset, volume);

        return State.Success;
    }
}
