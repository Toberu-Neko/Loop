using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBase : MonoBehaviour
{
    [SerializeField] private Level level;
    private enum Level
    {
        one,
        two,
        three,
    }
    protected virtual void Start()
    {
        if(level == Level.one)
        {
            if (DataPersistenceManager.Instance.GameData.finishTutorial)
            {
                gameObject.SetActive(false);
            }
        }
        
    }
}
