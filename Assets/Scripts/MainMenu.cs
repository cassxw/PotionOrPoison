using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject helpMenuPanel;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        InitialiseExitButton();
    }

    private void OnEnable()
    {
        InitialiseExitButton();
    }

    private void InitialiseExitButton()
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("Exit button not assigned in the MainMenu script!");
        }
    }

    public void ShowHelpMenu()
    {
        helpMenuPanel.SetActive(true);
    }

    public void HideHelpMenu()
    {
        helpMenuPanel.SetActive(false);
    }

    private void ExitGame()
    {
        GameSystem.Instance.ExitGame();
    }
}