using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    public string loadingSceneName = "LoadingScene"; // Your loading scene name

    // Assign to PlayButton OnClick
    public void PlayGame()
    {
        // Directs players to the loading scene
        SceneManager.LoadScene(loadingSceneName);
    }

    // Assign to ExitButton OnClick
    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit(); // Works in build
    }
}