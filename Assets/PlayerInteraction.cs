using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    public TextMeshProUGUI interactText; // chữ có sẵn của bạn mình
    public GameObject interactImage;     // hình "Bấm E..."

    // 🔒 FIX CỨNG TÊN SCENE PUZZLE
    private string puzzleScene = "PuzzleScene";

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        if (interactImage != null)
            interactImage.SetActive(false);
    }

    void Update()
    {
        RaycastHit hit;
        bool isInteracting = false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactableLayer))
        {
            // 🟢 CỬA (GIỮ NGUYÊN LOGIC CŨ)
            TransitionScene door = hit.collider.GetComponent<TransitionScene>();
            if (door != null)
            {
                isInteracting = true;
                interactText.gameObject.SetActive(true);

                if (interactImage != null)
                    interactImage.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.EnterDoor();
                }
            }
            // 🟡 BÀN PUZZLE
            else
            {
                if(InventoryManager.Instance.IsCanMakeToHe())
                {
                    isInteracting = true;
                    interactText.gameObject.SetActive(true);

                    if (interactImage != null)
                        interactImage.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SceneManager.LoadScene(puzzleScene);
                    }
                }
                else
                {
                    Debug.Log("ko du do");
                }
                    

            }
        }

        if (!isInteracting)
        {
            interactText.gameObject.SetActive(false);
            if (interactImage != null)
                interactImage.SetActive(false);
        }
    }
    // Inside your Puzzle Game logic script
    public void OnPuzzleWon()
    {
        // Save the victory state
        PlayerPrefs.SetInt("PuzzleFinished", 1);
        PlayerPrefs.Save();

        // Go back to the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
