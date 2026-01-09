using UnityEngine;

public class ItemGlow : MonoBehaviour
{
    [Header("Cài đặt Glow")]
    public Color glowColor = Color.red;
    public float glowIntensity = 2f;
    public float fadeSpeed = 5f;

    private Material material;
    private bool isMouseOver = false;
    private float currentIntensity = 0f;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.black);
        }
    }

    void Update()
    {
        if (material != null)
        {
            float targetIntensity = isMouseOver ? glowIntensity : 0f;
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * fadeSpeed);
            
            Color finalColor = glowColor * currentIntensity;
            material.SetColor("_EmissionColor", finalColor);
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }
}
