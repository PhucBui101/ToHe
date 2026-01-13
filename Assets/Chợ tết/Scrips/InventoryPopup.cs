using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryPopup : MonoBehaviour
{
    // FIX: Add static instance so InventoryManager can find it
    public static InventoryPopup Instance;

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;

    [Header("Item List (Left Side)")]
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private InventoryItemUI itemUIPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    [Header("Item Info Display (Right Side)")]
    [SerializeField] private Image infoIconImage;
    [SerializeField] private TextMeshProUGUI infoNameText;
    [SerializeField] private TextMeshProUGUI infoDescriptionText;
    [SerializeField] private GameObject infoPanel;
    private ItemData chosingData;
    [SerializeField] private Button btnUse;

    private List<InventoryItemUI> currentItemUIs = new List<InventoryItemUI>();

    private void Awake()
    {
        // Initialize Singleton
        if (Instance == null) Instance = this;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }
#endif

    // FIX: This method must be public and robust to prevent invisible items
    public void ParseData()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClearItemList();
        chosingData = null;

        if (InventoryManager.Instance == null) return;
        if (itemsContainer == null || itemUIPrefab == null) return;

        List<ItemData> items = InventoryManager.Instance.playerItems;

        if (items == null || items.Count == 0)
        {
            HideItemInfo();
            return;
        }

        foreach (ItemData item in items)
        {
            if (item == null) continue;
            CreateItemUI(item);
        }

        if (currentItemUIs.Count > 0)
        {
            currentItemUIs[0].isOn = true;
        }
        else
        {
            HideItemInfo();
        }
    }

    // FIX: Renamed to match the RefreshUI call from InventoryManager if needed
    public void RefreshUI() => ParseData();

    private void CreateItemUI(ItemData itemData)
    {
        InventoryItemUI itemUI = Instantiate(itemUIPrefab, itemsContainer);
        itemUI.Setup(itemData, OnItemSelected);

        if (toggleGroup != null) itemUI.group = toggleGroup;

        itemUI.gameObject.SetActive(true);
        currentItemUIs.Add(itemUI);
    }

    private void OnItemSelected(ItemData selectedItem)
    {
        if (selectedItem == null)
        {
            HideItemInfo();
            return;
        }
        chosingData = selectedItem;
        ShowItemInfo(selectedItem);
    }

    private void ShowItemInfo(ItemData item)
    {
        if (infoPanel != null) infoPanel.SetActive(true);
        // FIX: Ensure icon is enabled when showing
        if (infoIconImage != null)
        {
            infoIconImage.sprite = item.icon; // Check if your ItemData uses 'icon' or 'itemIcon'
            infoIconImage.enabled = true;
        }
        if (infoNameText != null) infoNameText.text = item.itemName;
        if (infoDescriptionText != null) infoDescriptionText.text = item.description;
        if (btnUse != null) btnUse.interactable = item.isUsableAtKitchen;
    }

    private void HideItemInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    private void ClearItemList()
    {
        // Physically destroy old GameObjects to avoid duplicates or invisible ghosts
        foreach (InventoryItemUI itemUI in currentItemUIs)
        {
            if (itemUI != null) Destroy(itemUI.gameObject);
        }
        currentItemUIs.Clear();
    }

    public void ClickUseItem()
    {
        if (chosingData == null) return;
        if (chosingData.isUsableAtKitchen)
        {
            InventoryManager.Instance.RemoveItem(chosingData);
            InventoryManager.Instance.AddItem(chosingData.resultItemAfterUsed);
            ParseData();
        }
    }

    #region Base Show/Hide

    public static InventoryPopup ShowDialog()
    {
        var d = FindObjectOfType<InventoryPopup>(includeInactive: true);
        if (d != null)
        {
            d.gameObject.SetActive(true);
            d.ParseData(); // Ensure data is parsed every time Tab is pressed
            d.AnimationShow();
            return d;
        }
        return null;
    }

    protected virtual void AnimationShow()
    {
        this.panel.localScale = Vector3.zero;
        if (this.canvasGroup != null) this.canvasGroup.alpha = 1;
        this.panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void CloseDialog() => AnimationHide();

    protected virtual void AnimationHide()
    {
        this.panel.DOScale(0.0f, 0.2f).SetEase(Ease.Linear).OnComplete(() => {
            this.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Clear();
        });
    }

    private void Clear()
    {
        ClearItemList();
        HideItemInfo();
    }
    #endregion
}