using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    public CursorController cursorController;

    public void Awake()
    {
        cursorController.ChangeCursor("neutral");
    }

    // Method to change scenes
    public void LoadScene(string sceneName)
    {
        cursorController.ChangeCursor("hover");

        // Play the button click sound before loading the scene
        if (audioSource != null)
        {
            audioSource.Play();

            // Start a coroutine to wait for the sound to finish before loading the scene
            StartCoroutine(WaitForSoundAndLoadScene(sceneName));
        }
        else //To handle Giving Up
        {
            if (sceneName == "MainMenu")
            {
                Debug.Log("Resetting Game!");
                GameSystem.Instance.InitialiseGame();
                SceneManager.LoadScene(sceneName);
            }
            else if (sceneName == "Level")
            {
                // If just finished Level 3 => GameOver
                if (GameSystem.Instance.GetLevel() == 3)
                {
                    SceneManager.LoadScene("GameOver");
                }
                else
                {
                    GameSystem.Instance.GoToNextLevel();
                    Debug.Log($"Starting Level {GameSystem.Instance.GetLevel()}");
                    SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private System.Collections.IEnumerator WaitForSoundAndLoadScene(string sceneName)
    {
        // Wait for the audio clip to finish playing
        yield return new WaitForSeconds(0.2f);

        // Now load the scene
        if (sceneName == "MainMenu")
        {
            Debug.Log("Resetting Game!");
            GameSystem.Instance.InitialiseGame();
            SceneManager.LoadScene(sceneName);
        }
        else if (sceneName == "Level")
        {
            // If just finished Level 3 => GameOver
            if (GameSystem.Instance.GetLevel() == 3)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                GameSystem.Instance.GoToNextLevel();
                Debug.Log($"Starting Level {GameSystem.Instance.GetLevel()}");
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
