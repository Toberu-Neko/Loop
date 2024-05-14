using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }

    [SerializeField] private GameObject soundFXObj2D;
    [SerializeField] private GameObject soundFXObj3D;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup soundFXMixerGroup;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;

    [SerializeField] private Sound buttonHover;
    [SerializeField] private Sound buttonClick;

    public enum SoundType 
    { 
        twoD,
        threeD
    }

    public void PlayButtonHover(Transform spawnTransform)
    {
        PlaySoundFX(buttonHover, spawnTransform, SoundType.twoD);
    }

    public void PlayButtonClick(Transform spawnTransform)
    {
        PlaySoundFX(buttonClick, spawnTransform, SoundType.twoD);
    }

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
        if(Instance == null)
        {
            Instance = this;
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

    public void PlaySoundFX(Sound sound, Transform spawnTransform, SoundType type)
    {
        if(sound == null)
        {
            Debug.LogError("Sound == null in audio manager.");
            return;
        }


        AudioSource audioSource = ObjectPoolManager.SpawnObject(type == SoundType.twoD ? soundFXObj2D : soundFXObj3D, spawnTransform.position, Quaternion.identity).GetComponent<AudioSource>();

        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.loop = sound.loop;
        audioSource.pitch = sound.pitch;
        audioSource.pitch += UnityEngine.Random.Range(sound.pitchRandomRangeMin, sound.pitchRandomRangeMax);

        audioSource.Play();

        float time = audioSource.clip.length;

        StartCoroutine(ReturnSFXObj(audioSource.gameObject, time));
    }

    public void PlayRandomSoundFX(Sound[] sounds, Transform spawnTransform)
    {
        int randomIndex = UnityEngine.Random.Range(0, sounds.Length);
        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundFXObj3D, spawnTransform.position, Quaternion.identity).GetComponent<AudioSource>();

        Sound s = sounds[randomIndex];
        audioSource.clip = s.clip;

        audioSource.clip = s.clip;
        audioSource.volume = s.volume;
        audioSource.loop = s.loop;
        audioSource.pitch = s.pitch;
        audioSource.pitch += UnityEngine.Random.Range(s.pitchRandomRangeMin, s.pitchRandomRangeMax);

        audioSource.Play();

        float time = audioSource.clip.length;

        StartCoroutine(ReturnSFXObj(audioSource.gameObject, time));
    }

    private IEnumerator ReturnSFXObj(GameObject sfxObj, float time)
    {
        yield return new WaitForSecondsRealtime(time);
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

    public void StopAllBGM()
    {
        foreach (var sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                StopBGM(sound.name);
            }
        }
    }

    IEnumerator IE_StopBGM(Sound s, float time)
    {
        while(s.source.volume > 0)
        {
            s.source.volume -= Time.unscaledDeltaTime / time;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        s.source.Stop();
    }

}
