using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FreeDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image _image;
    private PotionController potionController;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider;
    
    private Vector3 originalPosition;
    private Coroutine snapBackCoroutine;

    public bool isPoisonDropper;

    public CursorController cursorController;
    private bool isDragging = false;

    private void Start()
    {
        _image = GetComponent<Image>();
        potionController = FindAnyObjectByType<PotionController>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null && !gameObject.CompareTag("PoisonDropper"))
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();

            // Manually set the BoxCollider2D size and offset
            boxCollider.size = new Vector2(232f, 512f);
            boxCollider.offset = Vector2.zero;
        }

        // Check if Rigidbody2D exists, if not add it
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        }
        // Set it to Kinematic if you are manually controlling movement (dragging)
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        boxCollider.isTrigger = true;

        // Store the initial position for snap-back behavior
        originalPosition = transform.position;
    }

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

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        cursorController.ChangeCursor("grab");
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Stop snap-back if it's in progress
        if (snapBackCoroutine != null)
        {
            StopCoroutine(snapBackCoroutine);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        cursorController.ChangeCursor("neutral");

        Vector2 dropPosition = transform.position;

        if (isPoisonDropper)
        {   
            // Snap back the PoisonDropper object slowly
            snapBackCoroutine = StartCoroutine(SnapBackToOriginalPosition());
        }
        else
        {
            HandleDropAction(dropPosition);
        }
    }

    // Make sure this ethod name is exactly like this for 2D trigger collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPoisonDropper)
        {
            // Check if the object collided with is a potion
            Potion potion = other.GetComponent<Potion>();

            if (potion != null)
            {
                // Get the PoisonDropper component
                PoisonDropper dropper = GetComponent<PoisonDropper>();

                if (dropper != null)
                {
                    dropper.PoisonPotion(potion); // Trigger the poisoning of the potion
                }
            }
        }
    }

    private void HandleDropAction(Vector2 dropPosition)
    {
        // Check if on the PotionBin
        GameObject potionBin = GameObject.Find("PotionBin");
        BoxCollider2D potionBinCollider = potionBin.GetComponent<BoxCollider2D>();
        
        if (potionBinCollider != null && potionBinCollider.OverlapPoint(dropPosition))
        {
            AudioSource potionBinSound = potionBin.GetComponent<AudioSource>();
            potionBinSound.Play();
            Destroy(gameObject); // Destroy the object when dropped on PotionBin
            return;
        }
    
        // Check if on a Customer
        GameObject customerWindow = GameObject.Find("CustomerWindow");
        BoxCollider2D customerWindowCollider = customerWindow.GetComponent<BoxCollider2D>();
        
        if (customerWindowCollider != null && customerWindowCollider.OverlapPoint(dropPosition))
        {
            Potion potion = GetComponent<Potion>(); // Assuming you have a Potion component attached
            potionController.HandlePotionDelivery(potion); // Deliver the potion
            Destroy(gameObject); // Destroy the potion after delivery
            return;
        }
    }

    private IEnumerator SnapBackToOriginalPosition()
    {
        float duration = 0.5f; // Duration of the snap-back effect
        float elapsedTime = 0f;

        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Ensure exact final position
    }
}
