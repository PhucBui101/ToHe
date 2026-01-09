using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    public string mainSceneName = "SampleScene";
    
    [Header("UI")]
    public Button backButton;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBack);
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene(mainSceneName);
    }
    
    public void OnPuzzleCompleted()
    {
        Debug.Log("Hoàn thành ghép hình!");
    }
}