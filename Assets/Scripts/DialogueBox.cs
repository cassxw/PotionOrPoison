using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueBox : MonoBehaviour
{
    public Text requestText;
    public Button reasonButton;
    public Button backButton;
    public Button nextButton;
    public GameObject timerObject;
    private Timer timer;
    public bool reasonsButtonClicked = false;
    public string customerRequest;
    public string customerReason;
    private AudioSource audioSource;
    public AudioClip satisfiedSound;
    public AudioClip unsatisfiedSound;
    public bool imposter = false;
    private string[] rightPotionResponses;
    private string[] wrongPotionResponses;
    private string[] timeoutResponses;

    public CursorController cursorController;

    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        // Get the Timer component from the timerObject
        if (timerObject != null)
        {
            timer = timerObject.GetComponent<Timer>(); // Access the Timer component
            if (timer == null)
            {
                Debug.LogError("Timer component not found on the assigned GameObject!");
            }
            else
            {
                Debug.Log("Timer component successfully found.");
            }
        }
        else
        {
            Debug.LogError("timerObject is null.");
        }

        // Check if the reasonButton is assigned
        if (reasonButton != null)
        {
            reasonButton.onClick.AddListener(ShowReasons); // Add listener for button click
        }
        else
        {
            Debug.LogError("reasonButton is null!");
        }

        if(backButton != null){
            backButton.gameObject.SetActive(false);
            backButton.interactable = false;
            backButton.onClick.AddListener(BackToRequest);
        }

        if(nextButton != null){
            nextButton.gameObject.SetActive(false);
            nextButton.interactable = false;
            nextButton.onClick.AddListener(BackToReason);
        }

        rightPotionResponses = new string[]{
            "Oh, this is perfect! Just what I needed. Thank you—you're a true master of your craft!", 
            "I knew I could count on you!", 
            "Fascinating! You’ve truly outdone yourself. I can already tell this will do the trick. Thank you!", 
            "Wow, this is amazing! You’ve saved my day. I’ll definitely be back for more of your magic!", 
            "Ah, wonderful! You've exceeded my expectations. I shall not forget your skill and kindness.", 
            "Finally! This is exactly what I needed. You're a lifesaver, truly. Thank you!", 
            "This is just what I was hoping for. You’ve done me a great service. Much appreciated!", 
            "Yes! I can feel the magic in this. You’ve got real talent, my friend. Thank you!"
        };

        wrongPotionResponses = new string[]{
           "This doesn't look right. Are you sure you're even a sorcerer?", 
           "This is... not what I was expecting. Perhaps you should double-check your notes.", 
           "This? Really? I’ve seen better concoctions from street vendors!", 
           "This looks off. Is this really what you’re giving me? I was hoping for something a little more... effective.", "Are you trying to get me killed with this? This isn’t what I expected!", 
           "I don’t think this is what I asked for. Is this your first day on the job?", 
           "By the beard of Moradin, this isn’t what I asked for! You’d better not be playing tricks on me, mage.", 
           "I’ve traveled far and wide for a solution, and you give me this? I hope you’re not toying with me, sorcerer.", "This vial feels... wrong. My instincts scream at me to avoid it. Is this truly your idea of aid?", 
           "I’ve dealt with potions my whole life, and this... reeks of failure. Care to try again, mage?"
        };

        timeoutResponses = new string[]{
            "I don’t have all day! If you can’t brew a simple potion in time, what good are you?", 
            "In my tribe, slowness means death. Maybe you're not cut out for this line of work, mage.", 
            "I could have crossed the entire kingdom and back by now! Don’t think I’ll be recommending your services.", "Time is precious, and you’ve squandered mine. I’ll be sure to tell others of your ‘efficiency.’", 
            "Do you think this is a game? I've got better things to do than wait for a second-rate sorcerer to get it right!", 
            "Magic may be timeless, but my patience is not. I expected better from a so-called alchemist.", 
            "Even the winds grow tired of waiting. If you cannot deliver what was promised, perhaps nature will take its due."
        };
    }

    public void UpdateDialogue(string responseType ){
        string newText;
        
        if(responseType == "rightPotion"){
            newText = rightPotionResponses[Random.Range(0, rightPotionResponses.Length)];
            audioSource.clip = satisfiedSound;
            audioSource.Play();
        }
        else if(responseType == "wrongPotion"){
            newText = wrongPotionResponses[Random.Range(0, wrongPotionResponses.Length)];
            audioSource.clip = unsatisfiedSound;
            audioSource.Play();
        }
        else if(responseType == "timeout"){
            newText = timeoutResponses[Random.Range(0, timeoutResponses.Length)];
            audioSource.clip = unsatisfiedSound;
            audioSource.Play();
        }
        else{
            newText="";
        }

        requestText.text = newText;
    }
    public void UpdateDialogue(string request, string selectedReason, bool isImposter){
        customerRequest=request;
        requestText.text=customerRequest;
        customerReason=selectedReason;
        // reasonsText.text="";
        imposter=false;

        if(isImposter){
            imposter=true;
        }

        reasonsButtonClicked = false;
        backButton.interactable = false;
        backButton.gameObject.SetActive(false);
        nextButton.interactable = false;
        nextButton.gameObject.SetActive(false);
        reasonButton.gameObject.SetActive(true);
        reasonButton.interactable = true;
    }

    private void BackToRequest(){
        cursorController.ChangeCursor("neutral");
        requestText.text=customerRequest;
        backButton.interactable = false;
        backButton.gameObject.SetActive(false);
        nextButton.interactable = true;
        nextButton.gameObject.SetActive(true);
    }

    private void BackToReason(){
        cursorController.ChangeCursor("neutral");
        requestText.text=customerReason;
        nextButton.interactable = false;
        nextButton.gameObject.SetActive(false);
        backButton.interactable = true;
        backButton.gameObject.SetActive(true);
    }

    private void ShowReasons(){
        cursorController.ChangeCursor("neutral");
        if(customerReason != null){
            requestText.text=customerReason;
            
            if (timer != null)
            {
                timer.DockTime(5f);
            }
        }
        reasonsButtonClicked = true;
        reasonButton.interactable = false;
        reasonButton.gameObject.SetActive(false);
        backButton.interactable = true;
        backButton.gameObject.SetActive(true);
    }

    public void ClearDialogue(){
        requestText.text="";
    }
}
