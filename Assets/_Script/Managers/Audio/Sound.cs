using UnityEngine.Audio;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0f,3f)]
    public float pitch = 1f;
    [Tooltip("Can be negetive")]
    public float pitchRandomRangeMin;
    public float pitchRandomRangeMax;
    public bool loop;

    [HideInInspector] public AudioSource source;
}
