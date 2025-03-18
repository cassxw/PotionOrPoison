using System;
// using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;
    private bool stopTimer;
    private float timeRemaining;
    public bool isPaused;
    public delegate void TimeDockedEvent(float timeDocked);
    public static event TimeDockedEvent OnTimeDocked;
    public int currentLevel;

    public Color startGreen = new Color(42f / 255f, 176f / 255f, 69f / 255f);  // Green: rgb(42, 176, 69)
    public Color midYellow = new Color(255f / 255f, 236f / 255f, 110f / 255f); // Yellow: rgb(255, 236, 110)
    public Color endRed = new Color(189f / 255f, 50f / 255f, 20f / 255f);      // Red: rgb(189, 50, 20)

    void Start(){
        currentLevel = GameSystem.Instance.CurrentLevel;
        switch(currentLevel){
            case 1:
                Debug.Log("Timer in level 1");
                gameTime = 60f;
                break;
            case 2:
                gameTime = 40f;
                break;
            case 3:
                gameTime = 30f;
                break;
            default:
                gameTime =0f;
                return;
        }

        stopTimer = false;
        isPaused = true;
        timeRemaining = gameTime;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
        
    }

    void Update(){
        if (!stopTimer && !isPaused)
        {
            // Update time remaining
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                stopTimer = true;
                timeRemaining = 0;  // Prevent negative time
            }

            // Convert time to minutes and seconds
            string textTime = FormatTime(timeRemaining);

            // Update UI elements
            timerText.text = textTime;
            timerSlider.value = timeRemaining;

            UpdateTimerColor();
        }
    }

    private void UpdateTimerColor(){
        float percentageRemaining = timeRemaining / gameTime;

        if (percentageRemaining > 0.5f)
        {
            // Interpolate from green to yellow as time decreases from 100% to 50%
            Color newColor = Color.Lerp(midYellow, startGreen, (percentageRemaining - 0.5f) * 2);
            timerText.color = newColor;
            timerSlider.fillRect.GetComponent<Image>().color = newColor;
        }
        else
        {
            // Interpolate from yellow to red as time decreases from 50% to 0%
            Color newColor = Color.Lerp(endRed, midYellow, percentageRemaining * 2);
            timerText.color = newColor;
            timerSlider.fillRect.GetComponent<Image>().color = newColor;
        }
    }

    public void ResetTime(){
        stopTimer=false;
        isPaused=false;
        timeRemaining=gameTime; //Reset time remaining
        timerSlider.value=gameTime; //Reset slider
    }

    public void DockTime(float timeToDock){
        timeRemaining -= timeToDock;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            stopTimer = true;
        }

        timerText.text= FormatTime(timeRemaining);
        timerSlider.value = timeRemaining;

        // Notify listeners that time was docked
        OnTimeDocked?.Invoke(timeToDock);
    }

    public void Pause(){
        isPaused=true;
    }

    public void Resume(){
        isPaused = false;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public float GetGameTime(){
        return gameTime;
    }
}
