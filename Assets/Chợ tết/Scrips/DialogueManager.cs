using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Panels")]
    public GameObject uiPanel;
    public GameObject nextLineButtonObj;
    public Image scrollBackground;

    [Header("Scroll Sprites")]
    public Sprite talkSprite;
    public Sprite choiceSprite;

    [Header("Text Displays")]
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI contentDisplay;
    public float typeSpeed = 0.03f;

    [Header("Minigame Elements")]
    public GameObject choicesParent;
    public TextMeshProUGUI[] choiceTexts; // Ensure this is assigned in Inspector
    public float choiceFadeSpeed = 2f;

    [Header("Shake Settings")]
    public float shakeMagnitude = 5f;
    public float shakeSpeed = 50f;
    private bool isShaking = false;
    private Vector3 originalTextPos;

    [Header("Player Reference")]
    public MonoBehaviour playerScript;

    private NPCConversation currentData;
    private NPC currentNPC;
    private int currentLineIndex;
    private bool isInMinigame;
    private bool isTyping = false;
    private string completeText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        originalTextPos = contentDisplay.transform.localPosition;
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    public void StartDialogue(NPCConversation data, NPC npc)
    {
        currentData = data;
        currentNPC = npc;
        currentLineIndex = 0;
        isInMinigame = false;
        isShaking = false;

        contentDisplay.transform.localPosition = originalTextPos;

        StopAllCoroutines();
        StartCoroutine(AnimateUI(true));

        // EXIT BUG FIX: If player has won, show only line 0 and force exit index
        if (currentNPC.hasWon)
        {
            nameDisplay.text = currentData.npcName;
            completeText = currentData.talkLines[0];
            StartCoroutine(TypeText(completeText));

            // Set index to 999 so the NEXT click triggers CloseDialogue()
            currentLineIndex = 999;
        }
        else
        {
            UpdateUI();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerScript != null) playerScript.enabled = false;
    }

    public void OnCanvasClicked()
    {
        if (isInMinigame) return;

        if (isTyping)
        {
            StopCoroutine("TypeText");
            contentDisplay.text = completeText;
            isTyping = false;
            return;
        }

        isShaking = false;
        contentDisplay.transform.localPosition = originalTextPos;

        // GUARANTEED EXIT: Close immediately if at exit index
        if (currentLineIndex >= 999)
        {
            CloseDialogue();
            return;
        }

        currentLineIndex++;

        if (currentLineIndex < currentData.talkLines.Length) UpdateUI();
        else if (currentLineIndex == currentData.talkLines.Length) SetupMinigame();
        else CloseDialogue();
    }

    private void UpdateUI()
    {
        nameDisplay.text = currentData.npcName;
        completeText = currentData.talkLines[currentLineIndex];
        StopCoroutine("TypeText");
        StartCoroutine(TypeText(completeText));
    }

    IEnumerator TypeText(string text)
    {
        if (text.Contains("!!"))
        {
            isShaking = true;
            StartCoroutine(ApplyPhysicalShake());
        }
        else
        {
            isShaking = false;
            contentDisplay.transform.localPosition = originalTextPos;
        }

        isTyping = true;
        contentDisplay.text = "";
        foreach (char c in text.ToCharArray())
        {
            contentDisplay.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    IEnumerator ApplyPhysicalShake()
    {
        while (isShaking)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            contentDisplay.transform.localPosition = originalTextPos + new Vector3(offsetX, offsetY, 0);
            yield return new WaitForSeconds(1f / shakeSpeed);
        }
        contentDisplay.transform.localPosition = originalTextPos;
    }

    private void SetupMinigame()
    {
        isInMinigame = true;
        scrollBackground.sprite = choiceSprite;
        if (nextLineButtonObj != null) nextLineButtonObj.SetActive(false);

        // This loop uses the class-level choiceTexts array
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < currentData.answers.Length) choiceTexts[i].text = currentData.answers[i];
        }

        completeText = currentData.question;
        StartCoroutine(TypeTextAndFadeChoices(completeText));
    }

    IEnumerator TypeTextAndFadeChoices(string text)
    {
        yield return StartCoroutine(TypeText(text));
        choicesParent.SetActive(true);
        CanvasGroup cg = choicesParent.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * choiceFadeSpeed;
                cg.alpha = t;
                yield return null;
            }
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }

    public void OnChoiceSelected(int index)
    {
        // 1. Hide choices immediately
        CanvasGroup cg = choicesParent.GetComponent<CanvasGroup>();
        if (cg != null) { cg.alpha = 0; cg.interactable = false; cg.blocksRaycasts = false; }
        choicesParent.SetActive(false);

        isInMinigame = false;
        scrollBackground.sprite = talkSprite;
        if (nextLineButtonObj != null) nextLineButtonObj.SetActive(true);

        // 2. CRITICAL: Stop the Question typewriter before starting the Win typewriter
        StopAllCoroutines();
        contentDisplay.text = ""; // Force clear

        if (index == currentData.correctIndex)
        {
            currentNPC.hasWon = true;
            StartCoroutine(ShowWinSequence());
        }
        else
        {
            StartCoroutine(TypeText(currentData.loseLine));
            currentLineIndex = 999;
        }
    }

    IEnumerator ShowWinSequence()
    {
        // 1. Clear text and show initial Win Line (e.g., "Mày giỏi đấy cụ ạ...")
        contentDisplay.text = "";
        yield return StartCoroutine(TypeText(currentData.winLine));

        // 2. Wait for player to click before showing reward info
        while (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();

        // 3. Clear screen again for the Post-Win Reward Line
        contentDisplay.text = "";

        if (!string.IsNullOrEmpty(currentData.postWinLine))
        {
            // TRIGGER INVENTORY: Add item if one is assigned in the Inspector
            if (currentData.rewardItem != null && InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(currentData.rewardItem);
            }

            yield return StartCoroutine(TypeText(currentData.postWinLine));
        }

        // 4. Set exit flag so next click closes the box
        currentLineIndex = 999;
    }

    public void CloseDialogue()
    {
        isShaking = false;
        contentDisplay.transform.localPosition = originalTextPos;
        StopAllCoroutines();
        StartCoroutine(AnimateUI(false));
        if (playerScript != null) playerScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator AnimateUI(bool fadeIn)
    {
        if (fadeIn) uiPanel.SetActive(true);
        CanvasGroup cg = uiPanel.GetComponent<CanvasGroup>();
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            uiPanel.transform.localScale = Vector3.Lerp(fadeIn ? Vector3.one * 0.7f : Vector3.one, fadeIn ? Vector3.one : Vector3.one * 0.7f, t);
            if (cg != null) cg.alpha = Mathf.Lerp(fadeIn ? 0 : 1, fadeIn ? 1 : 0, t);
            yield return null;
        }
        if (!fadeIn) uiPanel.SetActive(false);
    }
}