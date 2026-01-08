using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    public Color hoverGlowColor = Color.yellow; // Public glow color
    public float scaleAmount = 1.1f;            // How much the button grows
    public float animationSpeed = 10f;

    private Color originalColor;
    private Vector3 originalScale;
    private Image buttonImage;
    private Vector3 targetScale;
    private Color targetColor;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
        originalScale = transform.localScale;

        targetScale = originalScale;
        targetColor = originalColor;
    }

    void Update()
    {
        // Smoothly transition scale and color
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        buttonImage.color = Color.Lerp(buttonImage.color, targetColor, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleAmount;
        targetColor = hoverGlowColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetColor = originalColor;
    }

    // Reset when the button is disabled (minigame ends)
    void OnDisable()
    {
        transform.localScale = originalScale;
        if (buttonImage != null) buttonImage.color = originalColor;
    }
}