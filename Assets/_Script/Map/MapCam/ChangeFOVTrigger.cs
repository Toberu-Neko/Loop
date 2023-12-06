using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFOVTrigger : MonoBehaviour
{
    [SerializeField] private float newFOV = 80f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ChangeFOVTrigger");
            CamManager.Instance.ChangeFOV(newFOV);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CamManager.Instance.ChangeFOV();
        }
    }
}
