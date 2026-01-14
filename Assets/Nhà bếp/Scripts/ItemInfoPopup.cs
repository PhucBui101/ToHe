using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfoPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public Button closeButton;

    public TextMeshProUGUI itemDescriptionText;

    [Header("Settings")]
    public bool pauseGameWhenOpen = true;

    public static ItemInfoPopup Instance { get; private set; }

    private bool isPopupOpen = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePopup);
        }
    }

    void Update()
    {
        if (isPopupOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
    }

    public void ShowPopup(ItemData itemData)
    {
        popupPanel.SetActive(true);
        isPopupOpen = true;

        if (itemData != null)
        {
            itemDescriptionText.text = $"Thu được {itemData.itemName}: {itemData.description}";
        }

        if (pauseGameWhenOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        isPopupOpen = false;

        if (pauseGameWhenOpen)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        // Kiểm tra nếu đã thu thập đủ 5 món thì hiện unlock popup
        if (GameManager.Instance != null && GameManager.Instance.IsAllItemsCollected())
        {
            GameManager.Instance.TriggerUnlockBookPopup();
        }
    }

    public bool IsPopupOpen()
    {
        return isPopupOpen;
    }
}