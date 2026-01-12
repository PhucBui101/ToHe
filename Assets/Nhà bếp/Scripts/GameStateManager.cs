using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Singleton - chỉ có 1 instance duy nhất
    public static GameStateManager Instance { get; private set; }
    
    [Header("=== GAME STATE ===")]
    public bool allItemsCollected = false;    // Đã thu thập hết đồ vật chưa
    public bool puzzleCompleted = false;      // Đã hoàn thành puzzle chưa
    public bool showDialogOnReturn = false;   // Có hiện hộp thoại khi quay về không
    public bool gameFullyCompleted = false;   // Đã hoàn thành toàn bộ game chưa
    
    [Header("=== SKIP INTRO ===")]
    public bool skipIntroOnReturn = false;    // Bỏ qua intro/hộp thoại mở đầu khi quay về
    
    void Awake()
    {
        // Singleton pattern - giữ object này khi chuyển scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Không bị xóa khi load scene mới
        }
        else
        {
            Destroy(gameObject);  // Xóa nếu đã có instance khác
        }
    }
    
    // Gọi khi thu thập hết đồ vật
    public void SetAllItemsCollected()
    {
        allItemsCollected = true;
    }
    
    // Gọi khi hoàn thành puzzle
    public void SetPuzzleCompleted()
    {
        puzzleCompleted = true;
        showDialogOnReturn = true;
        skipIntroOnReturn = true;  // Bỏ qua intro khi quay về
    }
    
    // Gọi sau khi đã hiện hộp thoại
    public void DialogShown()
    {
        showDialogOnReturn = false;
    }
    
    // Gọi khi hoàn thành toàn bộ (sau hội thoại cuối)
    public void SetGameFullyCompleted()
    {
        gameFullyCompleted = true;
    }
    
    // Kiểm tra có nên bỏ qua intro không
    public bool ShouldSkipIntro()
    {
        return skipIntroOnReturn || puzzleCompleted;
    }
    
    // Kiểm tra có nên hiện dialog completion không
    public bool ShouldShowCompletionDialog()
    {
        return showDialogOnReturn && puzzleCompleted;
    }
    
    // Reset tất cả (nếu cần chơi lại)
    public void ResetAll()
    {
        allItemsCollected = false;
        puzzleCompleted = false;
        showDialogOnReturn = false;
        gameFullyCompleted = false;
        skipIntroOnReturn = false;
    }
}