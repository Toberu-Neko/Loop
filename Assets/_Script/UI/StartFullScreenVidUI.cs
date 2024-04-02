using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class StartFullScreenVidUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private PlayableDirector L1StartDirector;

    private void OnEnable()
    {
        player.loopPointReached += EndReached;
    }

    private void OnDisable()
    {
        player.loopPointReached -= EndReached;
    }

    private void EndReached(VideoPlayer source)
    {
        Deactivate();
        L1StartDirector.Play();
    }

    public void Actvitate()
    {
        GameManager.Instance.PauseGame();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
