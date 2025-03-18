using UnityEngine;

public class GlassBottle : MonoBehaviour
{
    public string Name { get; private set; }
    public GlassBottleSlot CurrentSlot;

    void Start()
    {
        CurrentSlot = null;
    }

    public void SetSlot(GlassBottleSlot slot)
    {
        CurrentSlot = slot;
    }

    public void ClearSlot()
    {
        CurrentSlot = null;
    }
}