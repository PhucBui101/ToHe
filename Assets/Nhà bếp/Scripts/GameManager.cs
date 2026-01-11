using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Cài đặt")]
    public int totalItemsNeeded = 5;
    
    [Header("UI Checklist")]
    public GameObject checklistPanel;
    public GameObject check_Luoc;
    public GameObject check_DaoNhua;
    public GameObject check_OngTre;
    public GameObject check_SapOng;
    public GameObject check_Noi;
    
    [Header("UI Khác")]
    public GameObject promptTextObject;
    
    [Header("=== DIALOGUE (Khung chat thoại) ===")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    
    [TextArea(2, 5)]
    public string[] dialogueLines;
    
    [Header("=== LEVEL INFO (Popup thông tin màn) ===")]
    public GameObject levelInfoPopup;
    public Button levelInfoCloseButton;
    
    [Header("=== UNLOCK BOOK (Popup mở khóa sổ) ===")]
    public GameObject unlockBookPopup;
    public Button unlockCloseButton;
    
    [Header("=== QUYỂN SÁCH ===")]
    public GameObject bookObject;
    
    [Header("=== SETTINGS ===")]
    public float initialDelay = 0.5f;
    public float afterDialogueDelay = 0.5f;
    public float unlockPopupDelay = 0.5f;
    public float bookAppearDelay = 0.5f;
    
    private List<string> collectedItems = new List<string>();
    private int currentLineIndex = 0;
    private bool allItemsCollected = false;
    
    public static GameManager Instance { get; private set; }
    
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
        if (promptTextObject != null)
        {
            promptTextObject.SetActive(false);
        }
        
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (levelInfoPopup != null) levelInfoPopup.SetActive(false);
        if (checklistPanel != null) checklistPanel.SetActive(false);
        if (unlockBookPopup != null) unlockBookPopup.SetActive(false);
        
        // Ẩn sách ban đầu
        if (bookObject != null)
        {
            bookObject.SetActive(false);
        }
        
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
        
        if (levelInfoCloseButton != null)
        {
            levelInfoCloseButton.onClick.AddListener(OnLevelInfoClosed);
        }
        
        if (unlockCloseButton != null)
        {
            unlockCloseButton.onClick.AddListener(OnUnlockBookClosed);
        }
        
        Invoke("StartDialogue", initialDelay);
    }
    
    void Update()
    {
        // Click chuột trái để tiếp tục dialogue
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnContinueClicked();
            }
        }
        
        // Click chuột trái để đóng level info
        if (levelInfoPopup != null && levelInfoPopup.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLevelInfoClosed();
            }
        }
        
        // Click chuột trái để đóng unlock popup
        if (unlockBookPopup != null && unlockBookPopup.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnUnlockBookClosed();
            }
        }
    }
    
    // ==================== DIALOGUE ====================
    
    void StartDialogue()
    {
        currentLineIndex = 0;
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            ShowCurrentLine();
        }
        
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void ShowCurrentLine()
    {
        if (dialogueText != null && currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }
    
    void OnContinueClicked()
    {
        currentLineIndex++;
        
        if (currentLineIndex < dialogueLines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }
    
    void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        
        StartCoroutine(ShowLevelInfoAfterDelay());
    }
    
    // ==================== LEVEL INFO ====================
    
    System.Collections.IEnumerator ShowLevelInfoAfterDelay()
    {
        yield return new WaitForSecondsRealtime(afterDialogueDelay);
        
        if (levelInfoPopup != null)
        {
            levelInfoPopup.SetActive(true);
        }
    }
    
    void OnLevelInfoClosed()
    {
        if (levelInfoPopup != null)
        {
            levelInfoPopup.SetActive(false);
        }
        
        if (checklistPanel != null)
        {
            checklistPanel.SetActive(true);
        }
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // ==================== UNLOCK BOOK ====================
    
    public void TriggerUnlockBookPopup()
    {
        StartCoroutine(ShowUnlockBookAfterDelay());
    }
    
    System.Collections.IEnumerator ShowUnlockBookAfterDelay()
    {
        yield return new WaitForSecondsRealtime(unlockPopupDelay);
        
        if (checklistPanel != null)
        {
            checklistPanel.SetActive(false);
        }
        
        if (unlockBookPopup != null)
        {
            unlockBookPopup.SetActive(true);
        }
        
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void OnUnlockBookClosed()
    {
        if (unlockBookPopup != null)
        {
            unlockBookPopup.SetActive(false);
        }
        
        // Tiếp tục game
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Hiện sách sau một chút delay
        StartCoroutine(ShowBookAfterDelay());
    }
    
    System.Collections.IEnumerator ShowBookAfterDelay()
    {
        yield return new WaitForSeconds(bookAppearDelay);
        
        if (bookObject != null)
        {
            bookObject.SetActive(true);
            
            // Kích hoạt script BookInteraction trên sách
            BookInteraction bookScript = bookObject.GetComponent<BookInteraction>();
            if (bookScript != null)
            {
                bookScript.ActivateBook();
            }
            
            Debug.Log("Quyển sách bí ẩn đã xuất hiện! Hãy tìm và tương tác với nó.");
        }
    }
    
    // ==================== COLLECT ITEMS ====================

    public void CollectItem(string itemName)
    {
        collectedItems.Add(itemName);
        UpdateChecklist(itemName);
        
        if (collectedItems.Count >= totalItemsNeeded)
        {
            allItemsCollected = true;
        }
    }
    
    public bool IsAllItemsCollected()
    {
        return allItemsCollected;
    }

    void UpdateChecklist(string itemName)
    {
        switch (itemName)
        {
            case "Luoc":
                SetCheckmark(check_Luoc, "Lược");
                break;
            case "DaoNhua":
                SetCheckmark(check_DaoNhua, "Dao nhựa");
                break;
            case "OngTre":
                SetCheckmark(check_OngTre, "Ống tre");
                break;
            case "SapOng":
                SetCheckmark(check_SapOng, "Sáp ong");
                break;
            case "Noi":
                SetCheckmark(check_Noi, "Nồi");
                break;
        }
    }
    
    void SetCheckmark(GameObject checkObject, string displayName)
    {
        if (checkObject != null)
        {
            TextMeshProUGUI tmpText = checkObject.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = "[X] " + displayName;
                tmpText.color = Color.green;
            }
            
            Text normalText = checkObject.GetComponent<Text>();
            if (normalText != null)
            {
                normalText.text = "[X] " + displayName;
                normalText.color = Color.green;
            }
        }
    }
    
    public void ShowPrompt(bool show)
    {
        if (promptTextObject != null)
        {
            promptTextObject.SetActive(show);
        }
    }
}