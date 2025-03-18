using UnityEngine;
using UnityEngine.UI;

public class BackgroundTransition : MonoBehaviour
{
    public Image morningBackground;
    public Image afternoonBackground;
    public Image nightBackground;

    public float levelDuration=125f;
    private float cumulativeTime = 0f;

    public void Start(){
        SetBackgroundAlpha(morningBackground, 1f);
        SetBackgroundAlpha(afternoonBackground, 0f);
        SetBackgroundAlpha(nightBackground, 0f);
    }

    private void Update(){
        cumulativeTime += Time.deltaTime;

        //calculate which phase the time is in
        float timePercentange = cumulativeTime / levelDuration;

        //fade between backgrounds based on time percentage
        if(timePercentange <= 0.33f){
            float t = timePercentange/0.33f;
            FadeBetweenBackgrounds(morningBackground, afternoonBackground, t);
        }
        else if(timePercentange <= 0.66f){
            float t = (timePercentange - 0.33f)/0.33f;
            FadeBetweenBackgrounds(afternoonBackground, nightBackground, t);
        }
        else{
            SetBackgroundAlpha(morningBackground, 0f);
            SetBackgroundAlpha(afternoonBackground, 0f);
            SetBackgroundAlpha(nightBackground, 1f);
        }
    }

    private void FadeBetweenBackgrounds(Image fromBackground, Image toBackground, float t){
        SetBackgroundAlpha(fromBackground, Mathf.Lerp(1f, 0f, t));
        SetBackgroundAlpha(toBackground, Mathf.Lerp(0f, 1f, t));
    }

    private void SetBackgroundAlpha(Image background, float alpha){
        Color color = background.color;
        color.a = alpha;
        background.color=color;
    }
}
