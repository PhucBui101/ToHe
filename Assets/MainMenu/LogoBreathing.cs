using UnityEngine;

public class LogoBreathing : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseMagnitude = 0.1f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Creates a smooth pulsing scale effect
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude;
        transform.localScale = initialScale * scale;
    }
}