using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> playerItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(ItemData item)
    {
        if (item != null)
        {
            playerItems.Add(item);
            Debug.Log("Obtained: " + item.itemName);
        }
    }
}