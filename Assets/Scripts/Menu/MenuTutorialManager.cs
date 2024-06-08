using UnityEngine;

public class MenuTutorialManager : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject canvas;

    public void ShowCraftingTutorial()
    {
        var craftingTutorial = Instantiate(prefab, canvas.transform);
        craftingTutorial.GetComponent<MenuTutorialConfig>().titleText.text = "HOW DO CRAFTING WORK?";
        craftingTutorial.GetComponent<MenuTutorialConfig>().descriptionText.text
            = "Just collect the right amount of resources on the game map and use the \"craft\" button to convert them into one full-fledged item!" +
            "\n\nItems are very useful during the game!" +
            "\n\nFirst Aid: allows you to resume the game after losing. Gives a ghost effect for a few moments." +
            "\n\nStimpack: strengthens the body and allows you to destroy obstacles (except zombies)." +
            "\n\nMeal: Increases your score multiplier. Can only be used before the game." +
            "\n\nDrink: Increases starting points. Can only be used before the game." +
            "\n\nShield: Active until you take damage from any source, then grants invulnerability for a short duration." +
            "\n\nMagnet: Attracts coins and resources.";
    }

    public void ShowMissionsTutorial()
    {
        var missionsTutorial = Instantiate(prefab, canvas.transform);
        missionsTutorial.GetComponent<MenuTutorialConfig>().titleText.text = "HOW DO MISSIONS WORK?";
        missionsTutorial.GetComponent<MenuTutorialConfig>().descriptionText.text
            = "Missions are saved for 24 hours after they are created. That means you have to complete them within that time. " +
            "After that, they will be forfeited whether they have been completed or not (unclaimed rewards will also be forfeited!). " +
            "\nThere are very simple missions and those that will require more commitment." +
            "\n\nComplete missions to collect items that will help you beat your current record!";
    }

    public void ShowAchievementsTutorial()
    {
        var achievementTutorial = Instantiate(prefab, canvas.transform);
        achievementTutorial.GetComponent<MenuTutorialConfig>().titleText.text = "HOW DO ACHIEVEMENTS WORK?";
        achievementTutorial.GetComponent<MenuTutorialConfig>().descriptionText.text
            = "Achievements represent your progress in the game." +
            "\r\n\r\nThere are one-step achievements as well as ten-step achievements. " +
            "The higher the stage, the more stars you get. " +
            "The current stage is shown to the right of the achievement. " +
            "The description indicates what needs to be done to complete the stage." +
            " At the top of the panel, you can see your total stars as well as the maximum you can earn." +
            "\r\n\r\nCompete with your friends and try to get them all!";
    }

    public void ShowSpecialOffersTutorial()
    {
        var specialOffersTutorial = Instantiate(prefab, canvas.transform);
        specialOffersTutorial.GetComponent<MenuTutorialConfig>().titleText.text = "SPECIAL OFFERS";
        specialOffersTutorial.GetComponent<MenuTutorialConfig>().descriptionText.text
            = "Special offers change from time to time and offer a lot of useful things for the best price!" +
            "\r\n\r\nIf an offer interests you, be sure to buy it before it disappears!";
    }
}

