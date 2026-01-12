using UnityEngine;
using UnityEngine.SceneManagement;

public class BookInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptText;
    
    [Header("Settings")]
    public float interactDistance = 3f;
    
    private bool isActivated = false;
    private Transform playerTransform;
    private bool playerNearby = false;

    void Start()
    {
        isActivated = false;
        playerNearby = false;
        
        if (promptText != null)
        {
            promptText.SetActive(false);
        }
        
        // Tìm Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (!isActivated) return;

        // --- PHẦN ĐÃ SỬA ---
        // Không cho tương tác nếu đã hoàn thành puzzle
        if (GameStateManager.Instance != null && GameStateManager.Instance.puzzleCompleted)
        {
            if (promptText != null) promptText.SetActive(false);
            return;
        }
        // -------------------
        
        // Kiểm tra khoảng cách với Player
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distance <= interactDistance)
            {
                // Player ở gần
                if (!playerNearby)
                {
                    playerNearby = true;
                    if (promptText != null)
                    {
                        promptText.SetActive(true);
                    }
                    Debug.Log("Player ở gần sách!");
                }
                
                // Nhấn E để tương tác
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenPuzzleGame();
                }
            }
            else
            {
                // Player ở xa
                if (playerNearby)
                {
                    playerNearby = false;
                    if (promptText != null)
                    {
                        promptText.SetActive(false);
                    }
                }
            }
        }
    }

    public void ActivateBook()
    {
        isActivated = true;
        Debug.Log("Sách đã được kích hoạt!");
    }

    void OpenPuzzleGame()
    {
        Debug.Log("Mở Puzzle Game!");
        
        if (promptText != null)
        {
            promptText.SetActive(false);
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene("Puzzle Game Bep");
    }
}