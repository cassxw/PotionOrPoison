using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelIntermissionManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider threatLevelSlider;
    public Text craftFeedbackText;
    public Text poisoningFeedbackText;
    public Text innocentKillsFeedbackText;
    public Image[] ratingStars;

    [Header("Star Sprites")]
    public Sprite fullStar;
    public Sprite emptyStar;

    public AudioSource levelCompleteSound;

    private const int TOTAL_IMPOSTERS = 7;

    private void Start()
    {
        UpdateFeedback();
    }

    private void UpdateFeedback()
    {
        UpdateThreatLevel();
        UpdateCraftFeedback();
        UpdatePoisoningFeedback();
        UpdateInnocentKillsFeedback();
        UpdateRatingStars();
    }

    private void UpdateThreatLevel()
    {
        threatLevelSlider.maxValue = TOTAL_IMPOSTERS;
        threatLevelSlider.value = GameSystem.Instance.threatLevel;
    }

    private void UpdateCraftFeedback()
    {
        int correctCrafts = GameSystem.Instance.correctCrafts;
        int totalCrafts = GameSystem.Instance.correctCrafts + GameSystem.Instance.incorrectCrafts;
        string comment = GetCraftComment(correctCrafts, totalCrafts);
        craftFeedbackText.text = $"You correctly crafted {correctCrafts} out of {totalCrafts} potions...{comment}!";
    }

    private string GetCraftComment(int correct, int total)
    {
        float percentage = (float)correct / total;
        if (percentage >= 0.9f) return "Masterful";
        if (percentage >= 0.7f) return "Well done";
        if (percentage >= 0.5f) return "Not bad";
        if (percentage >= 0.3f) return "Room for improvement";
        return "Keep practicing";
    }

    private void UpdatePoisoningFeedback()
    {
        int rebelsKilled = GameSystem.Instance.rebelsKilled;
        int totalRebels = GameSystem.Instance.rebelsKilled + GameSystem.Instance.rebelsMissed;
        string comment = GetPoisoningComment(rebelsKilled, totalRebels);
        poisoningFeedbackText.text = $"You poisoned {rebelsKilled} out of {totalRebels} rebels...{comment}!";
    }

    private string GetPoisoningComment(int killed, int total)
    {
        float percentage = (float)killed / total;
        if (percentage >= 0.9f) return "Excellent work";
        if (percentage >= 0.7f) return "Good job";
        if (percentage >= 0.5f) return "Decent effort";
        if (percentage >= 0.3f) return "Be more vigilant";
        return "They're slipping through";
    }

    private void UpdateInnocentKillsFeedback()
    {
        int innocentsKilled = GameSystem.Instance.innocentsKilled;
        string comment = GetInnocentKillsComment(innocentsKilled);
        innocentKillsFeedbackText.text = $"You poisoned {innocentsKilled} innocent citizens...{comment}!";
    }

    private string GetInnocentKillsComment(int killed)
    {
        if (killed == 0) return "Perfect";
        if (killed <= 1) return "Be more careful";
        if (killed <= 3) return "Concerning";
        if (killed <= 5) return "Alarming";
        return "Disastrous";
    }

    private void UpdateRatingStars()
    {
        int correctCrafts = GameSystem.Instance.correctCrafts;
        int totalCrafts = GameSystem.Instance.correctCrafts + GameSystem.Instance.incorrectCrafts;
        float percentage = (float)correctCrafts / totalCrafts;
        int rating = Mathf.CeilToInt(percentage * 5);

        for (int i = 0; i < ratingStars.Length; i++)
        {
            ratingStars[i].sprite = i < rating ? fullStar : emptyStar;
        }
    }
}