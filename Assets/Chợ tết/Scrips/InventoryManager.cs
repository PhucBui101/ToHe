using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> playerItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemData item)
    {
        if (item != null)
        {
            playerItems.Add(item);
            Debug.Log("Obtained: " + item.itemName);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (item != null)
        {
            playerItems.Remove(item);
            Debug.Log("Removed: " + item.itemName);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            InventoryPopup.ShowDialog();
        }
    }

    public bool IsCollectFullColor(){
        List<ItemId> requiredColors = new List<ItemId>(){
            ItemId.Color_Red,
            ItemId.Color_Green,
            ItemId.Color_Blue,
            ItemId.Color_Yellow,
            ItemId.Color_White,
        };
        foreach (var color in requiredColors)
        {
            if (playerItems.Find(item => item.itemId.Equals(color)) == null) return false;
        }
        return true;
    }
    public bool IsCollectedFullKitchenItem(){
        List<ItemId> requiredKitchenItems = new List<ItemId>(){
            ItemId.KitchenItem_Pot,
            ItemId.KitchenItem_Knife,
            ItemId.KitchenItem_OngDua,
            ItemId.KitchenItem_Wasp,
        };
        foreach (var kitchenItem in requiredKitchenItems)
        {
            if (playerItems.Find(item => item.itemId.Equals(kitchenItem)) == null) return false;
        }
        return true;
    }

    public bool IsCanMakeToHe() => IsCollectFullColor() && IsCollectedFullKitchenItem();
}