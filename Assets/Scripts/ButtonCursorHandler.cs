using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCursorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CursorController cursorController;
    public bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        cursorController.ChangeCursor("hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        cursorController.ChangeCursor("neutral");
    }
}