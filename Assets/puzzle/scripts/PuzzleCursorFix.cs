using UnityEngine;

public class PuzzleCursorFix : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        // Áp dụng cursor khi vào Puzzle Scene
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnDestroy()
    {
        // Trả cursor về mặc định khi rời Puzzle Scene
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
