using UnityEngine;

public class TurnOffUIAnimationTrigger : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnTurnOffUI()
    {
        anim.SetBool("finishLoading", false);
        gameObject.SetActive(false);
    }
}
