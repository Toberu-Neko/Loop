using UnityEngine;

public class TurnOffUIAnimationTrigger : MonoBehaviour
{
    private Animator anim;

    private bool forceTurnOffUI;
    private float activeTime;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        activeTime = Time.time;
        forceTurnOffUI = false;
    }

    private void Update()
    {
        /*
        if(Time.time > activeTime + 0.75f && !forceTurnOffUI)
        {
            forceTurnOffUI = true;
            // Debug.LogError("The turnoff ui bug.");
            anim.SetBool("finishLoading", true);
        }
        */
    }

    private void OnDisable()
    {
        
    }

    public void OnTurnOffUI()
    {
        anim.SetBool("finishLoading", false);
        gameObject.SetActive(false);
    }
}
