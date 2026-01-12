using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public Button closeButton;
    public Image itemImage;  // Đã thêm biến này

    [Header("Ảnh đồ vật")]   // Đã thêm Header và các biến Sprite
    public Sprite luocSprite;      // Ảnh Lược
    public Sprite daoNhuaSprite;   // Ảnh Dao nhựa
    public Sprite ongTreSprite;    // Ảnh Ống tre
    public Sprite sapOngSprite;    // Ảnh Sáp ong
    public Sprite noiSprite;       // Ảnh Nồi

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

    // Hàm ShowPopup đã được sửa đổi để nhận tham số itemName
    public void ShowPopup(string itemName = "")
    {
        popupPanel.SetActive(true);
        isPopupOpen = true;

        // Hiển thị ảnh tương ứng với đồ vật
        if (itemImage != null && itemName != "")
        {
            switch (itemName)
            {
                case "Luoc":
                    itemImage.sprite = luocSprite;
                    break;
                case "DaoNhua":
                    itemImage.sprite = daoNhuaSprite;
                    break;
                case "OngTre":
                    itemImage.sprite = ongTreSprite;
                    break;
                case "SapOng":
                    itemImage.sprite = sapOngSprite;
                    break;
                case "Noi":
                    itemImage.sprite = noiSprite;
                    break;
            }
            
            // Đảm bảo ảnh hiển thị đúng tỷ lệ gốc (tùy chọn, bạn có thể bỏ nếu không cần)
            itemImage.preserveAspect = true; 
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