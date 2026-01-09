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
                isInteracting = true;
                interactText.gameObject.SetActive(true);

                if (interactImage != null)
                    interactImage.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    SceneManager.LoadScene(puzzleScene);
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
}
