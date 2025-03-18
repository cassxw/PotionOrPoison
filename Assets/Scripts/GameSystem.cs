using UnityEngine;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public int CurrentLevel;
    
    // THROUGHOUT THE GAME
    public int playerLives;
    public int threatLevel; //Percentage (1-100) or [0; no. of Imposters Throughout Level (which is 7 (1+2+4))]
    public List<PotionType> KnownPotions;

    // PER-LEVEL
    public int correctCrafts, incorrectCrafts;
    public int rebelsKilled, rebelsMissed;
    public int innocentsKilled;
    public Texture2D cursorNeutral;
    public List<int> impostorIndices = new List<int>();

    private void Awake()
    {
        Cursor.SetCursor(cursorNeutral, new Vector2(4f, 2f), CursorMode.ForceSoftware);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitialiseGame();
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public void InitialiseGame()
    {
        CurrentLevel = 0;
        playerLives = 5;
        threatLevel = 0;
        KnownPotions = new List<PotionType>();
    }

    public void GoToNextLevel()
    {
        if (CurrentLevel <= 3)
        {
            CurrentLevel += 1;
             
            // Reset per-level scores
            correctCrafts = 0;
            incorrectCrafts = 0;
            rebelsKilled = 0;
            rebelsMissed = 0;
            innocentsKilled = 0;
        }
        else
        {
            // If CurrentLevel now 4, go to Ending
            // This will be handled by SceneController
        }        
    }

    public void DecreaseLives(){
        Debug.Log("Decreasing lives in game system");
        playerLives -= 1;
    }

    public int GetLives(){
        return playerLives;
    }

    public void SetLives(int lives){
        playerLives = lives;
    }

    public void SetCurrentLevel(int level)
    {
        CurrentLevel = level;
    }

    public int GetLevel()
    {
        return CurrentLevel;
    }

    public bool AddKnownPotion(PotionType potionType)
    {
        if (!IsKnownPotion(potionType))
        {
            KnownPotions.Add(potionType);
            return true;
        }

        return false;
    }

    public bool IsKnownPotion(PotionType potionType)
    {
        return KnownPotions.Contains(potionType);
    }

    public void ExitGame()
    {
        // Only works in the built game
        Application.Quit();
        
        // Debug message for testing in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void UpdateImpostorIndices(int usedIndex){
        impostorIndices.Add(usedIndex);
    }

    public List<int> GetImpostorIndices(){
        return impostorIndices;
    }
}