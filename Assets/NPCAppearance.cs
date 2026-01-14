using UnityEngine;

public class NPCAppearance : MonoBehaviour
{
    void Start()
    {
        // Check if the PuzzleWinManager set the flag to 1
        int status = PlayerPrefs.GetInt("PuzzleFinished", 0);

        if (status == 1)
        {
            // Show NPC only if puzzle was won
            gameObject.SetActive(true);
        }
        else
        {
            // Keep NPC hidden otherwise
            gameObject.SetActive(false);
        }
    }
}