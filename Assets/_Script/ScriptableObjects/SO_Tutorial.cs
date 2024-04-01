using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial")]
public class SO_Tutorial : ScriptableObject
{
    public LocalizedString title;
    public LocalizedString description;
    public VideoClip clip;
    public bool returnToSavepoint = false;
}