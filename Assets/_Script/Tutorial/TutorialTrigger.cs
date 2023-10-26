using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField] private GameObject tutorialObject;

    private void Awake()
    {
        tutorialObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialObject.SetActive(false);
        }
    }
}
