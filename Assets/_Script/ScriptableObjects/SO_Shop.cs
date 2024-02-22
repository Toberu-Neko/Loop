using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Shop", menuName = "Map/Shop")]
public class SO_Shop : ScriptableObject
{
    public string shopID;
    public LocalizedString shopName;

    public List<ShopInventory> shopInventory;

    [System.Serializable]
    public class ShopInventory
    {
        public SO_ItemsBase item;
        public int quantity;
    }
}
