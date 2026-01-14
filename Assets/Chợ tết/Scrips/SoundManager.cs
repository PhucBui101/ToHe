using UnityEngine;
using DG.Tweening; // Required for the DOFade effect
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource uiAudioSource;  // For button pops/inventory
    [SerializeField] private AudioSource bgmAudioSource; // For the fading music

    [Header("UI Audio Clips")]
    [SerializeField] private AudioClip buttonClickPop;
    [SerializeField] private AudioClip inventoryOpenSFX;

    [Header("Music Settings")]
    [SerializeField] private float musicTargetVolume = 0.5f;
    [SerializeField] private float fadeDuration = 2.0f;

    private void Awake()
    {
        // Singleton pattern to keep audio playing across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start the music with a fade-in effect
        if (bgmAudioSource != null)
        {
            PlayBGMWithFade();
        }
    }

    /// <summary>
    /// Smoothly fades in the background music using DOTween.
    /// </summary>
    public void PlayBGMWithFade()
    {
        if (bgmAudioSource == null) return;

        bgmAudioSource.volume = 0; // Start at silent
        bgmAudioSource.Play();
        bgmAudioSource.DOFade(musicTargetVolume, fadeDuration).SetUpdate(true); // Fades even if game is paused
    }

    /// <summary>
    /// Plays a single pop sound for button clicks.
    /// </summary>
    public void PlayClickSound()
    {
        if (uiAudioSource != null && buttonClickPop != null)
        {
            uiAudioSource.PlayOneShot(buttonClickPop);
        }
    }

    /// <summary>
    /// Plays a sound when opening/closing the inventory.
    /// </summary>
    public void PlayInventorySound()
    {
        if (uiAudioSource != null && inventoryOpenSFX != null)
        {
            uiAudioSource.PlayOneShot(inventoryOpenSFX);
        }
    }
}