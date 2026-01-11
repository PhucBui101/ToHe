using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BookFlipManager : MonoBehaviour
{
    [Header("=== BOOK PAGES ===")]
    public RectTransform leftPage;
    public RectTransform rightPage;
    public RectTransform flippingPage;
    public Image leftPageImage;
    public Image rightPageImage;
    public Image flippingPageFront;
    public Image flippingPageBack;
    
    [Header("=== PAGE SHADOW ===")]
    public Image pageShadow;
    public Image flipShadow;
    
    [Header("=== PAGE CONTENT SPRITES ===")]
    [Tooltip("Hình cho trang trái: Intro, Level1, Level2, Level3...")]
    public Sprite[] leftPageSprites;
    [Tooltip("Hình cho trang phải: Intro, Level1, Level2, Level3...")]
    public Sprite[] rightPageSprites;
    
    [Header("=== PUZZLE SETTINGS ===")]
    [Tooltip("Số cột của puzzle")]
    public int puzzleColumns = 3;
    [Tooltip("Số hàng của puzzle")]
    public int puzzleRows = 3;
    
    [Header("=== PUZZLE ELEMENTS ===")]
    public GameObject puzzleContainer;
    public Image[] puzzlePieces;
    public Button[] puzzleButtons;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI completeText;
    public TextMeshProUGUI instructionText;
    
    [Header("=== LEVEL DATA ===")]
    public PuzzleLevelData[] levels;
    
    [Header("=== SETTINGS ===")]
    public string mainSceneName = "Nha bep";
    public float flipDuration = 0.8f;
    public Color pageColor = new Color(1f, 0.97f, 0.91f);
    
    // Trạng thái game
    private int currentPageIndex = 0;
    private int currentLevel = -1;
    private int[] currentRotations;
    private bool isFlipping = false;
    private bool isLevelComplete = false;
    private int totalPieces;
    
    private enum GameState { Intro, Playing, LevelComplete, GameComplete }
    private GameState currentState = GameState.Intro;
    
    void Start()
    {
        totalPieces = puzzleColumns * puzzleRows;
        currentRotations = new int[totalPieces];
        
        SetupInitialState();
        SetupButtons();
    }
    
    void SetupInitialState()
    {
        currentPageIndex = 0;
        UpdatePageDisplay();
        
        if (puzzleContainer != null)
            puzzleContainer.SetActive(false);
        
        if (flippingPage != null)
            flippingPage.gameObject.SetActive(false);
        
        if (completeText != null)
            completeText.gameObject.SetActive(false);
        
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(true);
            instructionText.text = ">>> Nhấn SPACE để bắt đầu <<<";
        }
        
        currentState = GameState.Intro;
    }
    
    void SetupButtons()
    {
        int buttonCount = Mathf.Min(puzzleButtons.Length, totalPieces);
        
        for (int i = 0; i < buttonCount; i++)
        {
            int index = i;
            if (puzzleButtons[i] != null)
            {
                puzzleButtons[i].onClick.AddListener(() => OnPieceClicked(index));
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFlipping)
        {
            HandleSpacePress();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainScene();
        }
    }
    
    void HandleSpacePress()
    {
        switch (currentState)
        {
            case GameState.Intro:
                StartCoroutine(FlipToNextPage());
                break;
                
            case GameState.LevelComplete:
                StartCoroutine(FlipToNextPage());
                break;
                
            case GameState.GameComplete:
                BackToMainScene();
                break;
        }
    }
    
    void UpdatePageDisplay()
    {
        // Cập nhật hình trang trái
        if (leftPageImage != null)
        {
            if (leftPageSprites != null && currentPageIndex < leftPageSprites.Length && 
                leftPageSprites[currentPageIndex] != null)
            {
                leftPageImage.sprite = leftPageSprites[currentPageIndex];
                leftPageImage.color = Color.white;
            }
            else
            {
                leftPageImage.sprite = null;
                leftPageImage.color = pageColor;
            }
        }
        
        // Cập nhật hình trang phải
        if (rightPageImage != null)
        {
            if (rightPageSprites != null && currentPageIndex < rightPageSprites.Length && 
                rightPageSprites[currentPageIndex] != null)
            {
                rightPageImage.sprite = rightPageSprites[currentPageIndex];
                rightPageImage.color = Color.white;
            }
            else
            {
                rightPageImage.sprite = null;
                rightPageImage.color = pageColor;
            }
        }
    }
    
    IEnumerator FlipToNextPage()
    {
        isFlipping = true;
        
        if (instructionText != null)
            instructionText.gameObject.SetActive(false);
        
        if (puzzleContainer != null)
            puzzleContainer.SetActive(false);
        
        if (completeText != null)
            completeText.gameObject.SetActive(false);
        
        yield return StartCoroutine(PageFlipAnimation());
        
        currentPageIndex++;
        
        if (currentPageIndex > levels.Length)
        {
            currentState = GameState.GameComplete;
            UpdatePageDisplay();
            
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(true);
                instructionText.text = ">>> Nhấn SPACE để quay lại <<<";
            }
        }
        else
        {
            currentLevel = currentPageIndex - 1;
            UpdatePageDisplay();
            
            if (puzzleContainer != null)
                puzzleContainer.SetActive(true);
            
            LoadLevel(currentLevel);
            currentState = GameState.Playing;
        }
        
        isFlipping = false;
    }
    
    IEnumerator PageFlipAnimation()
    {
        if (flippingPage == null || flippingPageFront == null || flippingPageBack == null)
        {
            yield return new WaitForSeconds(flipDuration);
            yield break;
        }
        
        flippingPage.gameObject.SetActive(true);
        
        // Đặt hình mặt trước
        if (rightPageSprites != null && currentPageIndex < rightPageSprites.Length && 
            rightPageSprites[currentPageIndex] != null)
        {
            flippingPageFront.sprite = rightPageSprites[currentPageIndex];
            flippingPageFront.color = Color.white;
        }
        else
        {
            flippingPageFront.sprite = null;
            flippingPageFront.color = pageColor;
        }
        
        // Đặt hình mặt sau
        int nextIndex = currentPageIndex + 1;
        if (leftPageSprites != null && nextIndex < leftPageSprites.Length && 
            leftPageSprites[nextIndex] != null)
        {
            flippingPageBack.sprite = leftPageSprites[nextIndex];
            flippingPageBack.color = Color.white;
        }
        else
        {
            flippingPageBack.sprite = null;
            flippingPageBack.color = pageColor;
        }
        
        flippingPageFront.gameObject.SetActive(true);
        flippingPageBack.gameObject.SetActive(false);
        
        flippingPage.localScale = Vector3.one;
        flippingPage.localRotation = Quaternion.identity;
        flippingPage.anchoredPosition = Vector2.zero;
        
        float halfDuration = flipDuration / 2f;
        float elapsed = 0f;
        
        // PHASE 1: Gập trang
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easedT = EaseInOutCubic(t);
            
            float scaleX = Mathf.Lerp(1f, 0.01f, easedT);
            flippingPage.localScale = new Vector3(scaleX, 1f, 1f);
            
            float posX = Mathf.Lerp(0f, -flippingPage.rect.width * 0.25f, easedT);
            flippingPage.anchoredPosition = new Vector2(posX, 0);
            
            if (flipShadow != null)
            {
                float alpha = Mathf.Lerp(0f, 0.5f, easedT);
                flipShadow.color = new Color(0, 0, 0, alpha);
            }
            
            yield return null;
        }
        
        // Đổi mặt
        flippingPageFront.gameObject.SetActive(false);
        flippingPageBack.gameObject.SetActive(true);
        
        // PHASE 2: Mở trang
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easedT = EaseInOutCubic(t);
            
            float scaleX = Mathf.Lerp(0.01f, 1f, easedT);
            flippingPage.localScale = new Vector3(scaleX, 1f, 1f);
            
            float posX = Mathf.Lerp(-flippingPage.rect.width * 0.25f, -flippingPage.rect.width * 0.5f, easedT);
            flippingPage.anchoredPosition = new Vector2(posX, 0);
            
            if (flipShadow != null)
            {
                float alpha = Mathf.Lerp(0.5f, 0f, easedT);
                flipShadow.color = new Color(0, 0, 0, alpha);
            }
            
            yield return null;
        }
        
        flippingPage.gameObject.SetActive(false);
        flippingPage.localScale = Vector3.one;
        flippingPage.anchoredPosition = Vector2.zero;
    }
    
    float EaseInOutCubic(float t)
    {
        return t < 0.5f 
            ? 4f * t * t * t 
            : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
    
    void OnPieceClicked(int index)
    {
        if (currentState != GameState.Playing || isLevelComplete || isFlipping) return;
        if (index >= currentRotations.Length) return;
        
        currentRotations[index] = (currentRotations[index] + 90) % 360;
        
        if (index < puzzlePieces.Length && puzzlePieces[index] != null)
        {
            puzzlePieces[index].transform.rotation = Quaternion.Euler(0, 0, -currentRotations[index]);
        }
        
        CheckLevelComplete();
    }
    
    void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length) return;
        
        PuzzleLevelData level = levels[levelIndex];
        isLevelComplete = false;
        
        if (levelText != null)
            levelText.text = $"Màn {levelIndex + 1}/{levels.Length}";
        
        if (completeText != null)
            completeText.gameObject.SetActive(false);
        
        if (level.puzzleImage == null || level.puzzleImage.texture == null)
        {
            Debug.LogError($"Level {levelIndex + 1}: Puzzle Image is missing!");
            return;
        }
        
        Texture2D mainTexture = level.puzzleImage.texture;
        
        int cols = puzzleColumns;
        int rows = puzzleRows;
        int pieceWidth = mainTexture.width / cols;
        int pieceHeight = mainTexture.height / rows;
        
        int pieceCount = Mathf.Min(totalPieces, puzzlePieces.Length);
        
        for (int i = 0; i < pieceCount; i++)
        {
            if (puzzlePieces[i] == null) continue;
            
            int col = i % cols;
            int row = i / cols;
            
            int x = col * pieceWidth;
            int y = (rows - 1 - row) * pieceHeight;
            
            Rect rect = new Rect(x, y, pieceWidth, pieceHeight);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            
            Sprite pieceSprite = Sprite.Create(mainTexture, rect, pivot);
            puzzlePieces[i].sprite = pieceSprite;
            
            int rotation = 0;
            if (level.initialRotations != null && i < level.initialRotations.Length)
            {
                rotation = level.initialRotations[i];
            }
            
            currentRotations[i] = rotation;
            puzzlePieces[i].transform.rotation = Quaternion.Euler(0, 0, -rotation);
        }
    }
    
    void CheckLevelComplete()
    {
        bool allCorrect = true;
        int pieceCount = Mathf.Min(totalPieces, currentRotations.Length);
        
        for (int i = 0; i < pieceCount; i++)
        {
            if (currentRotations[i] != 0)
            {
                allCorrect = false;
                break;
            }
        }
        
        if (allCorrect)
        {
            isLevelComplete = true;
            currentState = GameState.LevelComplete;
            
            if (completeText != null)
                completeText.gameObject.SetActive(true);
            
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(true);
                
                if (currentLevel >= levels.Length - 1)
                {
                    instructionText.text = ">>> Nhấn SPACE để hoàn thành <<<";
                }
                else
                {
                    instructionText.text = ">>> Nhấn SPACE để tiếp tục <<<";
                }
            }
        }
    }
    
    public void BackToMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}