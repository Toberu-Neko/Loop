using System;
using UnityEngine;

public class CamOffTrigger : MonoBehaviour
{
    public event Action OnTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }
}
