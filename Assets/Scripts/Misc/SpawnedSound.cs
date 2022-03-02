using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSound : MonoBehaviour
{
    private AudioSource audioSource;

    public static SpawnedSound SpawnSound(AudioClip clip, Vector3 position, float volume = 1)
    {
        GameObject soundObject = new GameObject("spawned " + clip.name);
        soundObject.transform.position = position;
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();
        soundSource.clip = clip;
        soundSource.volume = volume;
        soundSource.loop = false;
        soundSource.spatialBlend = 1;
        soundSource.Play();
        SpawnedSound spawnedSound = soundObject.AddComponent<SpawnedSound>();
        return spawnedSound;
    }

    public static SpawnedSound SpawnSoundAttached(AudioClip clip, Transform parent, Vector3 position, float volume = 1)
    {
        SpawnedSound spawnedSound = SpawnSound(clip, position, volume);
        spawnedSound.transform.SetParent(parent);
        spawnedSound.transform.localPosition = position;
        return spawnedSound;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!audioSource.isPlaying) Destroy(gameObject);
    }
}
