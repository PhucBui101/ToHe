using UnityEngine;

public class NPCAppearance : MonoBehaviour
{
    void Start()
    {
        // Check the value we saved in the puzzle scene
        int status = PlayerPrefs.GetInt("PuzzleFinished", 0);

        if (status == 1)
        {
            // Puzzle is done, show the NPC
            gameObject.SetActive(true);
        }
        else
        {
            // Puzzle not done, hide the NPC
            gameObject.SetActive(false);
        }
    }
}