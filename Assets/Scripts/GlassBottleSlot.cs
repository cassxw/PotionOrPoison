using UnityEngine;

public class GlassBottleSlot : MonoBehaviour
{
    public GlassBottle CurrentBottle;
    public bool isCrafting;

    public AudioSource placeSound;

    void Start()
    {
        CurrentBottle = null;
        isCrafting = false;
    }

    public void SetBottle(GlassBottle bottle)
    {
        if (CurrentBottle != null && CurrentBottle != bottle)
        {
            // If there's already a different bottle, reject the new one
            return;
        }

        if (CurrentBottle == bottle)
        {
            // If it's the same bottle, just update its position
            UpdateBottlePosition(bottle);
            return;
        }

        if (!isCrafting)
        {
            // Set the new bottle
            CurrentBottle = bottle;
            bottle.SetSlot(this);
            UpdateBottlePosition(bottle);
        }
    }

    private void UpdateBottlePosition(GlassBottle bottle)
    {
        placeSound.Play();
        bottle.transform.position = transform.position;
    }

    public void ClearBottle()
    {
        if (CurrentBottle != null)
        {
            CurrentBottle.ClearSlot();
            CurrentBottle = null;
        }
    }
}