using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Handles the behavior of the OvenLever, allowing it to be dragged and rotated.
// The lever follows the user's cursor during dragging and springs back to its initial position when released.
// The lever can toggle its ability to turn based on a valid potion-making configuration.
public class OvenLever : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Lever Settings")]
    public float maxDragAngle = -90f; // Maximum angle the lever can be dragged down to (negative for downwards movement).
    public float springBackSpeed = 5f; // Speed at which the lever springs back to its original position after release.
    public float requiredDragAngle = 80f; // The angle that must be reached for the pull to be registered (negative for downwards movement).

    private bool isDragging = false; // Tracks if the player is currently dragging the lever.
    private bool hasRegistered = false; // Whether the lever drag has been registered, to prevent multiple triggers with a single pull.
    private Quaternion initialRotation; // Initial rotation of the lever.
    private Transform leverTransform; // The lever's transform component for transformation.

    public delegate void LeverPulledEvent();
    public event LeverPulledEvent OnLeverPulled;

    public CursorController cursorController;

    //---------------------------------------------------------------------------------------------
    // Called when the script is initialised.
    // Stores the initial rotation and transform of the lever.
    private void Start()
    {
        // Store the initial rotation and transform of the lever.
        leverTransform = transform;
        initialRotation = leverTransform.localRotation;
    }
    //---------------------------------------------------------------------------------------------

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            cursorController.ChangeCursor("hover");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            cursorController.ChangeCursor("neutral");
        }
    }

    // Called when the player starts dragging the lever.
    // Sets dragging state and resets registration flag.
    // Param: eventData - Data associated with the pointer event.
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        hasRegistered = false;
    }

    // Called while the player is dragging the lever.
    // Rotates the lever based on the current mouse position.
    // Param: eventData - Data associated with the pointer event.
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            cursorController.ChangeCursor("grab");

            // Get the current mouse position.
            Vector2 currentMousePosition = eventData.position;
            
            // Calculate the distance dragged from the lever's original position.
            float dragDistance = currentMousePosition.y - leverTransform.position.y;

            // Clamp the drag distance to prevent exceeding maximum and minimum limits.
            dragDistance = Mathf.Clamp(dragDistance, maxDragAngle, -10.0f);

            // Calculate the angle based on the drag distance (negative for downwards movement)
            float angle = -dragDistance;

            // Apply the rotation to the lever.
            leverTransform.localRotation = Quaternion.Euler(0, 0, angle);

            // Check if the lever has been dragged down fully to register the pull.
            if (angle >= requiredDragAngle && !hasRegistered)
            { 
                hasRegistered = true; // Register the pull.
                RegisterLeverPulled(); // Notify other systems.
            }
        }
    }

    // Called when the player releases the lever.
    // Initiates the spring-back effect.
    // Param: eventData - Data associated with the pointer event.
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false; // Set dragging state to false.
        cursorController.ChangeCursor("neutral");
        StartCoroutine(SpringBackToInitialPosition()); // Start the spring-back coroutine.
    }

    // Coroutine to smoothly rotate the lever back to its initial position.
    public IEnumerator SpringBackToInitialPosition()
    {
        while (Quaternion.Angle(leverTransform.localRotation, initialRotation) > 0.1f)
        {
            // Smoothly rotate the lever back to the initial position.
            leverTransform.localRotation = Quaternion.Lerp(leverTransform.localRotation, initialRotation, Time.deltaTime * springBackSpeed);
            yield return null;  // Wait for the next frame.
        }

        // Ensure set exactly to the initial rotation at the end.
        leverTransform.localRotation = initialRotation;
    }

    // Notifies other scripts or systems that the lever has been fully pulled.
    // This method will be used to initiate the potion-checking process.
    private void RegisterLeverPulled()
    {
        Debug.Log("Lever fully pulled, starting potion making process...");
        OnLeverPulled?.Invoke();
    }
}
