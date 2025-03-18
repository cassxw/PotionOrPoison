using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PotionType
{
    // Failed Potion
    Muck,

    // Level 1
    BewitchedLove,
    EvasiveShapeshifting,
    Thoughtweaving,

    // Level 2
    SpectralVision, 
    Tranquillity,
    VeiledShadows,
    Lightstep,

    // Level 3
    SwiftStrength,
    PurifyingHealing,
    LimbRegrowth,
    MendingWounds
}

public class Potion : MonoBehaviour
{
    public string Name;
    public PotionType type;
    public Ingredient Ingredient1;
    public Ingredient Ingredient2;
    public Image image;

    public bool isPoisoned = false;

    [Header("All Potion Sprites")]
    [SerializeField] private Sprite muckSprite;
    [SerializeField] private Sprite bewitchedLoveSprite;
    [SerializeField] private Sprite evasiveShapeshiftingSprite;
    [SerializeField] private Sprite thoughtweavingSprite;
    [SerializeField] private Sprite spectralVisionSprite;
    [SerializeField] private Sprite tranquillitySprite;
    [SerializeField] private Sprite veiledShadowsSprite;
    [SerializeField] private Sprite lightstepSprite;
    [SerializeField] private Sprite swiftStrengthSprite;
    [SerializeField] private Sprite purifyingHealingSprite;
    [SerializeField] private Sprite limbRegrowthSprite;
    [SerializeField] private Sprite mendingWoundsSprite;


    [Header("Poison Glow Effect")]
    [SerializeField] private Image glowImage;
    [SerializeField] private Color glowColor = new Color(0, 1, 0, 0.5f); // Semi-transparent green
    [SerializeField] private float glowFadeDuration = 3f; // Duration for the glow fade-out

    [Header("Ghost Animation")]
    [SerializeField] public Image ghostImage;
    [SerializeField] private float ghostRiseDistance = 100f;
    [SerializeField] private float ghostAnimationDuration = 3f;
    [SerializeField] private float ghostScale = 0.5f;

    private Coroutine currentPoisonAnimation;
    private Coroutine glowAnimationCoroutine;
    private Coroutine ghostAnimationCoroutine;

    private GameObject potionMaking;

