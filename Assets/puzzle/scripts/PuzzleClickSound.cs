using UnityEngine;

public class PuzzleClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
