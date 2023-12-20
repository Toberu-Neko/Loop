using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    [SerializeField] private GameObject soundFXObj;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup soundFXMixerGroup;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;

    #region Set Volume

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }
    #endregion

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
            s.source.outputAudioMixerGroup = bgmMixerGroup;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundFXObj, spawnTransform.position, Quaternion.identity).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = false;

        audioSource.Play();

        float time = audioSource.clip.length;

        StartCoroutine(ReturnSFXObj(audioSource.gameObject, time));
    }

    public void PlayRandomSoundFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);
        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundFXObj, spawnTransform.position, Quaternion.identity).GetComponent<AudioSource>();

        audioSource.clip = audioClip[randomIndex];
        audioSource.volume = volume;
        audioSource.loop = false;

        audioSource.Play();

        float time = audioSource.clip.length;

        StartCoroutine(ReturnSFXObj(audioSource.gameObject, time));
    }

    private IEnumerator ReturnSFXObj(GameObject sfxObj, float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPoolManager.ReturnObjectToPool(sfxObj);
    }

    public void PlayBGM(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogError("音效名稱" + name + "錯誤");
            return;
        }

        if (s.source.isPlaying)
        {
            Debug.LogWarning("音效名稱" + name + "已經在播放中");
            return;
        }

        foreach (var sound in sounds)
        {
            if (sound.source.isPlaying && sound.name != name)
            {
                StopBGM(sound.name);
            }
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.pitch += UnityEngine.Random.Range(s.pitchRandomRangeMin, s.pitchRandomRangeMax);
        s.source.Play();
    }

    public void StopBGM(string name, float time = 1f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("音效名稱" + name + "錯誤");
            return;
        }

        StartCoroutine(IE_StopBGM(s, time));
    }

    IEnumerator IE_StopBGM(Sound s, float time)
    {
        while(s.source.volume > 0)
        {
            s.source.volume -= Time.deltaTime / time;
            yield return null;
        }

        s.source.Stop();
    }

}
