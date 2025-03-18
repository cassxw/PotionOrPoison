using UnityEngine;
using UnityEngine.EventSystems;

// Handles drag and drop functionality for an arbitrary GameObject.
// Implements various interfaces from UnityEngine.EventSystems to manage drag events.
public class CloneDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Canvas canvas; // Reference to the canvas (useful for UI elements).
    private RectTransform rectTransform; // Reference to the UI's RectTransform.
    private CanvasGroup canvasGroup; // Controls raycast and opacity during drag.

    private Vector3 originalPosition;
    private GameObject cloneInstance;

    public bool isOriginalInstance = false; // If this is the original, immovable instance.
    public bool isGlassBottle = false; // To determine if this is a GlassBottle

    public CursorController cursorController;
    private bool isDragging = false;

    // Initialises necessary components and references.
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();  
        canvas = GetComponentInParent<Canvas>();
        originalPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOriginalInstance || (!isDragging && !isOriginalInstance))
        {
            cursorController.ChangeCursor("hover");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOriginalInstance || (!isDragging && !isOriginalInstance))
        {
            cursorController.ChangeCursor("neutral");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOriginalInstance)
        {
            CreateAndDragClone(eventData);
        }
        else
        {
            StartDraggingExistingItem();
        }
    }

    private void CreateAndDragClone(PointerEventData eventData)
    {
        // Determine the parent based on the type (GlassBottle or Ingredient)
        GameObject parent = isGlassBottle ? GameObject.Find("CloneGlassParent") : GameObject.Find("CloneIngredientParent");
        
        // Instantiate a clone of the current game objec
        cloneInstance = Instantiate(gameObject, transform.position, Quaternion.identity, canvas.transform);
        cloneInstance.transform.SetParent(parent.transform, false);

        // Set the clone's position to match the original
        cloneInstance.GetComponent<RectTransform>().position = rectTransform.position;

        // If not a glass bottle, scale the size of the clone down by 50% of the original instance
        if (!isGlassBottle)
        {
            cloneInstance.GetComponent<RectTransform>().localScale = rectTransform.localScale * 0.6f;
        }

        // Set up the CloneDragAndDrop script on the clone
        CloneDragAndDrop cloneDragAndDrop = cloneInstance.GetComponent<CloneDragAndDrop>();
        cloneDragAndDrop.isOriginalInstance = false;
        cloneDragAndDrop.isGlassBottle = isGlassBottle;
        cloneDragAndDrop.isDragging = true;

        // Begin dragging the clone, instead of the original
        eventData.pointerDrag = cloneInstance;
        cloneDragAndDrop.OnBeginDrag(eventData);
    }

    private void StartDraggingExistingItem()
    {
        canvasGroup.blocksRaycasts = false;

        if (isGlassBottle)
        {
            GlassBottle glassBottle = GetComponent<GlassBottle>();
            if (glassBottle != null && glassBottle.CurrentSlot != null)
            {
                glassBottle.CurrentSlot.ClearBottle();
            }
        }
        else
        {
            Ingredient ingredient = GetComponent<Ingredient>();
            if (ingredient != null && ingredient.CurrentSlot != null)
            {
                ingredient.CurrentSlot.ClearIngredient();
            }
        }
    }

    // Handles the continuous dragging of the Ingredient.
    // Param: eventData - Data associated with the pointer event.
    public void OnDrag(PointerEventData eventData)
    {
        if (!isOriginalInstance)
        {
            isDragging = true;
            cursorController.ChangeCursor("grab");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    // Handles the end of a drag operation.
    // Param: eventData - Data associated with the pointer event.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOriginalInstance)
        {
            isDragging = false;
            cursorController.ChangeCursor("neutral");

            canvasGroup.blocksRaycasts = true;

            bool droppedOnSlot = isGlassBottle ? IsDroppedOnGlassBottleSlot(gameObject) : IsDroppedOnIngredientSlot(gameObject);
            Debug.Log($"Dropped on slot: {droppedOnSlot}");

            if (!droppedOnSlot)
            {
                Destroy(gameObject);
            }
        }
    }

    private bool IsDroppedOnIngredientSlot(GameObject obj)
    {
        Vector2 dropPosition = obj.transform.position;
        IngredientSlot[] slots = FindObjectsByType<IngredientSlot>(FindObjectsSortMode.None);

        foreach (IngredientSlot slot in slots)
        {
            CircleCollider2D slotCollider = slot.GetComponent<CircleCollider2D>();
            
            if (slotCollider != null && slotCollider.OverlapPoint(dropPosition))
            {
                Ingredient ingredient = obj.GetComponent<Ingredient>();
                if (ingredient != null && slot.CurrentIngredient == null && !slot.isCrafting)
                {
                    slot.SetIngredient(ingredient);
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsDroppedOnGlassBottleSlot(GameObject obj)
    {
        Vector2 dropPosition = obj.transform.position;
        GlassBottleSlot slot = FindAnyObjectByType<GlassBottleSlot>();

        if (slot != null)
        {
            BoxCollider2D slotCollider = slot.GetComponent<BoxCollider2D>();
            
            if (slotCollider != null && slotCollider.OverlapPoint(dropPosition))
            {
                GlassBottle glassBottle = obj.GetComponent<GlassBottle>();
                if (glassBottle != null && slot.CurrentBottle == null && !slot.isCrafting)
                {
                    slot.SetBottle(glassBottle);
                    return true;
                }
            }
        }

        return false;
    }
}