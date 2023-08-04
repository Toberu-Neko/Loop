using UnityEngine;

public class TurnOffUIAnimationTrigger : MonoBehaviour
{
    public void OnTurnOffUI()
    {
        gameObject.SetActive(false);
    }
}
