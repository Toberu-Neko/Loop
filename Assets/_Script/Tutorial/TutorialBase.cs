using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBase : MonoBehaviour
{
    protected virtual void Start()
    {
        if (DataPersistenceManager.Instance.GameData.finishTutorial)
        {
            gameObject.SetActive(false);
        }
    }
}
