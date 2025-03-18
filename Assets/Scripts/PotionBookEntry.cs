using UnityEngine;
using UnityEngine.UI;

public class PotionBookEntry : MonoBehaviour
{
    [SerializeField] private Text potionNameText;
    [SerializeField] private Text potionDescriptionText;
    [SerializeField] private Image potionImage;
    [SerializeField] private Image blurOverlay;
    [SerializeField] private PotionType potionType;

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

    private void Start()
    {
        SetPotionInfo(potionType);

        // Set the native size of the image
        potionImage.SetNativeSize();

        // Scale the potion GameObject equally on all axes (x, y, z) by 0.15
        potionImage.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    }

    public void SetPotionInfo(PotionType type)
    {
        potionType = type;
        potionNameText.text = GetPotionName(type);
        potionDescriptionText.text = GetPotionDescription(type);
        potionImage.sprite = GetPotionSprite(type);
    }

    public void UpdateKnownState()
    {
        bool isKnown = GameSystem.Instance.IsKnownPotion(potionType);
        gameObject.SetActive(true); // Always set active, we'll use blur for unknown

        potionNameText.gameObject.SetActive(isKnown); //isKnown
        potionDescriptionText.gameObject.SetActive(isKnown); //isKnown
        potionImage.gameObject.SetActive(isKnown); //isKnown

        blurOverlay.gameObject.SetActive(!isKnown); //!isKnown
    }

    public string GetPotionName(PotionType potionType)
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
                return "No name available.";
        }
    }

    private string GetPotionDescription(PotionType potionType)
    {
        switch (potionType)
        {
            case PotionType.BewitchedLove:
                return "Causes the drinker to fall deeply in love with the first person they see upon consumption.";
            case PotionType.EvasiveShapeshifting:
                return "Grants the drinker temporary shapeshifting ability with enhanced agility, to escape or hide from enemies by changing form and quickly fleeing.";
            case PotionType.Thoughtweaving:
                return "Grants the drinker temporary telepathic abilities, allowing them to delve into the thoughts of others.";
            case PotionType.SpectralVision:
                return "Allows the drinker to see and communicate with spirits while gaining heightened wisdom, lifting the veil and grants clarity in dealing with the other side.";
            case PotionType.Tranquillity:
                return "Calms the mind and restores mental clarity of the drinker, curing insanity or delirium.";
            case PotionType.VeiledShadows:
                return "Grants the drinker temporary invisibility and enhanced speed, allowing them to move unseen and swiftly through the shadows.";
            case PotionType.Lightstep:
                return "Imbues the drinker with a lightness of step and unparalleled agility, allowing them to move gracefully and silently as if floating on air.";
            case PotionType.SwiftStrength:
                return "Enhances the drinnker's speed and strength, ideal for combat situations where both quick reflexes and durability are needed.";
            case PotionType.PurifyingHealing:
                return "Purges curses, poisons, and dark magic from the drinker's body while accelerating healing.";
            case PotionType.LimbRegrowth:
                return "Regenerates the drinker's lost limbs and heals deep, irreversible injuries.";
            case PotionType.MendingWounds:
                return "Accelerates the drinker's healing of internal wounds while easing pain.";

            default:
                return "No description available.";
        }
    }

    private Sprite GetPotionSprite(PotionType potionType)
    {
        Sprite spriteToUse;

        switch (potionType)
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

        return spriteToUse;
    }
}