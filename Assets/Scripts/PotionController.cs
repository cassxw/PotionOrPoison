using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PotionController : MonoBehaviour
{
    public IngredientSlot ingredientSlot1;
    public IngredientSlot ingredientSlot2;
    public GlassBottleSlot glassBottleSlot;
    public OvenLever ovenLever;
    public OvenCoals ovenCoals;
    public DialogueBox dialogueBox;
    public float potionCreationDelay = 4f;

    public Canvas potionCanvas;
    public RectTransform potionSpawnPoint;

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

    private Dictionary<(IngredientType, IngredientType), PotionType> potionRecipes;

    [SerializeField] private Image ovenSmokeImage;
    [SerializeField] private OvenDoor ovenDoor;

    public LivesManager livesManager;
    public CustomerSpawner customerSpawner;

    public PotionTooltip potionTooltip;

    [Header("Audio")]
    [SerializeField] private AudioSource potionBubblingSound;
    [SerializeField] private AudioSource cannotCraftSound;
    [SerializeField] private AudioSource potionCompleteSound;
    [SerializeField] private AudioSource newPotionSound;
    
    public CursorController cursorController;

    private void Start()
    {
        // Subscribe to the lever pulled event.
        ovenLever.OnLeverPulled += CheckPotionCreation;
        
        livesManager = FindAnyObjectByType<LivesManager>();
        customerSpawner = FindAnyObjectByType<CustomerSpawner>();
        dialogueBox = FindAnyObjectByType<DialogueBox>();

        InitialisePotionRecipes();
    }

    private void InitialisePotionRecipes()
    {
        potionRecipes = new Dictionary<(IngredientType, IngredientType), PotionType>
        {
            // Level One
            { (IngredientType.RosePetalEssence, IngredientType.Wolfsbane), PotionType.BewitchedLove },
            { (IngredientType.Wolfsbane, IngredientType.PegasusFeather), PotionType.EvasiveShapeshifting },
            { (IngredientType.SerpentEye, IngredientType.BeetleBlood), PotionType.Thoughtweaving },
            
            // Level Two
            { (IngredientType.BottledClouds, IngredientType.SerpentEye), PotionType.SpectralVision },
            { (IngredientType.Lavender, IngredientType.BeetleBlood), PotionType.Tranquillity },
            { (IngredientType.BottledClouds, IngredientType.PegasusFeather), PotionType.VeiledShadows },
            { (IngredientType.GossamerThreads, IngredientType.PegasusFeather), PotionType.Lightstep },
            
            // Level Three
            { (IngredientType.CrushedDragonClaw, IngredientType.PegasusFeather), PotionType.SwiftStrength },
            { (IngredientType.MandrakeRoot, IngredientType.PhoenixFeather), PotionType.PurifyingHealing },
            { (IngredientType.Wolfsbane, IngredientType.PhoenixFeather), PotionType.LimbRegrowth },
            { (IngredientType.Lavender, IngredientType.PhoenixFeather), PotionType.MendingWounds }
        };
    }

    private void CheckPotionCreation()
    {
        if (CanCreatePotion())
        {
            StartCoroutine(CreatePotionWithDelay());
        }
        else
        {
            cannotCraftSound.Play();
            Debug.Log("Cannot create potion. Make sure you have two ingredients and a glass bottle.");
        }
    }

    private bool CanCreatePotion()
    {
        return ingredientSlot1.CurrentIngredient != null &&
               ingredientSlot2.CurrentIngredient != null &&
               glassBottleSlot.CurrentBottle != null;
    }

    // Helper method to wait for multiple coroutines
    private IEnumerator WaitForCoroutines(params Coroutine[] coroutines)
    {
        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }

    private IEnumerator CreatePotionWithDelay()
    {
        // Disable slots and oven lever
        LockSlots(true);
        ovenLever.enabled = false;
        cursorController.ChangeCursor("neutral");

        // Lock in the ingredient and glass bottle so it can't be removed
        Destroy(ingredientSlot1.CurrentIngredient.gameObject.GetComponent<CloneDragAndDrop>());
        Destroy(ingredientSlot2.CurrentIngredient.gameObject.GetComponent<CloneDragAndDrop>());
        Destroy(glassBottleSlot.CurrentBottle.gameObject.GetComponent<CloneDragAndDrop>());

        // Destroy the bottle
        Destroy(glassBottleSlot.CurrentBottle.gameObject);

        // Close the oven door
        ovenDoor.gameObject.SetActive(true);
        yield return StartCoroutine(ovenDoor.MoveDoor(true));

        DestroyUsedItems();

        // Start heating up the coals and fade in the smoke simultaneously
        Coroutine heatingCoals = StartCoroutine(ovenCoals.HeatUpCoals());
        Coroutine fadingSmoke = StartCoroutine(FadeOvenSmoke(true));
        Coroutine bubblingSound = StartCoroutine(FadePotionBubblingSound(true));
        yield return WaitForCoroutines(heatingCoals, fadingSmoke, bubblingSound);
        
        // Create the potion behind the oven door
        GameObject newPotionObject = CreatePotionBehindOvenDoor();
        ClearSlots();

        // Open the oven door
        yield return StartCoroutine(ovenDoor.MoveDoor(false));

        // Fix the layering of the potion once oven door is closed
        GameObject poisoningParent = GameObject.Find("Poisoning");
        newPotionObject.transform.SetParent(poisoningParent.transform, false);

        // Ensure still at potionSpawnPoint => inside oven
        RectTransform potionRectTransform = newPotionObject.GetComponent<RectTransform>();
        potionRectTransform.position = potionSpawnPoint.position;

        // Set the scale of the image
        newPotionObject.transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);

        // Show the tooltip with the potion name
        Potion potion = newPotionObject.GetComponent<Potion>();
        potionTooltip.gameObject.SetActive(true);
        potionTooltip.ShowTooltip(potion.type);

        // Add to Known Potions
        bool neverSeen = GameSystem.Instance.AddKnownPotion(potion.type);

        // Play potion complete sound
        if (neverSeen)
        {
            newPotionSound.Play();
        }
        else
        {
            potionCompleteSound.Play();
        }

        // Add FreeDragAndDrop component
        FreeDragAndDrop dragAndDrop = newPotionObject.AddComponent<FreeDragAndDrop>();
        dragAndDrop.isPoisonDropper = false;

        // Copy the CursorController component from Poisoning to newPotionObject
        CursorController originalCursorController = poisoningParent.GetComponent<CursorController>();
        CursorController newCursorController = newPotionObject.AddComponent<CursorController>();
        // Manually copy properties from original CursorController
        newCursorController.cursorNeutral = originalCursorController.cursorNeutral;
        newCursorController.cursorHover = originalCursorController.cursorHover;
        newCursorController.cursorGrab = originalCursorController.cursorGrab;
        dragAndDrop.cursorController = newPotionObject.GetComponent<CursorController>();

        // Cool down the coals and fade out the smoke simultaneously
        Coroutine resetLever = StartCoroutine(ovenLever.SpringBackToInitialPosition());
        Coroutine coolingCoals = StartCoroutine(ovenCoals.CoolDownCoals());
        Coroutine fadingOutSmoke = StartCoroutine(FadeOvenSmoke(false));
        Coroutine stoppingBubbling = StartCoroutine(FadePotionBubblingSound(false));
        yield return WaitForCoroutines(coolingCoals, fadingOutSmoke, resetLever, stoppingBubbling);

        // Enable oven lever and slots
        ovenLever.enabled = true;
        LockSlots(false);
    }

    private GameObject CreatePotionBehindOvenDoor()
    {
        // Create the new potion GameObject
        GameObject newPotionObject = new GameObject("CraftedPotion");

        // Set the potion's initial parent to be behind the oven door
        GameObject slot = GameObject.Find("GlassBottleSlot");
        newPotionObject.transform.SetParent(slot.transform, false);

        // Spawn at potionSpawnPoint => inside oven
        RectTransform potionRectTransform = newPotionObject.AddComponent<RectTransform>();
        potionRectTransform.position = potionSpawnPoint.position;

        // Set up the potion components
        Image potionImage = newPotionObject.AddComponent<Image>();
        CanvasGroup canvasGroup = newPotionObject.AddComponent<CanvasGroup>();
        Potion newPotion = newPotionObject.AddComponent<Potion>();
        newPotion.image = potionImage;

        // Set the potion sprites
        newPotion.GetType().GetField("muckSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, muckSprite);
        newPotion.GetType().GetField("bewitchedLoveSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, bewitchedLoveSprite);
        newPotion.GetType().GetField("evasiveShapeshiftingSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, evasiveShapeshiftingSprite);
        newPotion.GetType().GetField("thoughtweavingSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, thoughtweavingSprite);
        newPotion.GetType().GetField("spectralVisionSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, spectralVisionSprite);
        newPotion.GetType().GetField("tranquillitySprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, tranquillitySprite);
        newPotion.GetType().GetField("veiledShadowsSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, veiledShadowsSprite);
        newPotion.GetType().GetField("lightstepSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, lightstepSprite);
        newPotion.GetType().GetField("swiftStrengthSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, swiftStrengthSprite);
        newPotion.GetType().GetField("purifyingHealingSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, purifyingHealingSprite);
        newPotion.GetType().GetField("limbRegrowthSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, limbRegrowthSprite);
        newPotion.GetType().GetField("mendingWoundsSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(newPotion, mendingWoundsSprite);

        // Set potion type
        PotionType potionType = DeterminePotionType(ingredientSlot1.CurrentIngredient, ingredientSlot2.CurrentIngredient);
        newPotion.SetType(potionType);

        // Set the native size & scale of the image
        newPotion.image.SetNativeSize();
        newPotion.transform.localScale = new Vector3(0.39f, 0.39f, 0.39f);

        return newPotionObject;
    }

    private PotionType DeterminePotionType(Ingredient ingredient1, Ingredient ingredient2)
    {
        var combination = (ingredient1.ingredientType, ingredient2.ingredientType);
        var reverseCombination = (ingredient2.ingredientType, ingredient1.ingredientType);

        if (potionRecipes.TryGetValue(combination, out PotionType potionType))
        {
            return potionType;
        }
        else if (potionRecipes.TryGetValue(reverseCombination, out potionType))
        {
            return potionType;
        }
        else
        {
            return PotionType.Muck;
        }
    }

    private void ClearSlots()
    {
        ingredientSlot1.ClearIngredient();
        ingredientSlot2.ClearIngredient();
        glassBottleSlot.ClearBottle();
    }

    private void LockSlots(bool crafting)
    {
        ingredientSlot1.isCrafting = crafting;
        ingredientSlot2.isCrafting = crafting;
        glassBottleSlot.isCrafting = crafting;
    }

    private void DestroyUsedItems()
    {
        Destroy(ingredientSlot1.CurrentIngredient.gameObject);
        Destroy(ingredientSlot2.CurrentIngredient.gameObject);
    }

    private IEnumerator FadeOvenSmoke(bool fadeIn)
    {
        float duration = 1f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            ovenSmokeImage.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }

        ovenSmokeImage.color = new Color(1f, 1f, 1f, endAlpha);
    }

    private IEnumerator FadePotionBubblingSound(bool fadeIn)
    {
        float duration = 1.5f;
        float startVolume = fadeIn ? 0f : 1f;
        float endVolume = fadeIn ? 1f : 0f;

        AudioSource audioSource = potionBubblingSound;
        audioSource.loop = true;
        audioSource.volume = startVolume;

        if (fadeIn)
        {
            audioSource.Play();
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, normalizedTime);
            yield return null;
        }

        audioSource.volume = endVolume;

        if (!fadeIn)
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        ovenLever.OnLeverPulled -= CheckPotionCreation;
    }

    public void HandlePotionDelivery(Potion potion)
    {
        // Potion Validity Check
        if (potion.type == customerSpawner.selectedRequest.potionType)
        {
            // Correct Potion delivered

            // Increment correctCrafts
            GameSystem.Instance.correctCrafts += 1;

            // Customer reaction
            dialogueBox.UpdateDialogue("rightPotion");
        }
        else
        {
            // Incorrect Potion delivered

            // Lose a Life
            livesManager.DecreaseLives();

            // Increment incorrectCrafts
            GameSystem.Instance.incorrectCrafts += 1;

            // Customer reaction
            dialogueBox.UpdateDialogue("wrongPotion");
        }

        // Poisoning Check
        if (potion.isPoisoned)
        {
            if (customerSpawner.isImposter)
            {
                // Increment rebelsKilled
                GameSystem.Instance.rebelsKilled += 1;
            }
            else
            {
                // Increment innocentsKilled
                GameSystem.Instance.innocentsKilled += 1;
            }
        }
        else
        {
            if (customerSpawner.isImposter)
            {
                // Increment threatLevel + rebelsMissed
                GameSystem.Instance.threatLevel += 1;
                GameSystem.Instance.rebelsMissed += 1;
            }
        }

        StartCoroutine(customerSpawner.CustomerServed());
    }
}