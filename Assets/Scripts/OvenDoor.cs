using UnityEngine;
using System.Collections;

public class OvenDoor : MonoBehaviour
{
    public float closedPosition = -16f; 
    public float openPosition = 130f; 
    public float doorMovementDuration = 0.5f;

    private RectTransform doorRectTransform;
    
    public AudioSource audioSource;
    public AudioClip ovenDoorMove;
    // Closing => first 1.5seconds
    // Opening => from 3seconds to end

    private void Awake()
    {
        doorRectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator MoveDoor(bool close)
    {
        // Play the appropriate part of the sound depending on whether opening or closing
        if (close)
        {
            PlayClosingSound();
        }
        else
        {
            PlayOpeningSound();
        }

        float startPosition = close ? openPosition : closedPosition;
        float endPosition = close ? closedPosition : openPosition;
        float elapsedTime = 0f;

        // Move the door over the duration of doorMovementDuration
        while (elapsedTime < doorMovementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / doorMovementDuration;
            float currentPosition = Mathf.Lerp(startPosition, endPosition, t);
            doorRectTransform.anchoredPosition = new Vector2(doorRectTransform.anchoredPosition.x, currentPosition);
            yield return null;
        }

        doorRectTransform.anchoredPosition = new Vector2(doorRectTransform.anchoredPosition.x, endPosition);

        if (!close)
        {
            // Hide the door when it's fully open
            gameObject.SetActive(false);
        }
    }

    private void PlayOpeningSound()
    {
        // Play from the start and stop after 1.5 seconds
        audioSource.clip = ovenDoorMove;
        audioSource.time = 0f; // Start at the beginning
        audioSource.Play();

        StartCoroutine(StopSoundAfterDuration(1.5f)); // Stop after 1.5 seconds
    }

    private void PlayClosingSound()
    {
        // Start the sound from 3 seconds
        audioSource.clip = ovenDoorMove;
        audioSource.time = 3f; // Start at 3 seconds for closing
        audioSource.Play();

        // Let it play until the end
    }

    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}