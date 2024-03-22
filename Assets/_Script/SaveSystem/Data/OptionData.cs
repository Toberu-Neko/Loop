using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public int languageIndex;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float voiceVolume;
    public float uiVolume;

    public OptionData(int lanIndex = 2)
    {
        languageIndex = lanIndex;
        masterVolume = 1f;
        musicVolume = 1f;
        sfxVolume = 1f;
        voiceVolume = 1f;
        uiVolume = 1f;
    }
}
