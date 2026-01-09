using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform correctSlot;
    private RectTransform rect;
    private Vector2 startPos;
    private bool locked = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (locked) return;
        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked) return;

        if (Vector2.Distance(rect.position, correctSlot.position) < 100f) // sai số dễ
        {
            rect.position = correctSlot.position;
            locked = true;

            PuzzleWinManager.Instance.PiecePlaced();
        }
        else
        {
            rect.anchoredPosition = startPos;
        }
    }
}
