using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookReturnHandler : MonoBehaviour
{
    [Header("=== DIALOG UI ===")]
    [Tooltip("Panel hộp thoại hiện sau khi hoàn thành puzzle")]
    public GameObject completionDialogPanel;
    
    [Tooltip("Text nội dung hộp thoại")]
    public TextMeshProUGUI dialogText;
    
    [Header("=== SETTINGS ===")]
    [Tooltip("Thời gian chờ trước khi hiện hộp thoại (giây)")]
    public float delayBeforeDialog = 1f;
    
    [Tooltip("Nội dung hộp thoại khi hoàn thành puzzle")]
    [TextArea(3, 5)]
    public string completionMessage = "Bạn đã hoàn thành tất cả câu đố!\n\nNhững kỷ niệm đẹp về nghề làm tò he đã được gợi nhớ...";
    
    void Start()
    {
        // Ẩn hộp thoại ban đầu
        if (completionDialogPanel != null)
            completionDialogPanel.SetActive(false);
        
        // Kiểm tra xem có cần hiện hộp thoại không
        if (GameStateManager.Instance != null && GameStateManager.Instance.showDialogOnReturn)
        {
            StartCoroutine(ShowCompletionDialog());
        }
    }
    
    IEnumerator ShowCompletionDialog()
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(delayBeforeDialog);
        
        // Hiện hộp thoại
        if (completionDialogPanel != null)
        {
            completionDialogPanel.SetActive(true);
            
            // Đặt nội dung
            if (dialogText != null)
            {
                dialogText.text = completionMessage;
            }
        }
        
        // Đánh dấu đã hiện hộp thoại
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.DialogShown();
        }
    }
    
    // Gọi khi nhấn nút đóng hộp thoại
    public void CloseDialog()
    {
        if (completionDialogPanel != null)
        {
            completionDialogPanel.SetActive(false);
        }
    }
    
    // Gọi từ nút hoặc phím để đóng
    void Update()
    {
        // Nhấn Space hoặc E để đóng hộp thoại
        if (completionDialogPanel != null && completionDialogPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                CloseDialog();
            }
        }
    }
}