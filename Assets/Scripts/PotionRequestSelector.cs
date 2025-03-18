using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionRequestSelector : MonoBehaviour
{
    private PotionRequest[] potionRequests;
    private List<string> usedRequests = new List<string>();

    void Awake()
    {
        potionRequests = new PotionRequest[]
        {
            //level 1 potions
            new PotionRequest {potionType = PotionType.BewitchedLove, request = "I want to enchant someone so that they are irresistibly drawn to me.", reasons = new string[] { "I’ve fallen in love with a kind-hearted noble and want them to see me in the same light."}, level=1},
            new PotionRequest {potionType = PotionType.BewitchedLove, request = "To save my marriage, I need my beloved to see me as they once did, even if I must bewitch their mind to do it.", reasons = new string[] { "I fear that my spouse’s hesitation is more than just a loss of affection—it could be sympathy for the tiefling rebels. I must restore their love and keep them away from such treachery."}, level=1 },
            new PotionRequest {potionType = PotionType.Thoughtweaving, request = "I seek to peer into the thoughts of an associate, to unravel the truths they hide behind their words.", reasons = new string[] { "My business partner is undercutting me. I need evidence on that brat!", "Ah, you know how the winds blow in our circles—one wrong whisper and it’s curtains! I just need to ensure our friend is as loyal as they claim, lest they become a liability in these turbulent times."}, level=1},
            new PotionRequest {potionType = PotionType.Thoughtweaving, request = "I need something to help me see the thoughts behind another’s eyes. Sometimes words just don't tell the whole story!", reasons = new string[] { "I’ve been as clumsy as a goblin in love, trying to win back my spouse’s heart. A little peek into their mind might just help me say the right thing for once!", "With all the excitement around the coronation, I’d like to be sure a few folks aren’t withholding any… interesting details. You know, just in case there’s something I ought to know for the festivities!" }, level=1},
            new PotionRequest {potionType = PotionType.EvasiveShapeshifting, request = "I need to slip quickly through unfamiliar halls without raising a single eyebrow. They shouldn't be able to recognize that it's me.", reasons = new string[] { "My tiefling friend finds himself unjustly locked away for stealing a measly loaf of bread! I need to help him escape.", "The coronation’s coming up, and I’m just a humble guest hoping to admire the palace layout—purely for academic purposes, of course. You never know what little details might be… enlightening." }, level=1},
            new PotionRequest {potionType = PotionType.EvasiveShapeshifting, request = "I need to shift my form at a moment’s notice!", reasons = new string[] { "I want to become a butterfly for the day and flutter around the garden—who wouldn’t want to take a break from swordplay and enjoy the flowers?", "Flexibility is key, especially when your plans might need a quick rewrite. A bit of shapeshifting never hurt anyone, right?"}, level=1},

            //level 2 potions

            new PotionRequest {potionType = PotionType.BewitchedLove, request = "I want to bind someone’s affection towards me, no matter the cost.", reasons = new string[] { "Someone I care for is slipping away, and I need to secure their loyalty before they leave."}, level=2},
            new PotionRequest {potionType = PotionType.Thoughtweaving, request = "I want to penetrate the thoughts of one close to me, to uncover the secrets their heart dares not speak.", reasons = new string[] { "My beloved seems to grow further and further away from me. I just need to understand why, before I lose them.", "There’s a little something they’re not sharing, and it’s throwing our plans into a bit of a tizzy. A gentle nudge into their thoughts could reveal just what’s brewing." }, level=2},
            new PotionRequest {potionType = PotionType.Thoughtweaving, request = "Can you brew something to hear the thoughts of others? I need some crucial information.", reasons = new string[] { "I suspect my brother is keeping secrets from me - the brat! I just want to protect my inheritance.", "Well…rumour has it the Archmage knows more than he lets on. He owns this place, right? I want to know exactly what’s brewing behind these closed doors of yours." }, level=2},
            new PotionRequest {potionType = PotionType.EvasiveShapeshifting, request = "I desire the swiftness of a fox and the cunning of a chameleon!", reasons = new string[] { "I’m hoping to sneak into the bard’s rehearsal—nothing like a little eavesdropping to steal a few new tunes for the next tavern night!", "A swift getaway is part of the job description! Sometimes, it's better to diguise yourself than to face the music."}, level=2},
            new PotionRequest {potionType = PotionType.EvasiveShapeshifting, request = "I seek the ability to change my form and dash away!", reasons = new string[] { "I’ve got a bet with my mates that I can trick the town guard into thinking I’m a lost noble from a distant land—imagine their confused faces!", "I’ve got a few… important collections to make, and I'd rather not have prying eyes tracking my every move. A quick shift in appearance should keep things smooth and quiet while I go about my business."}, level=2},
            new PotionRequest {potionType = PotionType.SpectralVision, request = "I wish to see and converse with the spirits that linger in the shadows!", reasons = new string[] { "With the coronation nearing, I’d love to catch whispers from the spirits of our ancestors. Who knows what insights they might share to ensure our king’s reign is blessed?"}, level=2},
            new PotionRequest {potionType = PotionType.SpectralVision, request = "I… just want to see my husband once more. He fell fighting for our mana resources in the Old World.", reasons = new string[] { "He fought for our prosperity. He was beautiful.", "He was the best strategist I knew. And I need his courage for the next few days."}, level=2},
            new PotionRequest {potionType = PotionType.SpectralVision, request = "I’ve had dreams of the dead, but they speak in riddles. Is there a potion to help me understand?", reasons = new string[] { "My late mother did not write her favourite pie recipe down before she passed, and I believe she’s trying to tell me!", "Well, I’m searching for answers from ghosts of those in Eilistrae’s past. These secrets could have the potential to make a real change to the kingdom, for the better…"}, level=2},
            new PotionRequest {potionType = PotionType.Tranquillity, request = "Can you give me something for nerves? My hands won’t stop shaking.", reasons = new string[] { "The fervor of the coronation has been nothing short of enchanting, yet utterly overwhelming! I find myself yearning for a moment of tranquility to gather my thoughts.", "One must have steady hands for my delicate work, especially during such...eventful times."}, level=2},
            new PotionRequest {potionType = PotionType.Tranquillity, request = "I need a potion to clear the cobwebs from my mind and ease my thoughts.", reasons = new string[] { "After a long night of reviewing scrolls, I could use a little mental refreshment.", "Let’s just say my plans have become a tangled mess. A little clarity—and maybe some sleep—could work wonders. I’ll need that in the next few days."}, level=2},
            new PotionRequest {potionType = PotionType.Tranquillity, request = "One tonic to quell disconcerting thoughts, please.", reasons = new string[] { "I… befriended a rebel today. Sometimes I..wonder if Elistrae’s utopia is justified by the cost. Anyway, I’ve said too much."}, level=2},
            new PotionRequest {potionType = PotionType.VeiledShadows, request = "I cannot be seen today, alchemist—I need to move swiftly and without a trace!", reasons = new string[] { "An arcane engineer crafting new magic in my neighbourhood has the hots for me. And it is NOT mutual.", "I’ve…got some special cargo to move. Uh, invisibility potions hide what you’re holding too, right?" }, level=2},
            new PotionRequest {potionType = PotionType.VeiledShadows, request = "Do you have something to help me quickly slip away without anyone being able to see me?", reasons = new string[] { "I’ve been invited to the King’s coronation, but I have terrible anxiety around large gatherings, and I just want a way to quietly disappear when needed", "I’d like to attend a certain gathering without drawing … attention. A touch of the unseen can make all the difference in avoiding unwelcome conversations."}, level=2},
            new PotionRequest {potionType = PotionType.Lightstep, request = "Have you got anything to help me move through a crowd like a whisper? I don’t even want them to hear my steps.", reasons = new string[] { "I’m practicing for a dance performance and need to master my movements without tripping over my own feet.", "I have some personal matters to attend to, and it’s best if I slip past certain... distractions."}, level=2},
            new PotionRequest {potionType = PotionType.Lightstep, request = "I need to move gracefully through a busy crowd without drawing attention or jostling others.", reasons = new string[] { "I’ve promised my friends a grand reveal at the festival, and I can’t let a single bump spoil the surprise!", "I have some personal matters to attend to, and it’s best if I slip past certain... distractions."}, level=2},
            new PotionRequest {potionType = PotionType.Lightstep, request = "I need to step lightly today. Got anything for that, hot stuff?", reasons = new string[] { "Tender Elistrae’s Royal Gardeners are planting new flowers for us- a blessing before the coronation. I don’t want to trample them accidentally.", "Maybe I’m just a heavy guy. Let’s say I need to be somewhere with lots of ears and a lack of deep sleepers."}, level=2},

            //level 3 potions

            new PotionRequest {potionType = PotionType.BewitchedLove, request = "I seek to bend the heart of someone oblivious to my charm.", reasons = new string[] { "Their heart remains distant, immune to my affection—I need their gaze to linger and their thoughts to be consumed by me."}, level=3},
            new PotionRequest {potionType = PotionType.Thoughtweaving, request = "I need to delve into the thoughts of a trusted ally.", reasons = new string[] { "There are whispers of rebels in the shadows, and I must ensure my comrades aren’t secretly in league with those tiefling tricksters.", "Some of my allies have been acting a bit...erratic lately. A little insight into their thoughts will help me gauge if they're still on board with our plans. We can’t afford any unexpected surprises right now!"}, level=3},
            new PotionRequest {potionType = PotionType.EvasiveShapeshifting, request = "I’ve heard you can make potions that change your appearance. Could you make me one, just for a day?", reasons = new string[] { "I want to surprise my wife at the coronation tonight by looking like her favourite musical bard - just for fun, of course!", "Let’s just say it might be useful to go unrecognised in certain parts of a certain palace…"}, level=3},
            new PotionRequest {potionType = PotionType.SpectralVision, request = "I desperately seek to pierce the veil between our worlds.", reasons = new string[] { "The coronation tomorrow is steeped in spiritual tradition. I want to fully appreciate it.", "They say the old Kings offer guidance in times of change. I intend to seek their counsel directly, for no specific reason at all…"}, level=3},
            new PotionRequest {potionType = PotionType.Tranquillity, request = "I’ve been feeling so overwhelmed lately. I need something to clear my head and calm my nerves.", reasons = new string[] { "This coronation has everyone on edge, especially with the rebellion rumours swirling…I just want to relax. That’s all our good King wants for us, after all!", "Uh, well…to be honest, the rebellion has been weighing heavily on my mind. I need clarity before I decide where my loyalties lie. You understand, right?"}, level=3},
            new PotionRequest {potionType = PotionType.VeiledShadows, request = "I wish to dart through the night, unseen and swift!", reasons = new string[] { "I’m going to be late for the royal feast and can’t bear the thought of missing dessert! If I can slip past the guards, I’ll arrive just in time for the cake!", "Let’s just say I have some… business to attend to. A bit of extra speed will help me navigate the busy crowd without anyone noticing."}, level=3},
            new PotionRequest {potionType = PotionType.Lightstep, request = "Do you have anything that can help me move softly? I’m delivering an important message.", reasons = new string[] { "These streets have become so dangerous at night with those darn rebels. I need to move undetected.", "Let’s just say not all messages are meant for loyal ears. I need to ensure mine reaches its…intended audience."}, level=3},
            new PotionRequest {potionType = PotionType.Lightstep, request = "Do you have anything to make me more agile and graceful?", reasons = new string[] { "I’m terrified of tripping during the coronation process tomorrow. It would be so embarrassing!", "That old castle of the King has so many creaky floorboards. It would be a shame to disturb anyone’s sleep, especially now."}, level=3},
            new PotionRequest {potionType = PotionType.SwiftStrength, request = "A tonic to ease some heavy hauling, alchemist.", reasons = new string[] { "The king has given me a new home because I was top of my class! Lots to move!", "Uh..Sensitive back…? Ouch, ouch…Uh, there it goes again!"}, level=3},
            new PotionRequest {potionType = PotionType.SwiftStrength, request = "Is there a potion that could give me the strength and speed of a dragon in battle?", reasons = new string[] { "My father is in the Royal Sorcerer Guard, and I’m training to join, too! I need every advantage I can get.", "There is a battle to be had very soon. Let’s just say I need to be ready…to fight, one way or another."}, level=3},
            new PotionRequest {potionType = PotionType.PurifyingHealing, request = "I’ve been cursed by a rival mage. Do you have anything that could lift it?", reasons = new string[] { "Without relief, this curse could ruin my life, and I won’t be able to attend the coronation today to swear fealty to our true King!", "Let’s just say that I have a very important venture tomorrow. I must be cured before then!"}, level=3},
            new PotionRequest {potionType = PotionType.PurifyingHealing, request = "You wouldn’t happen to have something to cure a magical curse, would you?", reasons = new string[] { "A beloved friend has been struck down by a wicked hex, courtesy of a jealous rival. I cannot stand idly by while they languish in misery!", "My… acquaintance is about to enter a rather perilous duel. I need this to ensure they can recover swiftly if things take a turn for the worse."}, level=3},
            new PotionRequest {potionType = PotionType.LimbRegrowth, request = "My leg…it’s been gone for years. I heard of a potion that could restore it. Could you make that for me?", reasons = new string[] { "I lost it in service to the Crown. Now, with the coronation tomorrow, I want to stand proud once more for our Eilistrae."}, level=3},
            new PotionRequest {potionType = PotionType.LimbRegrowth, request = "I need the strongest restoration brew you can make, for whole entire limbs!", reasons = new string[] { "My sister lost her arm in the war. I want to surprise her before the coronation parade today.", "In these “festive” times, accidents happen. It’s always good to be prepared for…anything."}, level=3},
            new PotionRequest {potionType = PotionType.MendingWounds, request = "My tiefling neighbour tried shaving off his horns. I need a deep healing potion that soothes.", reasons = new string[] { "The poor thing is exhausted by the discrimination. He shaved the bone  too close to the skin."}, level=3},
            new PotionRequest {potionType = PotionType.MendingWounds, request = "I seek a remedy that can mend a wound gently, like a river caressing smooth stones, soothing pain as it flows.", reasons = new string[] { "The celebratory duels we’ve been having for the King are getting quite intense. I don’t want to miss any of the festivities.", "In such tumultuous times, one must be ready to bounce back painlessly from any…setbacks."}, level=3},
            new PotionRequest {potionType = PotionType.MendingWounds, request = "Got anything to heal a deep wound without pain?", reasons = new string[] { "The pre-coronation hunt was rougher than expected, and I've been wounded.", "One never knows what might happen in a crowded castle. It’s wise to have something for quick fixes, wouldn’t you say?"}, level=3},
        };
    }

    public PotionRequest GetPotionRequestForCurrentLevel(Customer currentCustomerData, int currentLevel){
        List<PotionRequest> filteredRequests = new List<PotionRequest>();
        List<PotionRequest> suspiciousRequests = new List<PotionRequest>();

        foreach(var request in potionRequests){
            //get requests by level
            if(request.level == currentLevel){
                filteredRequests.Add(request);
                //get all impostor requests
                if(request.reasons.Length == 2){
                    suspiciousRequests.Add(request);
                }
            }
        }

        // If no filtered requests are available, return null
        if (filteredRequests.Count == 0)
        {
            return null;
        }

        // Try to select a random suspicious request if available
        if (currentCustomerData.isImposter && suspiciousRequests.Count > 0)
        {
            // Shuffle suspicious requests for randomness
            Shuffle(suspiciousRequests);
            foreach (var potionRequest in suspiciousRequests)
            {
                if (!usedRequests.Contains(potionRequest.request))
                {
                    usedRequests.Add(potionRequest.request); // Mark as used
                    return potionRequest; // Return a suspicious request
                }
            }
        }

        // Shuffle filtered requests for randomness
        Shuffle(filteredRequests);
        foreach (var potionRequest in filteredRequests)
        {
            if (!usedRequests.Contains(potionRequest.request))
            {
                usedRequests.Add(potionRequest.request); // Mark as used
                return potionRequest; // Return a normal request
            }
        }

        // If no requests are available to return
        return null;
    }

    // Helper method to shuffle a list
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            // Swap
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}