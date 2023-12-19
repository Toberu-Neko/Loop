using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Localization.SmartFormat.Utilities;

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

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Debug.Log("play " + name);
        if(s == null)
        {
            Debug.LogWarning("音效名稱" + name + "錯誤");
            return;
        }

        if (s.source.isPlaying)
        {
            Debug.LogError("音效名稱" + name + "已經在播放中");
            return;
        }

        foreach (var sound in sounds)
        {
            if (sound.source.isPlaying && sound.name != name)
            {
                Stop(sound.name);
            }
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.pitch += UnityEngine.Random.Range(s.pitchRandomRangeMin, s.pitchRandomRangeMax);
        s.source.Play();
    }

    public void Stop(string name, float time = 1f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("音效名稱" + name + "錯誤");
            return;
        }

        StartCoroutine(StopSound(s, time));
    }

    IEnumerator StopSound(Sound s, float time)
    {
        while(s.source.volume > 0)
        {
            s.source.volume -= Time.deltaTime / time;
            yield return null;
        }

        s.source.Stop();
    }

}
