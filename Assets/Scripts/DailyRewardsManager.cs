using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DailyRewardsManager : MonoBehaviour
{
    private PlayerData playerData;

    [SerializeField]
    private InfoManager infoManager;

    [SerializeField]
    private GameObject dailyRewardsPanel;

    [SerializeField]
    private Button dailyRewardsButton;

    [SerializeField]
    private Button dayOne, dayTwo, dayThree, dayFour, dayFive, daySix, daySeven;

    [SerializeField]
    private DailyRewardSO[] normalDailyRewards;

    [SerializeField]
    private DailyRewardSO[] specialDailyRewards;

    [SerializeField]
    private TextMeshProUGUI nextRewardText;

    private TimeSpan timeToNextReward;

    private readonly string dateTimeFormat = "dd-MM-yyyy HH:mm:ss";

    private bool buttonRefreshed = true;

    private bool calendarRefreshed = false;

    private void Start()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.dailyRewardsDateTime.Equals(string.Empty))
        {
            GenerateCalendar(false);
        }
        else
        {
            GenerateCalendar(true);
        }
    }

    private void OnEnable()
    {
        SetNextClaimTime();
    }

    private void Update()
    {
        CheckAndResetCalendarIfNeeded();
        UpdateUITimeToNextReward();
    }

    private void SetAllButtonsColors()
    {
        SetButtonColor(0, dayOne);
        SetButtonColor(1, dayTwo);
        SetButtonColor(2, dayThree);
        SetButtonColor(3, dayFour);
        SetButtonColor(4, dayFive);
        SetButtonColor(5, daySix);
        SetButtonColor(6, daySeven);
    }

    private void SetButtonColor(int index, Button dayButton)
    {
        if (playerData.isDailyRewardClaimed[index])
        {
            dayButton.GetComponent<Button>().image.color = Color.gray;
        }
        else
        {
            dayButton.GetComponent<Button>().image.color = Color.white;
        }
    }

    public void OnClickOpenDailyRewards()
    {
        dailyRewardsPanel.SetActive(true);
        dailyRewardsButton.gameObject.SetActive(false);
    }

    public void OnClickCloseDailyRewards()
    {
        dailyRewardsPanel.SetActive(false);
        dailyRewardsButton.gameObject.SetActive(true);
    }

    private void GenerateCalendar(bool fromSave)
    {
        if (fromSave)
        {
            GenerateCalendarFromSave();
        }
        else
        {
            GenerateNewCalendar();
        }
    }

    private void SetCalendar()
    {
        SetNextClaimTime();
        CheckRewardsDatesAndSetButtons();
        SetAllButtonsColors();
    }

    private void SaveDateTime()
    {
        playerData.dailyRewardsDateTime = DateTime.Now.ToString(dateTimeFormat);
    }

    private void GenerateCalendarFromSave()
    {
        ConfigureButtonBySavedIndex(dayOne, 0, normalDailyRewards);
        ConfigureButtonBySavedIndex(dayTwo, 1, normalDailyRewards);
        ConfigureButtonBySavedIndex(dayThree, 2, normalDailyRewards);
        ConfigureButtonBySavedIndex(dayFour, 3, normalDailyRewards);
        ConfigureButtonBySavedIndex(dayFive, 4, normalDailyRewards);
        ConfigureButtonBySavedIndex(daySix, 5, normalDailyRewards);
        ConfigureButtonBySavedIndex(daySeven, 6, specialDailyRewards);

        SetCalendar();
    }

    private DailyRewardSO FindRewardByIndex(int index, DailyRewardSO[] dailyRewardsArray)
    {
        DailyRewardSO foundReward = null;

        foreach (var reward in dailyRewardsArray)
        {
            if (reward.index == index)
            {
                foundReward = reward;
                break;
            }
        }

        return foundReward;
    }

    private void ConfigureButtonBySavedIndex(Button dayButton, int dayIndex, DailyRewardSO[] dailyRewardsArray)
    {
        var reward = FindRewardByIndex(playerData.dailyRewardCalendarIndexes[dayIndex], dailyRewardsArray);
        int finalAmount;

        dayButton.GetComponent<Button>().image.sprite = reward.rewardImage;
        dayButton.GetComponent<DailyRewardButton>().DailyRewardIndex = reward.index;

        if (dayIndex != 6)
        {
            finalAmount = reward.rewardAmount * (dayIndex + 1);
        }
        else
        {
            finalAmount = reward.rewardAmount;
        }

        dayButton.GetComponent<DailyRewardButton>().DailyRewardAmount = finalAmount;
        dayButton.GetComponent<DailyRewardButton>().OnButtonAmountText.text = "x" + finalAmount;
    }

    private void GenerateNewCalendar()
    {
        var normalRewardIndexList = MakeListFromRewardIndexes(normalDailyRewards);
        var specialRewardIndexList = MakeListFromRewardIndexes(specialDailyRewards);

        ConfigureButtonByRandomIndex(dayOne, 0, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(dayTwo, 1, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(dayThree, 2, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(dayFour, 3, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(dayFive, 4, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(daySix, 5, GetRandomIndexFromList(normalRewardIndexList), normalDailyRewards);
        ConfigureButtonByRandomIndex(daySeven, 6, GetRandomIndexFromList(specialRewardIndexList), specialDailyRewards);

        SaveDateTime();
        SetCalendar();

        SaveSystem.SavePlayerData(playerData);
    }

    private List<int> MakeListFromRewardIndexes(DailyRewardSO[] dailyRewardsArray)
    {
        List<int> readyList = new();

        foreach (var reward in dailyRewardsArray)
        {
            readyList.Add(reward.index);
        }

        return readyList;
    }

    private int GetRandomIndexFromList(List<int> indexList)
    {
        var randomIndex = Random.Range(0, indexList.Count);
        return indexList[randomIndex];
    }

    private void ConfigureButtonByRandomIndex(Button dayButton, int dayIndex, int index, DailyRewardSO[] dailyRewardsArray)
    {
        var reward = FindRewardByIndex(index, dailyRewardsArray);
        int finalAmount;

        dayButton.GetComponent<DailyRewardButton>().DailyRewardIndex = reward.index;
        dayButton.GetComponent<Button>().image.sprite = reward.rewardImage;

        if (dayIndex != 6)
        {
            finalAmount = reward.rewardAmount * (dayIndex + 1);
        }
        else
        {
            finalAmount = reward.rewardAmount;
        }

        dayButton.GetComponent<DailyRewardButton>().DailyRewardAmount = finalAmount;
        dayButton.GetComponent<DailyRewardButton>().OnButtonAmountText.text = "x" + finalAmount;

        playerData.dailyRewardCalendarIndexes[dayIndex] = reward.index;
        playerData.isDailyRewardClaimed[dayIndex] = false;
    }

    private void SetRewardButtons(TimeSpan daysSinceCalendarCreation)
    {
        if (daysSinceCalendarCreation.TotalDays < 1.0 && !playerData.isDailyRewardClaimed[0] && isAbleToClaimAnotherReward())
        {
            SetButtons(dayOne);
        }
        else if (daysSinceCalendarCreation.TotalDays < 2.0 && !playerData.isDailyRewardClaimed[1] && isAbleToClaimAnotherReward())
        {
            SetButtons(dayTwo);
        }
        else if (daysSinceCalendarCreation.TotalDays < 3.0 && !playerData.isDailyRewardClaimed[2] && isAbleToClaimAnotherReward())
        {
            SetButtons(dayThree);
        }
        else if (daysSinceCalendarCreation.TotalDays < 4.0 && !playerData.isDailyRewardClaimed[3] && isAbleToClaimAnotherReward())
        {
            SetButtons(dayFour);
        }
        else if (daysSinceCalendarCreation.TotalDays < 5.0 && !playerData.isDailyRewardClaimed[4] && isAbleToClaimAnotherReward())
        {
            SetButtons(dayFive);
        }
        else if (daysSinceCalendarCreation.TotalDays < 6.0 && !playerData.isDailyRewardClaimed[5] && isAbleToClaimAnotherReward())
        {
            SetButtons(daySix);
        }
        else if (daysSinceCalendarCreation.TotalDays < 7.0 && !playerData.isDailyRewardClaimed[6] && isAbleToClaimAnotherReward())
        {
            SetButtons(daySeven);
        }
        else
        {
            SetButtons(null);
        }
    }

    private bool isAbleToClaimAnotherReward()
    {
        if (!playerData.dailyRewardsNextClaimDateTime.Equals(string.Empty))
        {
            if (timeToNextReward.TotalSeconds <= 0)
            {
                Debug.Log("Claim reward unlocked (24h passed)");
                return true;
            }
            else
            {
                Debug.Log("Can't claim reward yet");
                return false;
            }
        }
        else
        {
            Debug.Log("Next Claim Date is null - first reward");
            return true;
        }

    }

    private void CheckRewardsDatesAndSetButtons()
    {
        DateTime currentTime = DateTime.Now;

        DateTime calendarCreationDateTime = DateTime.ParseExact(playerData.dailyRewardsDateTime, dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);

        TimeSpan daysSinceCalendarCreation = currentTime.Subtract(calendarCreationDateTime);

        if (daysSinceCalendarCreation.TotalDays > 7.00)
        {
            GenerateCalendar(false);
        }
        else
        {
            SetRewardButtons(daysSinceCalendarCreation);
        }

    }

    private void SetNextClaimTime()
    {
        if (playerData != null && !playerData.dailyRewardsNextClaimDateTime.Equals(string.Empty))
        {
            DateTime currentTime = DateTime.Now;
            timeToNextReward = DateTime.ParseExact(playerData.dailyRewardsNextClaimDateTime, dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture).Subtract(currentTime);
        }
    }

    private void UpdateTimeSpanForNextReward()
    {
        timeToNextReward = timeToNextReward.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
    }

    private void UpdateUITimeToNextReward()
    {
        UpdateTimeSpanForNextReward();

        if (timeToNextReward.TotalSeconds >= 0)
        {
            nextRewardText.text = "Next reward available in: " + timeToNextReward.Hours + "H:" + timeToNextReward.Minutes + "M:" + timeToNextReward.Seconds + "S";
        }
        else
        {
            nextRewardText.text = "Claim your daily reward!";

            if (!buttonRefreshed)
            {
                CheckRewardsDatesAndSetButtons();
                SetAllButtonsColors();
                buttonRefreshed = true;
                Debug.Log("Buttons refreshed because time to next reward is 0");
            }
        }

    }

    private void SetButtons(Button day)
    {
        dayOne.interactable = false;
        dayTwo.interactable = false;
        dayThree.interactable = false;
        dayFour.interactable = false;
        dayFive.interactable = false;
        daySix.interactable = false;
        daySeven.interactable = false;

        if (day != null)
        {
            day.interactable = true;
        }
    }

    private void CheckAndResetCalendarIfNeeded()
    {
        if (!calendarRefreshed && timeToNextReward.TotalHours <= -48)
        {
            calendarRefreshed = true;

            ResetCalendar();

            Debug.Log("Reset Calendar (reward unclaimed in 48h)");
        }

        if (playerData.isDailyRewardClaimed[6] && timeToNextReward.TotalSeconds <= 0)
        {
            ResetCalendar();

            Debug.Log("Reset Calendar (all rewards claimed and 24h passed)");
        }
    }

    private void ResetCalendar()
    {
        playerData.dailyRewardsNextClaimDateTime = string.Empty;
        GenerateCalendar(false);
    }

    public void OnClickDayOne()
    {
        SetButtonAndReward(dayOne, false);
        SaveClaimedReward(0);
    }

    public void OnClickDayTwo()
    {
        SetButtonAndReward(dayTwo, false);
        SaveClaimedReward(1);
    }

    public void OnClickDayThree()
    {
        SetButtonAndReward(dayThree, false);
        SaveClaimedReward(2);
    }

    public void OnClickDayFour()
    {
        SetButtonAndReward(dayFour, false);
        SaveClaimedReward(3);
    }

    public void OnClickDayFive()
    {
        SetButtonAndReward(dayFive, false);
        SaveClaimedReward(4);
    }

    public void OnClickDaySix()
    {
        SetButtonAndReward(daySix, false);
        SaveClaimedReward(5);
    }

    public void OnClickDaySeven()
    {
        SetButtonAndReward(daySeven, true);
        SaveClaimedReward(6);
    }

    private void SetButtonAndReward(Button dayButton, bool specialReward)
    {
        if (dayButton != null)
        {
            dayButton.GetComponent<Button>().interactable = false;
            dayButton.GetComponent<Button>().image.color = Color.gray;

            int rewardIndex = dayButton.GetComponent<DailyRewardButton>().DailyRewardIndex;
            int rewardAmount = dayButton.GetComponent<DailyRewardButton>().DailyRewardAmount;

            if (!specialReward)
            {
                AddNormalRewardForPlayer(rewardIndex, rewardAmount);
            }
            else
            {
                AddSpecialRewardForPlayer(rewardIndex, rewardAmount);
            }

            if (buttonRefreshed)
            {
                buttonRefreshed = false;
            }
        }
    }

    private void SaveClaimedReward(int saveIndex) //0-6 (7 days)
    {
        playerData.isDailyRewardClaimed[saveIndex] = true;
        playerData.dailyRewardsNextClaimDateTime = DateTime.Now.AddHours(24).ToString(dateTimeFormat);
        SetNextClaimTime();
        CheckRewardsDatesAndSetButtons();

        SaveSystem.SavePlayerData(playerData);
    }

    private void AddNormalRewardForPlayer(int rewardIndex, int rewardAmount)
    {
        switch (rewardIndex)
        {
            case 0:
                playerData.totalCoins += rewardAmount;
                break;
            case 1:
                playerData.leftover += rewardAmount;
                break;
            case 2:
                playerData.meds += rewardAmount;
                break;
            case 3:
                playerData.scraps += rewardAmount;
                break;
            default:
                Debug.Log("Something went wrong with: DailyReward -> Add reward");
                break;
        }

        Debug.Log("Added normal reward: " + normalDailyRewards[rewardIndex].rewardName + " x" + rewardAmount);

        SaveSystem.SavePlayerData(playerData);

        UpdateInfoManager();
    }

    private void AddSpecialRewardForPlayer(int rewardIndex, int rewardAmount)
    {
        switch (rewardIndex)
        {
            case 0:
                playerData.drink += rewardAmount;
                break;
            case 1:
                playerData.firstAid += rewardAmount;
                break;
            case 2:
                playerData.gold += rewardAmount;
                break;
            case 3:
                playerData.magnet += rewardAmount;
                break;
            case 4:
                playerData.meal += rewardAmount;
                break;
            case 5:
                playerData.shield += rewardAmount;
                break;
            case 6:
                playerData.stimpack += rewardAmount;
                break;
            default:
                Debug.Log("Something went wrong with: DailyReward (special) -> Add reward");
                break;
        }

        Debug.Log("Added special reward: " + specialDailyRewards[rewardIndex].rewardName + " x" + rewardAmount);

        SaveSystem.SavePlayerData(playerData);

        UpdateInfoManager();
    }

    private void UpdateInfoManager()
    {
        infoManager.UpdateSaveFile();
    }
}
