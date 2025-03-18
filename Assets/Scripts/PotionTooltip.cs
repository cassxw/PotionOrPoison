using UnityEngine;
using UnityEngine.UI;

public class PotionTooltip : MonoBehaviour
{
    public Image background;
    public Text potionNameText;
    public float displayDuration = 3f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        gameObject.SetActive(false);
    }

    private string GetPotionName(PotionType potionType)
    {
        switch (potionType)
        {
            case PotionType.BewitchedLove:
                return "Potion of Bewitched Love";
            case PotionType.EvasiveShapeshifting:
                return "Potion of Evasive Shapeshifting";
            case PotionType.Thoughtweaving:
                return "Potion of Thoughtweaving";
            case PotionType.SpectralVision:
                return "Potion of Spectral Vision";
            case PotionType.Tranquillity:
                return "Potion of Tranquility";
            case PotionType.VeiledShadows:
                return "Potion of Veiled Shadows";
            case PotionType.Lightstep:
                return "Potion of Lightstep";
            case PotionType.SwiftStrength:
                return "Potion of Swift Strength";
            case PotionType.PurifyingHealing:
                return "Potion of Purifying Healing";
            case PotionType.LimbRegrowth:
                return "Potion of Limb Regrowth";
            case PotionType.MendingWounds:
                return "Potion of Mending Wounds";

            default:
                return "Muck!";
        }
    }


    public void ShowTooltip(PotionType potionType)
    {
        potionNameText.text = GetPotionName(potionType);
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        Invoke("StartFading", displayDuration);
    }

    private void StartFading()
    {
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}