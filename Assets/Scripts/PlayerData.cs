[System.Serializable]
public class PlayerData
{
    public bool firstStart;

    public string playerName;

    public int gold;
    public int totalCoins;
    public int stars;

    public int bestScore;

    public int leftover;
    public int meds;
    public int scraps;

    public int firstAid;
    public int stimpack;
    public int meal;
    public int drink;
    public int shield;
    public int magnet;

    public int maxAmmo;

    public bool[] ownedPlayerSkins;
    public bool[] ownedGunSkins;

    public int selectedPlayerSkinIndex;
    public int selectedGunSkinIndex;

    #region Quests

    public int quest1requirementsIndex;
    public int quest2requirementsIndex;
    public int quest3requirementsIndex;

    public int quest1rewardIndex;
    public int quest2rewardIndex;
    public int quest3rewardIndex;

    public int quest1multiplier;
    public int quest2multiplier;
    public int quest3multiplier;

    public string quest1ValueName;
    public string quest2ValueName;
    public string quest3ValueName;

    public int quest1Value;
    public int quest2Value;
    public int quest3Value;

    public bool areQuestsCreated;

    public bool isQuest1Completed;
    public bool isQuest2Completed;
    public bool isQuest3Completed;

    public bool isQuest1RewardClaimed;
    public bool isQuest2RewardClaimed;
    public bool isQuest3RewardClaimed;

    public string questsDateTime;

    #endregion

    #region Achievements

    public int[] achievementValue;
    public int[] achievementLastRewardClaimed;
    public int[] achievementCurrentLevel;

    #endregion

    public bool isMusicMuted;
    public bool isSFXMuted;

    public string dailyRewardsDateTime;
    public string dailyRewardsNextClaimDateTime;
    public bool[] isDailyRewardClaimed;
    public int[] dailyRewardCalendarIndexes;

    public PlayerData()
    {
        firstStart = true;
        playerName = string.Empty;
        gold = 0;
        totalCoins = 0;
        stars = 0;
        bestScore = 0;
        leftover = 0;
        meds = 0;
        scraps = 0;
        firstAid = 0;
        stimpack = 0;
        meal = 0;
        drink = 0;
        shield = 0;
        magnet = 0;
        maxAmmo = 10;
        ownedPlayerSkins = new bool[5];
        ownedPlayerSkins[0] = true;
        ownedPlayerSkins[1] = false;
        ownedPlayerSkins[2] = false;
        ownedPlayerSkins[3] = false;
        ownedPlayerSkins[4] = false;
        ownedGunSkins = new bool[5];
        ownedGunSkins[0] = true;
        ownedGunSkins[1] = false;
        ownedGunSkins[2] = false;
        ownedGunSkins[3] = false;
        ownedGunSkins[4] = false;
        selectedPlayerSkinIndex = 0;
        selectedGunSkinIndex = 0;

        selectedPlayerSkinIndex = 0;
        selectedGunSkinIndex = 0;

        quest1requirementsIndex = 0;
        quest2requirementsIndex = 0;
        quest3requirementsIndex = 0;

        quest1rewardIndex = 0;
        quest2rewardIndex = 0;
        quest3rewardIndex = 0;

        quest1multiplier = 0;
        quest2multiplier = 0;
        quest3multiplier = 0;

        quest1ValueName = string.Empty;
        quest2ValueName = string.Empty;
        quest3ValueName = string.Empty;

        quest1Value = 0;
        quest2Value = 0;
        quest3Value = 0;

        areQuestsCreated = false;

        isQuest1Completed = false;
        isQuest2Completed = false;
        isQuest3Completed = false;

        isQuest1RewardClaimed = false;
        isQuest2RewardClaimed = false;
        isQuest3RewardClaimed = false;

        questsDateTime = string.Empty;

        achievementValue = new int[16];
        achievementLastRewardClaimed = new int[16];
        for (int i = 0; i < achievementLastRewardClaimed.Length; i++)
        {
            achievementLastRewardClaimed[i] = -1;
        }
        achievementCurrentLevel = new int[16];

        isMusicMuted = false;
        isSFXMuted = false;

        dailyRewardsDateTime = string.Empty;
        dailyRewardsNextClaimDateTime = string.Empty;
        isDailyRewardClaimed = new bool[7];
        dailyRewardCalendarIndexes = new int[7];
    }
    public PlayerData(PlayerData playerData)
    {
        firstStart = playerData.firstStart;
        playerName = playerData.playerName;
        gold = playerData.gold;
        totalCoins = playerData.totalCoins;
        stars = playerData.stars;
        bestScore = playerData.bestScore;
        leftover = playerData.leftover;
        meds = playerData.meds;
        scraps = playerData.scraps;
        firstAid = playerData.firstAid;
        stimpack = playerData.stimpack;
        meal = playerData.meal;
        drink = playerData.drink;
        shield = playerData.shield;
        magnet = playerData.magnet;
        maxAmmo = playerData.maxAmmo;
        ownedPlayerSkins = playerData.ownedPlayerSkins;
        ownedGunSkins = playerData.ownedGunSkins;
        selectedPlayerSkinIndex = playerData.selectedPlayerSkinIndex;
        selectedGunSkinIndex = playerData.selectedGunSkinIndex;

        quest1requirementsIndex = playerData.quest1requirementsIndex;
        quest2requirementsIndex = playerData.quest2requirementsIndex;
        quest3requirementsIndex = playerData.quest3requirementsIndex;

        quest1rewardIndex = playerData.quest1rewardIndex;
        quest2rewardIndex = playerData.quest2rewardIndex;
        quest3rewardIndex = playerData.quest3rewardIndex;

        quest1multiplier = playerData.quest1multiplier;
        quest2multiplier = playerData.quest2multiplier;
        quest3multiplier = playerData.quest3multiplier;

        quest1ValueName = playerData.quest1ValueName;
        quest2ValueName = playerData.quest2ValueName;
        quest3ValueName = playerData.quest3ValueName;

        quest1Value = playerData.quest1Value;
        quest2Value = playerData.quest2Value;
        quest3Value = playerData.quest3Value;

        areQuestsCreated = playerData.areQuestsCreated;

        isQuest1Completed = playerData.isQuest1Completed;
        isQuest2Completed = playerData.isQuest2Completed;
        isQuest3Completed = playerData.isQuest3Completed;

        isQuest1RewardClaimed = playerData.isQuest1RewardClaimed;
        isQuest2RewardClaimed = playerData.isQuest2RewardClaimed;
        isQuest3RewardClaimed = playerData.isQuest3RewardClaimed;

        questsDateTime = playerData.questsDateTime;

        achievementValue = playerData.achievementValue;
        achievementLastRewardClaimed = playerData.achievementLastRewardClaimed;
        achievementCurrentLevel = playerData.achievementCurrentLevel;

        isMusicMuted = playerData.isMusicMuted;
        isSFXMuted = playerData.isSFXMuted;

        dailyRewardsDateTime = playerData.dailyRewardsDateTime;
        dailyRewardsNextClaimDateTime = playerData.dailyRewardsNextClaimDateTime;
        isDailyRewardClaimed = playerData.isDailyRewardClaimed;
        dailyRewardCalendarIndexes = playerData.dailyRewardCalendarIndexes;
    }
}