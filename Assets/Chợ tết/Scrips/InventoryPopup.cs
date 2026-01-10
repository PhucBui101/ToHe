using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryPopup : MonoBehaviour
{
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
#endif

    /// <summary>
    /// Parses items from InventoryManager and creates UI elements for each item.
    /// </summary>
    public void ParseData()
    {
        // Release cursor lock state to interact with dialog
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        ClearItemList();
        chosingData = null;

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryPopup: InventoryManager.Instance is null");
            return;
        }

        if (itemsContainer == null)
        {
            Debug.LogWarning("InventoryPopup: Items container is not assigned");
            return;
        }

        if (itemUIPrefab == null)
        {
            Debug.LogWarning("InventoryPopup: Item UI prefab is not assigned");
            return;
        }

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

        // Select first item by default if available
        if (currentItemUIs.Count > 0 && currentItemUIs[0] != null)
        {
            // Set the toggle to selected state, which will trigger the callback
            currentItemUIs[0].isOn = true;
        }
        else
        {
            HideItemInfo();
        }
    }

    private void CreateItemUI(ItemData itemData)
    {
        if (itemUIPrefab == null || itemsContainer == null) return;

        InventoryItemUI itemUI = Instantiate(itemUIPrefab, itemsContainer);
        itemUI.Setup(itemData, OnItemSelected);
        
        // Assign toggle group if available (ensures only one item can be selected at a time)
        if (toggleGroup != null)
        {
            itemUI.group = toggleGroup;
        }
        
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
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }

        if (infoIconImage != null)
        {
            infoIconImage.sprite = item.icon;
            //infoIconImage.enabled = item.icon != null;
        }

        if (infoNameText != null)
        {
            infoNameText.text = item.itemName ?? string.Empty;
        }

        if (infoDescriptionText != null)
        {
            infoDescriptionText.text = item.description ?? string.Empty;
        }

        btnUse.interactable = item.isUsableAtKitchen;
    }

    private void HideItemInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        if (infoIconImage != null)
        {
            infoIconImage.sprite = null;
            infoIconImage.enabled = false;
        }

        if (infoNameText != null)
        {
            infoNameText.text = string.Empty;
        }

        if (infoDescriptionText != null)
        {
            infoDescriptionText.text = string.Empty;
        }
    }

    private void ClearItemList()
    {
        foreach (InventoryItemUI itemUI in currentItemUIs)
        {
            if (itemUI != null)
            {
                Destroy(itemUI.gameObject);
            }
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

            // Refresh UI data by parsing again instead of closing dialog
            ParseData();
        }
        else
        {
            CloseDialog();
        }
    }

    #region Base Show/Hide

    public static InventoryPopup ShowDialog()
    {
        var d = FindObjectOfType<InventoryPopup>(includeInactive: true);
        if (d != null && !d.isActiveAndEnabled)
        {
            d.gameObject.SetActive(true);
            d.ParseData();
            d.AnimationShow();
            return d;
        }

        return null;
    }

    protected virtual void AnimationShow()
    {
        this.panel.localScale = Vector3.zero;
        if (this.canvasGroup != null)
        {
            this.canvasGroup.alpha = 1;
        }
        
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.OnComplete(this.OnCompleteShow).SetDelay(0.001f);
    }

    protected virtual void OnCompleteShow()
    {
    }

    protected virtual void AnimationHide()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(0.0f, 0.2f).SetEase(Ease.Linear));
        if (this.canvasGroup != null)
        {
            seq.Join(this.canvasGroup.DOFade(0, 0.2f));
        }
        seq.OnComplete(this.OnCompleteHide).SetId(this.panel);
    }

    protected virtual void OnCompleteHide()
    {
        // Set cursor back to locked state when dialog closes
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
        this.gameObject.SetActive(false);
        Clear();
    }

    public void CloseDialog()
    {
        AnimationHide();
    }

    private void Clear()
    {
        ClearItemList();
        HideItemInfo();
    }

    #endregion
}
