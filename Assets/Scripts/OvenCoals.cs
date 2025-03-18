using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OvenCoals : MonoBehaviour
{
    public Image coalsBack; 
    public Image coalsImage;
    public Sprite normalCoalsSprite;
    public Sprite firedCoalsSprite;
    public float heatingDuration = 1.5f;
    public float coolingDuration = 1.5f;

    private void Start()
    {
        // Ensure the coals start with the normal sprite
        coalsImage.sprite = normalCoalsSprite;
    }

    public IEnumerator HeatUpCoals()
    {
        yield return StartCoroutine(TransitionSprite(normalCoalsSprite, firedCoalsSprite, heatingDuration));
        coalsBack.sprite = firedCoalsSprite;
    }

    public IEnumerator CoolDownCoals()
    {
        yield return StartCoroutine(TransitionSprite(firedCoalsSprite, normalCoalsSprite, coolingDuration));
        coalsBack.sprite = normalCoalsSprite;
    }

    private IEnumerator TransitionSprite(Sprite fromSprite, Sprite toSprite, float duration)
    {
        float elapsedTime = 0f;
        coalsImage.sprite = toSprite;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            coalsImage.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, t);
            yield return null;
        }

        coalsImage.color = Color.white;
    }
}