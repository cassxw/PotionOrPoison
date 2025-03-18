using UnityEngine;

public class IngredientSlot : MonoBehaviour
{
    public Ingredient CurrentIngredient;
    public bool isCrafting;

    public AudioSource placeSound;

    void Start()
    {
        CurrentIngredient = null;
        isCrafting = false;
    }

    public void SetIngredient(Ingredient ingredient)
    {
        if (CurrentIngredient != null && CurrentIngredient != ingredient)
        {
            // If there's already a different ingredient, reject the new one
            return;
        }

        if (CurrentIngredient == ingredient)
        {
            // If it's the same ingredient, just update its position
            UpdateIngredientPosition(ingredient);
            return;
        }

        if (!isCrafting)
        {
            // Set the new ingredient
            CurrentIngredient = ingredient;
            ingredient.SetSlot(this);
            UpdateIngredientPosition(ingredient);
        }
    }

    private void UpdateIngredientPosition(Ingredient ingredient)
    {
        placeSound.Play();
        ingredient.transform.position = transform.position;
    }

    public void ClearIngredient()
    {
        if (CurrentIngredient != null)
        {
            CurrentIngredient.ClearSlot();
            CurrentIngredient = null;
        }
    }
}