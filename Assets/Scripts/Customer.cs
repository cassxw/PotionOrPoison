using UnityEngine;
[System.Serializable]
public class Customer : MonoBehaviour
{
    public bool isImposter;
    public Sprite image;

    public int level;

    public void Initialise(string name, bool isImposter, Sprite image, int level)
    {
        this.isImposter = isImposter;
        this.image = image;
        this.level = level;
    }
}