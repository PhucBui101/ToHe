using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cái nồi nấu đồ
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
                gameManager.ShowPrompt(true, "Nhấn Tab để mở túi & nấu đồ");
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
                gameManager.ShowPrompt(false);
            }
        }
    }
}
