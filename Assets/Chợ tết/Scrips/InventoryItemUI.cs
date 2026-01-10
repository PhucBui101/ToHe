using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI component representing a single inventory item in the list.
/// Inherits from Toggle to handle selection state on/off.
/// Displays item icon and name, invokes callback when selected.
/// </summary>
public class InventoryItemUI : Toggle
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private ItemData itemData;
    private System.Action<ItemData> onSelectCallback;

    protected override void Awake()
    {
        base.Awake();
        
        // Subscribe to Toggle's value changed event
        onValueChanged.AddListener(HandleToggleValueChanged);
    }

    protected override void OnDestroy()
    {
        // Unsubscribe from Toggle's value changed event
        onValueChanged.RemoveListener(HandleToggleValueChanged);
        base.OnDestroy();
    }

    /// <summary>
    /// Initializes the UI element with item data and selection callback.
    /// </summary>
    /// <param name="data">The item data to display</param>
    /// <param name="onSelect">Callback invoked when this item is selected (toggle becomes on)</param>
    public void Setup(ItemData data, System.Action<ItemData> onSelect)
    {
        if (data == null)
        {
            Debug.LogWarning("InventoryItemUI: Setup called with null ItemData");
            return;
        }

        itemData = data;
        onSelectCallback = onSelect;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (itemData == null) return;

        if (iconImage != null)
        {
            iconImage.sprite = itemData.icon;
            //iconImage.enabled = itemData.icon != null;
        }

        if (nameText != null)
        {
            nameText.text = itemData.itemName ?? string.Empty;
        }
    }

    /// <summary>
    /// Handles Toggle value changes. Invokes callback when toggle is turned on.
    /// </summary>
    /// <param name="isOn">True when toggle is selected, false when deselected</param>
    private void HandleToggleValueChanged(bool isOn)
    {
        // Only invoke callback when toggle is turned ON (selected)
        if (isOn && itemData != null && onSelectCallback != null)
        {
            onSelectCallback.Invoke(itemData);
        }
    }
}
