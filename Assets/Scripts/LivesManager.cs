using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public SceneController sceneController;

    void Start(){
        sceneController = FindAnyObjectByType<SceneController>();
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearts();
    }   

    public void UpdateHearts(){
        int playerLives = GameSystem.Instance.GetLives();
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        //fill the hearts according to how many lives the player has left
        for(int i=0; i<playerLives; i++){
            hearts[i].sprite=fullHeart;
        }
    }

    public void DecreaseLives(){
        Debug.Log("Calling game system decrease lives");
        GameSystem.Instance.DecreaseLives();
        UpdateHearts();

        if(GameSystem.Instance.GetLives() == 0)
        {
            sceneController.LoadScene("GameOver");
        }
    }
}