    public PoisonDropper poisonDropper;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("Image component not found on Potion object.");
            }
        }

        potionMaking = GameObject.Find("PotionMaking");
        
        poisonDropper = GameObject.FindObjectOfType<PoisonDropper>();
        if (poisonDropper == null)
        {
            Debug.LogError("PoisonDropper not found in the scene.");
        }

        SetupGhostImage();
        SetupGlowImage();

        // Set the proper layering
        ghostImage.transform.SetSiblingIndex(transform.childCount - 2);  // Ghost above the potion
        glowImage.transform.SetSiblingIndex(transform.childCount - 1);   // Glow on top

        // Initially hide the glow and ghost
        glowImage.enabled = false;
        ghostImage.enabled = false;
    }

    private void Update()
    {
        // Update ghost and glow positions to follow the potion
        if (ghostImage != null)
        {
            ghostImage.rectTransform.position = transform.position;
        }
        if (glowImage != null)
        {
            glowImage.rectTransform.position = transform.position;
        }
    }

    private void SetupGlowImage()
    {
        if (glowImage == null)
        {
            GameObject glowObject = new GameObject("PoisonGlow");
            glowObject.transform.SetParent(potionMaking.transform, false);
            glowImage = glowObject.AddComponent<Image>();
            glowImage.sprite = CreateGlowSprite();
            glowImage.color = glowColor;

            // Set the RectTransform to match the potion's size and scale it up
            // glowImage.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta * glowScale;
            glowImage.rectTransform.position = transform.position;
        }
    }

    private Sprite CreateGlowSprite()
    {
        // Create a simple white circular sprite for the glow
        Texture2D tex = new Texture2D(500, 500);
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(tex.width / 2, tex.height / 2));
                float alpha = Mathf.Max(0, 1 - (distance / (tex.width / 2)));
                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    private void SetupGhostImage()
    {
        if (ghostImage == null)
        {
            GameObject ghostObject = new GameObject("PoisonGhost");
            ghostObject.transform.SetParent(potionMaking.transform, false);
            ghostImage = ghostObject.AddComponent<Image>();

            // Get the ghost sprite from PoisonDropper
            if (poisonDropper != null)
            {
                Sprite ghostSprite = poisonDropper.GetGhostSprite();
                if (ghostSprite != null)
                {
                    ghostImage.sprite = ghostSprite;
                }
                else
                {
                    Debug.LogError("Ghost sprite not set in PoisonDropper.");
                }
            }

            // Set to native size and then scale
            if (ghostImage.sprite != null)
            {
                ghostImage.SetNativeSize();
                ghostImage.rectTransform.localScale = Vector3.one * ghostScale;
            }

            // Position the ghost image
            ghostImage.rectTransform.position = transform.position;
        }
    }

    public void SetType(PotionType newType)
    {
        type = newType;
        Name = type.ToString();
        UpdateAppearance();
    }

    public void SetPoisoned(bool poisoned)
    {
        isPoisoned = poisoned;
        
        // If poisoned and not currently animating, trigger poison effect
        if (isPoisoned && currentPoisonAnimation == null)
        {
            // Recreate the glow and ghost objects if they were destroyed
            SetupGlowImage();
            SetupGhostImage();

            UpdateAppearance();
            poisonDropper.poisonSound.Play();
            currentPoisonAnimation = StartCoroutine(PoisonEffect());
        }
        else if (!isPoisoned && currentPoisonAnimation != null)
        {
            // If animation is running and potion is no longer poisoned, stop the animation
            StopPoisonAnimation();
        }
    }

    private IEnumerator PoisonEffect()
    {
        // Start both glow and ghost effects
        glowAnimationCoroutine = StartCoroutine(FadeOutGlow());
        ghostAnimationCoroutine = StartCoroutine(AnimateGhost());
        yield return new WaitUntil(() => glowAnimationCoroutine == null && ghostAnimationCoroutine == null);

        // Reset the current animation coroutine
        currentPoisonAnimation = null;
    }

    private IEnumerator FadeOutGlow()
    {
        glowImage.enabled = true;
        float elapsedTime = 0f;
        Color initialColor = glowColor;

        while (elapsedTime < glowFadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate new alpha value
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / glowFadeDuration);

            // Set the new color with the fading alpha
            glowImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null; // Wait until the next frame
        }

        // Ensure the glow is fully transparent and disable it
        glowImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        glowImage.enabled = false; // Hide the glow after it fades out

        yield return null; // Wait until the next frame

        // Destroy the glow image after the animation
        DestroyGlowImage();
        glowAnimationCoroutine = null;
    }

    private IEnumerator AnimateGhost()
    {
        ghostImage.enabled = true;
        ghostImage.color = Color.white;
        Vector2 startPosition = ghostImage.rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(0, ghostRiseDistance);

        float elapsedTime = 0f;
        while (elapsedTime < ghostAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / ghostAnimationDuration;

            // Move the ghost upwards
            ghostImage.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // Fade out the ghost
            ghostImage.color = new Color(1, 1, 1, 1 - t);

            yield return null;
        }

        // Ensure the ghost is fully transparent at the end
        ghostImage.color = new Color(1, 1, 1, 0);
        ghostImage.rectTransform.anchoredPosition = startPosition; // Reset position for next animation
        ghostImage.enabled = false;

        yield return null; // Wait until the next frame

        // Destroy the ghost image after the animation
        DestroyGhostImage();
        ghostAnimationCoroutine = null;
    }

    private void DestroyGlowImage()
    {
        if (glowImage != null)
        {
            Destroy(glowImage.gameObject);
            glowImage = null;
        }
    }

    private void DestroyGhostImage()
    {
        if (ghostImage != null)
        {
            Destroy(ghostImage.gameObject);
            ghostImage = null;
        }
    }

    private void StopPoisonAnimation()
    {
        if (currentPoisonAnimation != null)
        {
            StopCoroutine(currentPoisonAnimation);
            currentPoisonAnimation = null;
        }

        if (glowAnimationCoroutine != null)
        {
            StopCoroutine(glowAnimationCoroutine);
            glowAnimationCoroutine = null;
        }

        if (ghostAnimationCoroutine != null)
        {
            StopCoroutine(ghostAnimationCoroutine);
            ghostAnimationCoroutine = null;
        }

        DestroyGlowImage();
        DestroyGhostImage();
    }

    private void OnDisable()
    {
        StopPoisonAnimation();
    }

    private void UpdateAppearance()
    {
        Sprite spriteToUse = null;

        switch (type)
        {
            case PotionType.Muck:
                spriteToUse = muckSprite;
                break;
            case PotionType.BewitchedLove:
                spriteToUse = bewitchedLoveSprite;
                break;
            case PotionType.EvasiveShapeshifting:
                spriteToUse = evasiveShapeshiftingSprite;
                break;
            case PotionType.Thoughtweaving:
                spriteToUse = thoughtweavingSprite;
                break;
            case PotionType.SpectralVision:
                spriteToUse = spectralVisionSprite;
                break;
            case PotionType.Tranquillity:
                spriteToUse = tranquillitySprite;
                break;
            case PotionType.VeiledShadows:
                spriteToUse = veiledShadowsSprite;
                break;
            case PotionType.Lightstep:
                spriteToUse = lightstepSprite;
                break;
            case PotionType.SwiftStrength:
                spriteToUse = swiftStrengthSprite;
                break;
            case PotionType.PurifyingHealing:
                spriteToUse = purifyingHealingSprite;
                break;
            case PotionType.LimbRegrowth:
                spriteToUse = limbRegrowthSprite;
                break;
            case PotionType.MendingWounds:
                spriteToUse = mendingWoundsSprite;
                break;
            default:
                spriteToUse = muckSprite;
                break;
        }

        if (spriteToUse != null)
        {
            image.sprite = spriteToUse;
        }
        else
        {
            Debug.LogError($"Sprite not assigned for potion type: {type}");
        }

        if (image.sprite == null)
        {
            Debug.LogError($"Failed to set sprite for potion type: {type}");
        }

        // Update glow visibility based on poison status
        glowImage.enabled = isPoisoned;
        ghostImage.enabled = isPoisoned;

        // Update ghost and glow positions to follow the potion
        if (ghostImage != null)
        {
            ghostImage.rectTransform.position = transform.position;
        }
        if (glowImage != null)
        {
            glowImage.rectTransform.position = transform.position;
        }
    }
}