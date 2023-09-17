using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IMapDamageableItem, ITempDataPersistence, IUniqueID
{
    public bool isAddedID { get; set; }
    public string ID { get; set; }
    [SerializeField] private int health = 1;

    private bool isDefeated = false;

    public void LoadTempData(TempData data)
    {
        data.defeatedObjects.TryGetValue(ID, out isDefeated);

        if (isDefeated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveTempData(TempData data)
    {
        if (data.defeatedObjects.ContainsKey(ID))
        {
            data.defeatedObjects.Remove(ID);
        }
        data.defeatedObjects.Add(ID, isDefeated);
    }

    public void TakeDamage(float damage)
    {
        health --;
        if (health == 0)
        {
            gameObject.SetActive(false);
            isDefeated = true;
        }
    }
}
