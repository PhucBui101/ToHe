using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuzzleWinManager : MonoBehaviour
{
    public static PuzzleWinManager Instance;

    public int totalPieces;
    private int placedPieces = 0;

    public GameObject winImage;
    public string homeSceneName;
    public float winDelay = 2.5f;

    void Awake()
    {
        Instance = this;
        winImage.SetActive(false);
    }

    public void PiecePlaced()
    {
        placedPieces++;

        if (placedPieces >= totalPieces)
        {
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        // --- THE FLAG ---
        // Save the progress so NPCAppearance knows the puzzle is done
        PlayerPrefs.SetInt("PuzzleFinished", 1);
        PlayerPrefs.Save();

        winImage.SetActive(true);
        winImage.transform.localScale = Vector3.zero;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            winImage.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        yield return new WaitForSeconds(winDelay);
        SceneManager.LoadScene(homeSceneName);
    }
}