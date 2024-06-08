using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MissionMenager : MonoBehaviour
{
    [SerializeField]
    public GameObject missionsContainer;

    [SerializeField]
    private GameObject prefab;

    private PlayerData playerData;

    [SerializeField]
    private QuestRequirementsSO[] requirements;

    [SerializeField]
    private RewardSO[] rewards;

    private GameObject[] quests;

    public MissionButtonData buttonData;

    public bool isButtonClicked = false;

    private void Start()
    {

        quests = new GameObject[3];
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.areQuestsCreated && Math.Abs(DateTime.Parse(playerData.questsDateTime).Subtract(DateTime.UtcNow).TotalHours) <= 24)
        {
            Debug.Log("MISSION_MANAGER: Loading quests from save file");
            LoadQuestsFromSave();
        }
        else
        {
            Debug.Log("MISSION_MANAGER: Creating new quests");
            playerData.questsDateTime = DateTime.UtcNow.ToString();
            MakeNewQuests();
        }
    }

    private void Update()
    {
        if (isButtonClicked)
        {
            AddRewardToPlayerSave();
            isButtonClicked = false;
        }
    }

    private void AddRewardToPlayerSave()
    {
        AddItemsForPlayer(buttonData.rewardName, buttonData.rewardAmount);
        SaveSystem.SavePlayerData(playerData);
        Debug.Log("Otrzymujesz: " + buttonData.rewardName + " w iloœci: " + buttonData.rewardAmount);
    }

    private void AddItemsForPlayer(string itemName, int itemQuantity)
    {
        switch (itemName)
        {
            case ("Gold"):
                playerData.gold += itemQuantity;
                break;
            case ("Meds"):
                playerData.meds += itemQuantity;
                break;
            case ("FirstAid"):
                playerData.firstAid += itemQuantity;
                break;
            case ("Stimpack"):
                playerData.stimpack += itemQuantity;
                break;
            case ("Leftovers"):
                playerData.leftover += itemQuantity;
                break;
            case ("Meal"):
                playerData.meal += itemQuantity;
                break;
            case ("Drink"):
                playerData.drink += itemQuantity;
                break;
            case ("Scraps"):
                playerData.scraps += itemQuantity;
                break;
            case ("Magnet"):
                playerData.magnet += itemQuantity;
                break;
            case ("Shield"):
                playerData.shield += itemQuantity;
                break;
            case ("Coins"):
                playerData.totalCoins += itemQuantity;
                break;
        }
    }

    private void MakeNewQuests()
    {

        GameObject quest1 = Instantiate(prefab, missionsContainer.transform);
        GameObject quest2 = Instantiate(prefab, missionsContainer.transform);
        GameObject quest3 = Instantiate(prefab, missionsContainer.transform);

        quests[0] = quest1;
        quests[1] = quest2;
        quests[2] = quest3;

        quest1.GetComponent<QuestConfig>().allQuestData = quest1.GetComponent<QuestConfig>().allQuestData.Clone();
        quest2.GetComponent<QuestConfig>().allQuestData = quest1.GetComponent<QuestConfig>().allQuestData.Clone();
        quest3.GetComponent<QuestConfig>().allQuestData = quest1.GetComponent<QuestConfig>().allQuestData.Clone();

        int quest1req = GetRandomIntWithMaxLength(requirements.Length);
        int quest1rew = GetRandomIntWithMaxLength(rewards.Length);

        quest1.GetComponent<QuestConfig>().allQuestData.requirements = requirements[quest1req];
        quest1.GetComponent<QuestConfig>().allQuestData.requirements = quest1.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest1.GetComponent<QuestConfig>().allQuestData.reward = rewards[quest1rew];
        quest1.GetComponent<QuestConfig>().allQuestData.reward = quest1.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest1.GetComponent<QuestConfig>().allQuestData.multiplier = GetRandomMultiplier();

        int quest2req = GetRandomIntWithMaxLength(requirements.Length);
        int quest2rew = GetRandomIntWithMaxLength(rewards.Length);

        while (quest2req == quest1req)
        {
            quest2req = GetRandomIntWithMaxLength(requirements.Length);
        }

        while (quest2rew == quest1rew)
        {
            quest2rew = GetRandomIntWithMaxLength(rewards.Length);
        }

        quest2.GetComponent<QuestConfig>().allQuestData.requirements = requirements[quest2req];
        quest2.GetComponent<QuestConfig>().allQuestData.requirements = quest2.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest2.GetComponent<QuestConfig>().allQuestData.reward = rewards[quest2rew];
        quest2.GetComponent<QuestConfig>().allQuestData.reward = quest2.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest2.GetComponent<QuestConfig>().allQuestData.multiplier = GetRandomMultiplier();

        int quest3req = GetRandomIntWithMaxLength(requirements.Length);
        int quest3rew = GetRandomIntWithMaxLength(rewards.Length);

        while (quest3req == quest2req && quest3req == quest1req)
        {
            quest3req = GetRandomIntWithMaxLength(requirements.Length);
        }

        while (quest3rew == quest2rew && quest3rew == quest1rew)
        {
            quest3rew = GetRandomIntWithMaxLength(rewards.Length);
        }

        quest3.GetComponent<QuestConfig>().allQuestData.requirements = requirements[quest3req];
        quest3.GetComponent<QuestConfig>().allQuestData.requirements = quest3.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest3.GetComponent<QuestConfig>().allQuestData.reward = rewards[quest3rew];
        quest3.GetComponent<QuestConfig>().allQuestData.reward = quest3.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest3.GetComponent<QuestConfig>().allQuestData.multiplier = GetRandomMultiplier();

        MultiplyValues(quest1);
        MultiplyValues(quest2);
        MultiplyValues(quest3);

        SetDescriptions(quest1);
        SetDescriptions(quest2);
        SetDescriptions(quest3);

        SetQuestsValuesForChecking(quest1, quest2, quest3);

        SaveCreatedMissions(quest1, quest2, quest3);
    }

    private int GetRandomIntWithMaxLength(int length)
    {
        System.Random random = new();
        return random.Next(0, length);
    }

    private int GetRandomMultiplier()
    {
        System.Random random = new();
        return random.Next(1, 5);
    }

    private void SetDescriptions(GameObject quest)
    {
        string text = "";

        if (quest.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount > 0)
        {
            text = "Craft " + SetItemName(quest.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName) + " "
                + quest.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount + " times!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount > 0)
        {
            text = "Use " + SetItemName(quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse) + " "
                + quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount + " times!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount > 0)
        {
            text = "Collect " + quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount + " "
                + SetItemName(quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect) + "!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX > 0)
        {
            text = "Lose a run between the " + quest.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX + " and "
                + quest.GetComponent<QuestConfig>().allQuestData.requirements.dieBeforeScoreY + " score!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill > 0)
        {
            text = "Defeat " + quest.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill + " zombies!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve > 0)
        {
            text = "Reach " + quest.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve + " score!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount > 0)
        {
            text = "Jump " + quest.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount + " times!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount > 0)
        {
            text = "Slide " + quest.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount + " times!";
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed > 0)
        {
            text = "Reach max speed!";
        }
        else
        {
            text = "COMPLETED!\nClaim your reward!";
        }

        quest.GetComponent<QuestConfig>().descriptionText.text = text;
        quest.GetComponent<QuestConfig>().rewardButtonText.text = "Complete to get\n" + quest.GetComponent<QuestConfig>().allQuestData.reward.rewardAmount + "x "
            + SetItemName(quest.GetComponent<QuestConfig>().allQuestData.reward.rewardItemName);
    }

    private void MultiplyValues(GameObject quest)
    {
        if (quest.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
            quest.GetComponent<QuestConfig>().allQuestData.requirements.dieBeforeScoreY = quest.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX + 200;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }
        else if (quest.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount != 0)
        {
            quest.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
        }

        quest.GetComponent<QuestConfig>().allQuestData.reward.rewardAmount *= quest.GetComponent<QuestConfig>().allQuestData.multiplier;
    }

    private string SetItemName(string itemRawName) // usuwa spacje pomiêdzy ma³¹ a wielk¹ liter¹ np. FirstAid -> First Aid
    {
        var words =
    Regex.Matches(itemRawName, @"([A-Z][a-z]+)")
    .Cast<Match>()
    .Select(m => m.Value);

        var withSpaces = string.Join(" ", words);

        return withSpaces;
    }

    private void LoadQuestsFromSave()
    {
        GameObject quest1 = Instantiate(prefab, missionsContainer.transform);
        GameObject quest2 = Instantiate(prefab, missionsContainer.transform);
        GameObject quest3 = Instantiate(prefab, missionsContainer.transform);

        quests[0] = quest1;
        quests[1] = quest2;
        quests[2] = quest3;

        quest1.GetComponent<QuestConfig>().allQuestData = quest1.GetComponent<QuestConfig>().allQuestData.Clone();
        quest2.GetComponent<QuestConfig>().allQuestData = quest2.GetComponent<QuestConfig>().allQuestData.Clone();
        quest3.GetComponent<QuestConfig>().allQuestData = quest3.GetComponent<QuestConfig>().allQuestData.Clone();

        quest1.GetComponent<QuestConfig>().allQuestData.requirements = requirements[playerData.quest1requirementsIndex];
        quest1.GetComponent<QuestConfig>().allQuestData.requirements = quest1.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest2.GetComponent<QuestConfig>().allQuestData.requirements = requirements[playerData.quest2requirementsIndex];
        quest2.GetComponent<QuestConfig>().allQuestData.requirements = quest2.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest3.GetComponent<QuestConfig>().allQuestData.requirements = requirements[playerData.quest3requirementsIndex];
        quest3.GetComponent<QuestConfig>().allQuestData.requirements = quest3.GetComponent<QuestConfig>().allQuestData.requirements.Clone();

        quest1.GetComponent<QuestConfig>().allQuestData.reward = rewards[playerData.quest1rewardIndex];
        quest1.GetComponent<QuestConfig>().allQuestData.reward = quest1.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest2.GetComponent<QuestConfig>().allQuestData.reward = rewards[playerData.quest2rewardIndex];
        quest2.GetComponent<QuestConfig>().allQuestData.reward = quest2.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest3.GetComponent<QuestConfig>().allQuestData.reward = rewards[playerData.quest3rewardIndex];
        quest3.GetComponent<QuestConfig>().allQuestData.reward = quest3.GetComponent<QuestConfig>().allQuestData.reward.Clone();

        quest1.GetComponent<QuestConfig>().allQuestData.multiplier = playerData.quest1multiplier;
        quest2.GetComponent<QuestConfig>().allQuestData.multiplier = playerData.quest2multiplier;
        quest3.GetComponent<QuestConfig>().allQuestData.multiplier = playerData.quest3multiplier;

        quest1.GetComponent<QuestConfig>().allQuestData.completed = playerData.isQuest1Completed;
        quest2.GetComponent<QuestConfig>().allQuestData.completed = playerData.isQuest2Completed;
        quest3.GetComponent<QuestConfig>().allQuestData.completed = playerData.isQuest3Completed;

        quest1.GetComponent<QuestConfig>().allQuestData.rewardClaimed = playerData.isQuest1RewardClaimed;
        quest2.GetComponent<QuestConfig>().allQuestData.rewardClaimed = playerData.isQuest2RewardClaimed;
        quest3.GetComponent<QuestConfig>().allQuestData.rewardClaimed = playerData.isQuest3RewardClaimed;

        MultiplyValues(quest1);
        MultiplyValues(quest2);
        MultiplyValues(quest3);

        UpdateQuestByValueName(quest1, quest2, quest3);
        CheckIfQuestsAreCompleted(quest1, quest2, quest3);

        SetDescriptions(quest1);
        SetDescriptions(quest2);
        SetDescriptions(quest3);

    }

    private void UpdateQuestByValueName(GameObject quest1, GameObject quest2, GameObject quest3)
    {
        string quest1ValueName = playerData.quest1ValueName;

        if (quest1ValueName != string.Empty)
        {

            switch (quest1ValueName)
            {
                case "craftFirstAid":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;
                case "craftStimpack":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;
                case "craftMeal":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;
                case "craftDrink":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;
                case "craftShield":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;
                case "craftMagnet":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest1Value;
                    break;

                case "useFirstAid":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;
                case "useStimpack":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;
                case "useMeal":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;
                case "useDrink":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;
                case "useShield":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;
                case "useMagnet":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest1Value;
                    break;

                case "collectMeds":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest1Value;
                    break;
                case "collectLeftovers":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest1Value;
                    break;
                case "collectScraps":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest1Value;
                    break;
                case "collectCoins":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest1Value;
                    break;

                case "dieAfterScoreX":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX = playerData.quest1Value;
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.dieBeforeScoreY = playerData.quest1Value + 200;
                    break;
                case "enemiesToKill":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill = playerData.quest1Value;
                    break;
                case "scoreToAchieve":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve = playerData.quest1Value;
                    break;
                case "jumpsAmount":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount = playerData.quest1Value;
                    break;
                case "slidesAmount":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount = playerData.quest1Value;
                    break;
                case "achieveMaxSpeed":
                    quest1.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed = playerData.quest1Value;
                    break;
            }
        }

        string quest2ValueName = playerData.quest2ValueName;

        if (quest2ValueName != string.Empty)
        {

            switch (quest2ValueName)
            {
                case "craftFirstAid":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;
                case "craftStimpack":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;
                case "craftMeal":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;
                case "craftDrink":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;
                case "craftShield":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;
                case "craftMagnet":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest2Value;
                    break;

                case "useFirstAid":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;
                case "useStimpack":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;
                case "useMeal":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;
                case "useDrink":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;
                case "useShield":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;
                case "useMagnet":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest2Value;
                    break;

                case "collectMeds":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest2Value;
                    break;
                case "collectLeftovers":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest2Value;
                    break;
                case "collectScraps":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest2Value;
                    break;
                case "collectCoins":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest2Value;
                    break;

                case "dieAfterScoreX":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX = playerData.quest2Value;
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.dieBeforeScoreY = playerData.quest2Value + 200;
                    break;
                case "enemiesToKill":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill = playerData.quest2Value;
                    break;
                case "scoreToAchieve":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve = playerData.quest2Value;
                    break;
                case "jumpsAmount":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount = playerData.quest2Value;
                    break;
                case "slidesAmount":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount = playerData.quest2Value;
                    break;
                case "achieveMaxSpeed":
                    quest2.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed = playerData.quest2Value;
                    break;
            }
        }

        string quest3ValueName = playerData.quest3ValueName;

        if (quest3ValueName != string.Empty)
        {

            switch (quest3ValueName)
            {
                case "craftFirstAid":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;
                case "craftStimpack":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;
                case "craftMeal":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;
                case "craftDrink":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;
                case "craftShield":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;
                case "craftMagnet":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount = playerData.quest3Value;
                    break;

                case "useFirstAid":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;
                case "useStimpack":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;
                case "useMeal":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;
                case "useDrink":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;
                case "useShield":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;
                case "useMagnet":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount = playerData.quest3Value;
                    break;

                case "collectMeds":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest3Value;
                    break;
                case "collectLeftovers":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest3Value;
                    break;
                case "collectScraps":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest3Value;
                    break;
                case "collectCoins":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount = playerData.quest3Value;
                    break;

                case "dieAfterScoreX":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX = playerData.quest3Value;
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.dieBeforeScoreY = playerData.quest3Value + 200;
                    break;
                case "enemiesToKill":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill = playerData.quest3Value;
                    break;
                case "scoreToAchieve":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve = playerData.quest3Value;
                    break;
                case "jumpsAmount":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount = playerData.quest3Value;
                    break;
                case "slidesAmount":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount = playerData.quest3Value;
                    break;
                case "achieveMaxSpeed":
                    quest3.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed = playerData.quest3Value;
                    break;
            }
        }

    }

    private void CheckIfQuestsAreCompleted(GameObject quest1, GameObject quest2, GameObject quest3)
    {
        if (playerData.quest1Value <= 0)
        {
            playerData.isQuest1Completed = true;
            quest1.GetComponent<QuestConfig>().allQuestData.completed = true;
        }

        if (playerData.quest2Value <= 0)
        {
            playerData.isQuest2Completed = true;
            quest2.GetComponent<QuestConfig>().allQuestData.completed = true;
        }

        if (playerData.quest3Value <= 0)
        {
            playerData.isQuest3Completed = true;
            quest3.GetComponent<QuestConfig>().allQuestData.completed = true;
        }
    }

    private void SetQuestsValuesForChecking(GameObject quest1, GameObject quest2, GameObject quest3)
    {
        //quest1

        if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName != string.Empty)
        {
            playerData.quest1ValueName = "craft" + quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName;
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse != string.Empty)
        {
            playerData.quest1ValueName = "use" + quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse;
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect != string.Empty)
        {
            playerData.quest1ValueName = "collect" + quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect;
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX != 0)
        {
            playerData.quest1ValueName = "dieAfterScoreX";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill != 0)
        {
            playerData.quest1ValueName = "enemiesToKill";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve != 0)
        {
            playerData.quest1ValueName = "scoreToAchieve";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount != 0)
        {
            playerData.quest1ValueName = "jumpsAmount";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount != 0)
        {
            playerData.quest1ValueName = "slidesAmount";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount;
        }
        else if (quest1.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed != 0)
        {
            playerData.quest1ValueName = "achieveMaxSpeed";
            playerData.quest1Value = quest1.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed;
        }

        // quest2

        if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName != string.Empty)
        {
            playerData.quest2ValueName = "craft" + quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName;
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse != string.Empty)
        {
            playerData.quest2ValueName = "use" + quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse;
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect != string.Empty)
        {
            playerData.quest2ValueName = "collect" + quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect;
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX != 0)
        {
            playerData.quest2ValueName = "dieAfterScoreX";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill != 0)
        {
            playerData.quest2ValueName = "enemiesToKill";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve != 0)
        {
            playerData.quest2ValueName = "scoreToAchieve";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount != 0)
        {
            playerData.quest2ValueName = "jumpsAmount";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount != 0)
        {
            playerData.quest2ValueName = "slidesAmount";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount;
        }
        else if (quest2.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed != 0)
        {
            playerData.quest2ValueName = "achieveMaxSpeed";
            playerData.quest2Value = quest2.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed;
        }

        // quest3

        if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName != string.Empty)
        {
            playerData.quest3ValueName = "craft" + quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemName;
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.craftItemAmount;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse != string.Empty)
        {
            playerData.quest3ValueName = "use" + quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUse;
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToUseAmount;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect != string.Empty)
        {
            playerData.quest3ValueName = "collect" + quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollect;
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.itemToCollectAmount;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX != 0)
        {
            playerData.quest3ValueName = "dieAfterScoreX";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.dieAfterScoreX;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill != 0)
        {
            playerData.quest3ValueName = "enemiesToKill";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.enemiesToKill;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve != 0)
        {
            playerData.quest3ValueName = "scoreToAchieve";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.scoreToAchieve;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount != 0)
        {
            playerData.quest3ValueName = "jumpsAmount";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.jumpsAmount;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount != 0)
        {
            playerData.quest3ValueName = "slidesAmount";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.slidesAmount;
        }
        else if (quest3.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed != 0)
        {
            playerData.quest3ValueName = "achieveMaxSpeed";
            playerData.quest3Value = quest3.GetComponent<QuestConfig>().allQuestData.requirements.achieveMaxSpeed;
        }
    }

    private void SaveCreatedMissions(GameObject quest1, GameObject quest2, GameObject quest3)
    {
        playerData.quest1requirementsIndex = quest1.GetComponent<QuestConfig>().allQuestData.requirements.index;
        playerData.quest2requirementsIndex = quest2.GetComponent<QuestConfig>().allQuestData.requirements.index;
        playerData.quest3requirementsIndex = quest3.GetComponent<QuestConfig>().allQuestData.requirements.index;

        playerData.quest1rewardIndex = quest1.GetComponent<QuestConfig>().allQuestData.reward.index;
        playerData.quest2rewardIndex = quest2.GetComponent<QuestConfig>().allQuestData.reward.index;
        playerData.quest3rewardIndex = quest3.GetComponent<QuestConfig>().allQuestData.reward.index;

        playerData.quest1multiplier = quest1.GetComponent<QuestConfig>().allQuestData.multiplier;
        playerData.quest2multiplier = quest2.GetComponent<QuestConfig>().allQuestData.multiplier;
        playerData.quest3multiplier = quest3.GetComponent<QuestConfig>().allQuestData.multiplier;

        playerData.isQuest1Completed = quest1.GetComponent<QuestConfig>().allQuestData.completed;
        playerData.isQuest2Completed = quest2.GetComponent<QuestConfig>().allQuestData.completed;
        playerData.isQuest3Completed = quest3.GetComponent<QuestConfig>().allQuestData.completed;

        playerData.isQuest1RewardClaimed = quest1.GetComponent<QuestConfig>().allQuestData.rewardClaimed;
        playerData.isQuest2RewardClaimed = quest2.GetComponent<QuestConfig>().allQuestData.rewardClaimed;
        playerData.isQuest3RewardClaimed = quest3.GetComponent<QuestConfig>().allQuestData.rewardClaimed;

        playerData.areQuestsCreated = true;

        SaveSystem.SavePlayerData(playerData);
    }

    internal void RebuildMissions()
    {
        Destroy(quests[0]);
        Destroy(quests[1]);
        Destroy(quests[2]);

        quests = new GameObject[3];

        playerData = SaveSystem.LoadPlayerData();

        Debug.Log("MISSION_MANAGER: Updating quests data!");

        LoadQuestsFromSave();
    }

    internal void SaveMissionsWhenRewardClaimed()
    {
        SaveCreatedMissions(quests[0], quests[1], quests[2]);
    }

    public GameObject[] Quests { get { return quests; } }

}
