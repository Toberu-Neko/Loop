using UnityEngine;

public class DataPersistMapObjBase : MonoBehaviour, IDataPersistance
{
    [Header("ID")]
    public bool isAddedID;
    public string ID;

    protected bool isActivated;

    protected virtual void Start()
    {
        DataPersistenceManager.Instance.GameData.activatedMapItem.TryGetValue(ID, out isActivated);
    }

    public void LoadData(GameData data)
    {
        Debug.LogError("LoadData not implemented in " + gameObject.name + ", use start() instead.");
    }

    public void SaveData(GameData data)
    {
        if (ID == "")
        {
            Debug.LogError("ID is empty, data not saved. Object: " + gameObject.name);
            return;
        }

        if(data.activatedMapItem.ContainsKey(ID))
        {
            data.activatedMapItem[ID] = isActivated;
        }
        else
        {
            data.activatedMapItem.Add(ID, isActivated);
        }
    }
}
