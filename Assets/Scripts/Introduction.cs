using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{   
    public Text archMageText;
    public Text playerText;
    private List<string> dialogue;
    public Button responseButton;
    private int currentDialogueIndex = 0;
    private bool playerResponded = false;

    public CursorController cursorController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        responseButton.gameObject.SetActive(false);
        dialogue = new List<string>
        {
            "Ah, there you are. You’ve arrived at a most pivotal moment, apprentice. I am the Archmage of our kingdom of Eilistrae, and the leader of the Royal Sorcerer Guard. For many years, I have watched over the kingdom, preserving its peace, warding off its dangers, and ensuring that the King’s rule is unwavering.",
            
            "You have shown great promise—both in magic and loyalty to the empire. That is why you stand here now, on the verge of greatness. This is your final trial. Pass it, and you will join the ranks of the Royal Guard, an esteemed brotherhood of knights who wield both sword and spell in defense of the King.",
            
            "Player: What is the final trial, Archmage?",
            
            "For three days, you will serve the people of Eilistrae as an alchemist, using only your arcane intuition to guide you. Citizens will come to you with their ailments, but your task is far greater than just curing headaches or rashes. His Majesty's coronation is near, and danger lurks in the shadows.",
            
            "There are those among us— ungrateful few — tiefling rebels who seek to bring chaos to our land. They reject the harmony of our empire, yearning for the fractured world that cast them out. And now, in their heresy, they plot to take the throne for themselves, threatening the life of the King.",
            
            "Your mission, apprentice, is twofold. You must serve the citizens of this kingdom, yes. But you must also protect the King from these rebels. Use your skills wisely. Craft potions not only to heal but also to poison those who seek to harm our King.",
            
            "Player: But how will I know who to poison and who to cure?",
            
            "Not all who come to you will have innocent intentions. Some may be disguised rebels, testing your loyalty. Serve the kingdom by providing remedies to its people, but use your knight’s cunning to deal with those who oppose the crown.",
            
            "For three days, you will walk the line between healer and protector. Succeed, and you will achieve the highest honor a knight of magic can hope for: a place among the Royal Sorcerer Guard.",
            
            "Player: And what if I fail to protect the King?",
            
            "Failure is not an option. If you fail, the kingdom falls, and you will have to live with the weight of that on your conscience—if you live at all.",
            
            "The fate of the kingdom lies in your hands. The people will seek healing. The rebels will seek destruction. It is up to you to decide who receives which. Now, apprentice—prove your worth.",
            
            "Player: I won’t let the rebels take the throne. I’m ready."
        };

        responseButton.onClick.AddListener(Respond);
        StartCoroutine(DisplayDialogueSequence());
    }

    IEnumerator DisplayDialogueSequence(){
        while(currentDialogueIndex < dialogue.Count){
            string currentText = dialogue[currentDialogueIndex];

            if (!currentText.StartsWith("Player: "))
            {
                // Display archmage text and wait for a few seconds
                yield return StartCoroutine(TypeText(archMageText, currentText));
                yield return new WaitForSeconds(1f); // Delay for archmage dialogues
            }
            else
            {
                // When it's the player's turn, show the button and wait for response
                responseButton.gameObject.SetActive(true);
                yield return new WaitUntil(() => playerResponded); // Wait until player responds

                // Display player text after response
                 yield return StartCoroutine(TypeText(playerText, currentText.Replace("Player: ", "")));


                // Hide button and reset the response flag
                responseButton.gameObject.SetActive(false);
                playerResponded = false;

                yield return new WaitForSeconds(1f);
            }

            // Move to the next dialogue line
            currentDialogueIndex++;
        }
    }
    IEnumerator TypeText(Text textComponent, string message){
        textComponent.text = "";
        foreach(char letter in message){
            textComponent.text +=letter;
            yield return new WaitForSeconds(0.035f);
        }
    }
    public void Respond(){
        cursorController.ChangeCursor("hover");
        playerResponded = true;
        cursorController.ChangeCursor("neutral");
    }
}
