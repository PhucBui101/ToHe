using UnityEngine;

[System.Serializable]
public class NPCConversation
{
    public string npcName;
    [TextArea(2, 5)]
    public string[] talkLines; // For multiple-click dialogue

    [Header("Minigame Settings")]
    public string question;
    public string[] answers = new string[4];
    public int correctIndex; // 0 for A, 1 for B, etc.
    public string postWinLine;
    // Add this line inside your NPCConversation class
    public ItemData rewardItem;
    public string winLine = "Win.";
    public string loseLine = "might be lucky next time blud";
}

// NPC.cs
// NPC.cs
public class NPC : MonoBehaviour
{
    public NPCConversation data;
    public bool hasWon = false; // Tracks if the player won this NPC's game
    public void OnInteract()
    {
        if (DialogueManager.Instance != null)
        {
            // Pass 'this' NPC reference so the manager can update 'hasWon'
            DialogueManager.Instance.StartDialogue(data, this);
        }
    }
}