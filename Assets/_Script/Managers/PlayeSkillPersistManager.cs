using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeSkillPersistManager : MonoBehaviour, IDataPersistance
{
    public static PlayeSkillPersistManager Instance { get; private set; }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockTimeSkill()
    {
    }

    public void LoadData(GameData data)
    {
    }

    public void SaveData(GameData data)
    {
    }

}
