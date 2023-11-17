using TMPro;
using UnityEngine;

public class PauseInvDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void Activate(SO_ItemsBase item)
    {
        gameObject.SetActive(true);

        itemName.text = item.displayName;
        itemDescription.text = item.itemDescription;
    }

    public void Deactivate()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        gameObject.SetActive(false);
        ClearDescription();
    }

    public void ClearDescription()
    {
        itemName.text = "";
        itemDescription.text = "";
    }


}
