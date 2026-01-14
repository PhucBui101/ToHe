using UnityEngine;
using System.Collections;

public class PuzzleIntro : MonoBehaviour
{
    public GameObject howToPlayImage;
    public GameObject sampleImage;
    public GameObject puzzlePanel;

    public float sampleShowTime = 5f;

    private bool started = false;

    void Start()
    {
        howToPlayImage.SetActive(true);
        sampleImage.SetActive(false);
        puzzlePanel.SetActive(false);
    }

    void Update()
    {
        if (started) return;

        // Nhận cả E hoặc click chuột
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            started = true;
            howToPlayImage.SetActive(false);
            StartCoroutine(ShowSample());
        }
    }

    IEnumerator ShowSample()
    {
        sampleImage.SetActive(true);
        yield return new WaitForSeconds(sampleShowTime);
        sampleImage.SetActive(false);
        puzzlePanel.SetActive(true);
    }
}
