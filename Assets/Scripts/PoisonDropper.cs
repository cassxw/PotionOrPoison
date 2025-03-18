using UnityEngine;

public class PoisonDropper : MonoBehaviour
{
    [SerializeField] private Sprite ghostSprite;
    [SerializeField] public AudioSource poisonSound;

    public void PoisonPotion(Potion potion)
    {
        potion.SetPoisoned(true);
    }

    public Sprite GetGhostSprite()
    {
        return ghostSprite;
    }
}