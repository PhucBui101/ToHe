using UnityEngine;
using UnityEngine.UI;

public class MenuScroller : MonoBehaviour
{
    public RawImage rawImage;
    public float xSpeed = 0.05f;
    public float ySpeed = 0.02f;

    void Update()
    {
        // Moves the entire texture coordinates endlessly
        Rect uv = rawImage.uvRect;
        uv.x += xSpeed * Time.deltaTime;
        uv.y += ySpeed * Time.deltaTime;
        rawImage.uvRect = uv;
    }
}