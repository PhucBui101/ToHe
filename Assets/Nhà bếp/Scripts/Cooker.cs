using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cái nồi nấu đồ
public class Cooker : MonoBehaviour
{
    private bool playerIsNear = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            if (gameManager != null)
            {
                // FIXED: Provided both arguments (bool, string)
                gameManager.ShowPrompt(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (gameManager != null)
            {
                // FIXED: Provided both arguments (bool, string) 
                // We use an empty string "" for the second argument when hiding
                gameManager.ShowPrompt(false);
            }
        }
    }
}