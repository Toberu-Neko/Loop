using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {

    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // Debug.Log("play");
        if(s == null)
        {
            Debug.LogWarning("���ĦW��" + name + "���~");
            return;
        }
        s.source.pitch = s.pitch;
        s.source.pitch += UnityEngine.Random.Range(s.pitchRandomRangeMin, s.pitchRandomRangeMax);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("���ĦW��" + name + "���~");
            return;
        }
        s.source.Stop();
    }
}
