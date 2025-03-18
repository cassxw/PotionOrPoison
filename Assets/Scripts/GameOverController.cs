using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOverController : MonoBehaviour
{
    [System.Serializable]
    public class EndingSequence
    {
        public Sprite[] images;
        public string[] texts;

        public EndingSequence(Sprite[] images, string[] texts)
        {
            this.images = images;
            this.texts = texts;
        }
    }

    [SerializeField] private GameObject firstSequencePanel;
    [SerializeField] private GameObject finalSequencePanel;
    
    [SerializeField] private Image sequenceImage;
    [SerializeField] private Text sequenceText;
    [SerializeField] private Button continueButton;

    [SerializeField] private Image finalCharacterImage;
    [SerializeField] private Text finalSpeechBubble;

    [SerializeField] private Sprite archmage;
    [SerializeField] private Sprite tiefling;
    [SerializeField] private Sprite king;

    [SerializeField] private EndingSequence firedButGoodDetectiveEnding;
    [SerializeField] private EndingSequence firedAndBadDetectiveEnding;
    [SerializeField] private EndingSequence kingLivesAndPromotedEnding;
    [SerializeField] private EndingSequence kingAssassinatedEnding;

    private EndingSequence currentEnding;

    private void Start()
    {
        // Initialise Ending Sequences
        SetupEndingSequences();

        DetermineEnding();
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(OnContinueButtonClicked);

        StartCoroutine(DisplayEndingSequence());
    }

    private void SetupEndingSequences()
    {
        firedButGoodDetectiveEnding.texts = new string[] {
            "Despite your failure in alchemy, your sharp mind did not go unnoticed. You uncovered the whispers hidden in the shadows, pieced together threats that others could not see. You stood at the edge of greatness, but in the end, you faltered before the final leap.",
            "The kingdom acknowledges your keen intellect and perceptiveness, but without the necessary skill in sorcery, you cannot be entrusted with the Crown’s protection.",
            "As the sun rises over Eilistrae, so too does the kingdom move on, while your journey as a would-be Royal Sorcerer comes to a close. The King’s fate at the upcoming coronation lies in someone else’s hands. You ponder the paths not taken and the secrets you've uncovered. What unseen forces truly shape the fate of Eilistrae?"
        };

        firedAndBadDetectiveEnding.texts = new string[] {
            "Not only was your alchemy insufficient, but you failed to perceive the dangers lurking in plain sight right before your eyes. The kingdom needed more than you could provide.",
            "Whether through ignorance or neglect, you allowed enemies of the kingdom to grow in strength under your very nose, and your failure is written in their whispered plans.",
            "Your watch as a Royal Sorcerer has ended before it could truly begin. The people of Eilistrae tremble under the threats that you have failed to extinguish. The King’s fate at the upcoming coronation lies in someone else’s hands."
        };

        kingLivesAndPromotedEnding.texts = new string[] {
            "Through the trials of magic and cunning these past few days, you have proven yourself worthy as a Royal Sorcerer. The coronation proceeds without a hitch, thanks to your vigilance and skill.",
            "You have earned your place among the Royal Sorcerer Guard. Thanks to your cunning and vigilance, the coronation of your King proceeds. And as of now, you stand beside him as his guardian, and a protector of both your kingdom’s conquest and legacy.",
            "The kingdom of Eilistrae rests in peace. But as you don the crest of the Royal Sorcerer Guard, a shadow of doubt creeps into your mind. Was this utopian peace truly built upon justice…or on the cries of the forgotten? Your reflection offers no easy answers."
        };

        kingAssassinatedEnding.texts = new string[] {
            "The coronation never came to pass. The King has been assassinated, and with him, are the ruins of the kingdom you were tasked with protecting.",
            "In your hands, the fate of Eilistrae was lost. Even with your alchemy skills, you misread the signs, trusting where you should have questioned, and now, Elistrae burns in the flames of rebellion. Your action, or inaction, has tipped the scales, but towards what end?",
            "The kingdom shifts, and with it, so do the stories you’ve been told. Those once called rebels now appear to some as visionaries, their cause stirring whispers of change. What will you choose as the future of Eilistrae unfolds before you?"
        };
    }
    

    private void DetermineEnding()
    {
        int level = GameSystem.Instance.GetLevel();
        int lives = GameSystem.Instance.GetLives();
        int threatLevel = GameSystem.Instance.threatLevel;

        // Determine threatLevel threshold depending on current level
        int threshold = 0;
        switch (level)
        {
            case 1: //Seen 1 imposter.
                threshold = 1;
                break;
            case 2: //Seen 3 imposters.
                threshold = 2;
                break;
            case 3: //Seen 7 imposters.
                threshold = 5;
                break;
        }

        if (lives == 0 && threatLevel <= threshold)
        {
            currentEnding = firedButGoodDetectiveEnding;
        }
        else if (lives == 0 && threatLevel > threshold)
        {
            currentEnding = firedAndBadDetectiveEnding;
        }
        else if (lives > 0 && threatLevel <= threshold)
        {
            currentEnding = kingLivesAndPromotedEnding;
        }
        else if (lives > 0 && threatLevel > threshold)
        {
            currentEnding = kingAssassinatedEnding;
        }
        else
        {
            Debug.LogError("Unexpected game state. Unable to determine ending.");
        }
    }

    private IEnumerator DisplayEndingSequence()
    {
        firstSequencePanel.SetActive(true);
        finalSequencePanel.SetActive(false);

        for (int i = 0; i < currentEnding.images.Length && i < currentEnding.texts.Length; i++)
        {
            sequenceImage.sprite = currentEnding.images[i];
            yield return StartCoroutine(TypeText(sequenceText, currentEnding.texts[i]));
            yield return new WaitForSeconds(1f);
        }

        continueButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeText(Text textComponent, string message)
    {
        textComponent.text = "";
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.035f);
        }
    }

    private void ShowFinalSequence()
    {
        firstSequencePanel.SetActive(false);
        finalSequencePanel.SetActive(true);

        // Set the final character image and speech bubble text based on the ending
        if (currentEnding == firedButGoodDetectiveEnding)
        {
            finalCharacterImage.sprite = archmage;
            finalSpeechBubble.text = "You were sharp, young apprentice, but not sharp enough. Your mind is quick, but your alchemy…leaves much to be desired. A Royal Sorcerer must wield both with excellence, and I must take over from here to undo the damage you’ve caused. Perhaps your commendable skills as a detective are better suited for a different path? The kingdom still thanks you, but your journey with us ends here. Care to try again and rewrite your fate?";
        }
        else if (currentEnding == firedAndBadDetectiveEnding)
        {
            finalCharacterImage.sprite = king;
            finalSpeechBubble.text = "I’m sorry, young apprentice, but in both magic and duty, you were not one of the protectors my kingdom needed. Your incompetence has cost me everything, and jeopardised everything I have stol– *cough* I mean, built. The Archmage will have to take matters into his own hands now to clean up the mess you’ve made. There is no place for such ineptitude here in my true Eilistrae. For that, you are banished to the dungeons where no one will remember your name. Now...farewell. Guards!";
        }
        else if (currentEnding == kingLivesAndPromotedEnding)
        {
            finalCharacterImage.sprite = tiefling;
            finalSpeechBubble.text = "Congratulations, ‘hero’. You’ve safeguarded a tyrant, while we, the oppressed, are burned fighting for true freedom. Tell me, do you even know the King you’ve sworn to defend? I see the doubt in your eyes. Perhaps you're not as blind as we thought. Would you make the same choices if you knew the whole truth? Perhaps another journey might open your eyes to our struggles…The truth awaits, if you dare to seek it.";
        }
        else if (currentEnding == kingAssassinatedEnding)
        {
            finalCharacterImage.sprite = tiefling;
            finalSpeechBubble.text = "Your failure has become our salvation, though I suspect you're only now seeing the full picture. That tyrant, evil King is gone, and now, a new dawn rises for Eilistrae. I know the people of your kingdom have been drowning in propaganda for millenia, but I sense confusion in your heart. You must realise now that things are never as simple as they seem. Tell me, would you have chosen differently if you had seen the truth from the start? Care to try again, this time with your eyes truly open to the lies? Join us!";
        }
    }

    private void OnContinueButtonClicked()
    {
        ShowFinalSequence();
    }

    private void OnDestroy()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
    }
}