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
            // CHECK: Look through playerItems to see if an item with the same ID already exists
            if (playerItems.Exists(i => i.itemId == item.itemId))
            {
                Debug.Log("Item " + item.itemName + " already in inventory. Not adding again.");
                return; // Exit the function without adding
            }

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

    private void Update()
    {
        
            if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryPopup popup = InventoryPopup.ShowDialog();
            InventoryPopup.ShowDialog();
            if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
            // If the popup was already active, ShowDialog might return null, 
            // so we use the Instance to force a refresh
            if (InventoryPopup.Instance != null)
            {
                InventoryPopup.Instance.ParseData();
            }
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
            ItemId.KitchenItem_Luoc
        };
        foreach (var kitchenItem in requiredKitchenItems)
        {
            if (playerItems.Find(item => item.itemId.Equals(kitchenItem)) == null) return false;
        }
        return true;
    }

    public bool IsCanMakeToHe() => IsCollectFullColor() && IsCollectedFullKitchenItem();
}