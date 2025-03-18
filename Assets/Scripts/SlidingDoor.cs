using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float moveDuration = 1f;
    private RectTransform rectTransform;
    private Vector2 openPosition;
    private Vector2 closedPosition;
    private bool isMoving = false;

    private AudioSource audioSource;
    public AudioClip moveUpSound;
    public AudioClip moveDownSound;

    void Start(){
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        closedPosition = rectTransform.anchoredPosition;
        openPosition=new Vector3(closedPosition.x, closedPosition.y+rectTransform.rect.height);
    }
    public void MoveUp(){
        if(!isMoving){
            audioSource.clip = moveUpSound;
            audioSource.Play();
            StartCoroutine(MoveDoor(closedPosition, openPosition));
        }
    }

    public void MoveDown(){
        if(!isMoving){
            audioSource.clip = moveDownSound;
            audioSource.Play();
            StartCoroutine(MoveDoor(openPosition, closedPosition));
        }
    }

    private System.Collections.IEnumerator MoveDoor(Vector2 from, Vector2 to){
        isMoving=true;
        float elapsedTime = 0f;

        while(elapsedTime<moveDuration){
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, elapsedTime/moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition=to;
        isMoving=false;
    }
}
