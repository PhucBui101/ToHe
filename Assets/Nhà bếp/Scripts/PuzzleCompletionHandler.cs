using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleCompletionHandler : MonoBehaviour
{
    [Header("=== DIALOG UI ===")]
    [Tooltip("Panel hộp thoại")]
    public GameObject dialogPanel;
    
    [Tooltip("Text nội dung hộp thoại")]
    public TextMeshProUGUI dialogText;
    
    [Tooltip("Text hướng dẫn (Click để tiếp tục)")]
    public TextMeshProUGUI continueText;
    
    [Header("=== DIALOG CONTENT ===")]
    [Tooltip("Danh sách các đoạn hội thoại")]
    [TextArea(2, 4)]
    public string[] dialogMessages = new string[]
    {
        "Ông ơi, con đã nhớ lại hết rồi...",
        "Những kỷ niệm về nghề làm tò he thật đẹp.",
        "Con sẽ tiếp tục giữ gìn nghề truyền thống này."
    };
    
    [Header("=== SETTINGS ===")]
    [Tooltip("Thời gian chờ trước khi hiện hộp thoại")]
    public float delayBeforeDialog = 1f;
    
    [Tooltip("Khóa di chuyển khi đang hiện hộp thoại")]
    public bool lockMovementDuringDialog = true;
    
    [Header("=== REFERENCES ===")]
    [Tooltip("Player controller (để khóa di chuyển)")]
    public MonoBehaviour playerController;
    
    [Tooltip("GameManager hoặc script quản lý game")]
    public MonoBehaviour gameManager;
    
    // Biến nội bộ
    private int currentDialogIndex = 0;
    private bool isDialogActive = false;
    private bool hasShownDialog = false;
    
    void Start()
    {
        // Ẩn hộp thoại ban đầu
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
        
        // Kiểm tra xem có cần hiện hộp thoại không
        if (GameStateManager.Instance != null && GameStateManager.Instance.showDialogOnReturn)
        {
            StartCoroutine(ShowDialogAfterDelay());
        }
    }
    
    void Update()
    {
        // Nếu đang hiện hộp thoại và click chuột trái
        if (isDialogActive && Input.GetMouseButtonDown(0))
        {
            NextDialog();
        }
    }
    
    IEnumerator ShowDialogAfterDelay()
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(delayBeforeDialog);
        
        // Bắt đầu hiện hộp thoại
        StartDialog();
    }
    
    void StartDialog()
    {
        if (hasShownDialog) return;
        
        isDialogActive = true;
        currentDialogIndex = 0;
        
        // Khóa di chuyển
        if (lockMovementDuringDialog && playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Hiện panel
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
        }
        
        // Hiện đoạn đầu tiên
        ShowCurrentDialog();
        
        // Đánh dấu đã hiện
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.DialogShown();
        }
    }
    
    void ShowCurrentDialog()
    {
        if (dialogText != null && currentDialogIndex < dialogMessages.Length)
        {
            dialogText.text = dialogMessages[currentDialogIndex];
        }
        
        // Cập nhật text hướng dẫn
        if (continueText != null)
        {
            if (currentDialogIndex < dialogMessages.Length - 1)
            {
                continueText.text = "Click để tiếp tục...";
            }
            else
            {
                continueText.text = "Click để đóng";
            }
        }
    }
    
    void NextDialog()
    {
        currentDialogIndex++;
        
        if (currentDialogIndex >= dialogMessages.Length)
        {
            // Hết hội thoại
            EndDialog();
        }
        else
        {
            // Hiện đoạn tiếp theo
            ShowCurrentDialog();
        }
    }
    
    void EndDialog()
    {
        isDialogActive = false;
        hasShownDialog = true;
        
        // Ẩn panel
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
        
        // Mở khóa di chuyển
        if (lockMovementDuringDialog && playerController != null)
        {
            playerController.enabled = true;
        }
        
        // Có thể gọi event hoặc hàm khác ở đây
        OnDialogComplete();
    }
    
    void OnDialogComplete()
    {
        // Gọi khi hoàn thành hội thoại
        // Bạn có thể thêm logic ở đây
        Debug.Log("Dialog completed! Player can now move freely.");
    }
    
    // Public method để gọi từ bên ngoài nếu cần
    public void ForceShowDialog()
    {
        hasShownDialog = false;
        StartDialog();
    }
}
