using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum IngredientType
{
    // Level 1
    PegasusFeather,
    Wolfsbane,
    SerpentEye,
    BeetleBlood,
    RosePetalEssence,
    
    // Level 2
    Lavender,
    BottledClouds,
    GossamerThreads,

    // Level 3
    CrushedDragonClaw,
    MandrakeRoot,
    PhoenixFeather  
}

// Represents an Ingredient that can be placed in an IngredientSlot,
// to be used (with one other Ingredient) to create a Potion.
public class Ingredient : MonoBehaviour
{
    public IngredientType ingredientType;
    public Image image;

    public string Name { get; private set; }
    public List<string> Effects { get; private set; }

    // The current slot this ingredient is placed in.
    public IngredientSlot CurrentSlot;

    //---------------------------------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitialiseName();
        InitialiseEffects();
    }
    //---------------------------------------------------------------------------------------------

    private void InitialiseName()
    {
        switch (ingredientType)
        {
            case IngredientType.PegasusFeather:
                Name = "Pegasus Feather";
                break;

            case IngredientType.SerpentEye:
                Name = "Serpent's Eye";
                break;

            case IngredientType.BeetleBlood:
                Name = "Beetle's Blood";
                break;

            case IngredientType.RosePetalEssence:
                Name = "Rose Petal Essence";
                break;

            case IngredientType.BottledClouds:
                Name = "Bottled Clouds";
                break;

            case IngredientType.GossamerThreads:
                Name = "Gossamer Threads";
                break;
            
            case IngredientType.CrushedDragonClaw:
                Name = "Crushed Dragon Claw";
                break;

            case IngredientType.MandrakeRoot:
                Name = "Mandrake Root";
                break;
            
            case IngredientType.PhoenixFeather:
                Name = "Phoenix Feather";
                break;
            
            default:
                Name = ingredientType.ToString();
                break;
        }
    }

    private void InitialiseEffects()
    {
        Effects = new List<string>();
        switch (ingredientType)
        {
            case IngredientType.PegasusFeather:
                Effects.Add("Speed");
                Effects.Add("Reflexes");
                Effects.Add("Agility");
                break;

            case IngredientType.Wolfsbane:
                Effects.Add("Shapeshifting");
                Effects.Add("Regeneration");
                Effects.Add("Enchantment");
                break;

            case IngredientType.SerpentEye:
                Effects.Add("Insight");
                Effects.Add("Telepathy");
                Effects.Add("Clairvoyance");
                break;

            case IngredientType.BeetleBlood:
                Effects.Add("Wisdom");
                Effects.Add("Clarity");

                break;

            case IngredientType.RosePetalEssence:
                Effects.Add("Love");
                Effects.Add("Attraction");
                Effects.Add("Affection");
                break;

            case IngredientType.Lavender:
                Effects.Add("Soothing");
                Effects.Add("Sleep-Inducing");

                break;

            case IngredientType.BottledClouds:
                Effects.Add("Invisibility");
                Effects.Add("Levitation");
                Effects.Add("Spirits");
                break;

            case IngredientType.GossamerThreads:
                Effects.Add("Elegance");
                Effects.Add("Lightness");
                Effects.Add("Grace");
                break;
            
            case IngredientType.CrushedDragonClaw:
                Effects.Add("Strength");
                Effects.Add("Durability");
                
                break;

            case IngredientType.MandrakeRoot:
                Effects.Add("Invulnerability");
                Effects.Add("Curse Protection");
                break;
            
            case IngredientType.PhoenixFeather:
                Effects.Add("Healing");
                Effects.Add("Purification");
                Effects.Add("Luck");
                break;
        }
    }

    public void SetSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }

    // Sets the current IngredientSlot for this Ingredient.
    // Param: slot - The IngredientSlot to associate with this Ingredient.
    public void SetSlot(IngredientSlot slot)
    {
        CurrentSlot = slot;
    }

    // Clears the current IngredientSlot association.
    public void ClearSlot()
    {
        
        CurrentSlot = null;
    }
}