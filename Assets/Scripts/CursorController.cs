using System.Collections;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool canChangeCursor = true;  // Flag to control when the cursor can be changed

    public Texture2D cursorNeutral;
    public Texture2D cursorHover;
    public Texture2D cursorGrab;

    // Public method to change the cursor
    public void ChangeCursor(string type)
    {
        if (canChangeCursor)
        {
            StartCoroutine(ChangeCursorWithDelay(type));
        }
    }

    public void ButtonClick()
    {
        StartCoroutine(ButtonClickWithDelay());
    }

    // Coroutine that handles the cursor change with a delay
    private IEnumerator ChangeCursorWithDelay(string type)
    {
        canChangeCursor = false; // Blocks further cursor changes during the delay

        switch (type)
        {
            case "neutral":
                Cursor.SetCursor(cursorNeutral, new Vector2(4f, 2f), CursorMode.ForceSoftware);
                break;
            case "hover":
                Cursor.SetCursor(cursorHover, new Vector2(20f, 28f), CursorMode.ForceSoftware);
                break;
            case "grab":
                Cursor.SetCursor(cursorGrab, new Vector2(40f, 44f), CursorMode.ForceSoftware);
                break;
        }

        // Wait for a short delay (e.g., 0.2 seconds) to make the cursor change clear
        yield return new WaitForSeconds(0.2f);

        canChangeCursor = true;  // Allow the cursor to change again
    }

    // Coroutine that handles the cursor change with a delay
    private IEnumerator ButtonClickWithDelay()
    {
        Cursor.SetCursor(cursorHover, new Vector2(20f, 28f), CursorMode.ForceSoftware);

        // Wait for a short delay (e.g., 0.2 seconds) to make the cursor change clear
        yield return new WaitForSeconds(0.2f);

        Cursor.SetCursor(cursorNeutral, new Vector2(4f, 2f), CursorMode.ForceSoftware);
    }
}
