using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private AchievementSO[] allAchievements;

    private GameObject[] achievementsObjects;

    private PlayerData playerData;

    private PopupManager popupManager;

    void Start()
    {
        Debug.Log("ACHIEVEMENT_MANAGER: Working!");
        popupManager = FindObjectOfType<PopupManager>();
        playerData = SaveSystem.LoadPlayerData();
        SpawnAchievementsInContainer();

    }

    private void SpawnAchievementsInContainer()
    {
        Debug.Log("ACHIEVEMENT_MANAGER: Creating achievements");

        achievementsObjects = new GameObject[allAchievements.Length];

        for (int i = 0; i < allAchievements.Length; i++)
        {
            var achievement = Instantiate(prefab, container.transform);

            achievement.GetComponent<AchievementConfig>().achievement = allAchievements[i].Clone();

            achievementsObjects[i] = achievement;
        }

        UpdateStarsAndSaveData(achievementsObjects);
        UpdateLevelText(achievementsObjects);
        UpdateDescription(achievementsObjects);

    }

    private void UpdateLevelText(GameObject[] createdAchievements)
    {
        foreach (var achievement in createdAchievements)
        {
            int index = achievement.GetComponent<AchievementConfig>().achievement.index;

            achievement.GetComponent<AchievementConfig>().currentAndMaxLevel.text += playerData.achievementCurrentLevel[index] +
            "/" + achievement.GetComponent<AchievementConfig>().achievement.tasks.Length;
        }
    }

    private void UpdateDescription(GameObject[] createdAchievements)
    {
        for (int i = 0; i < createdAchievements.Length; i++)
        {
            if (playerData.achievementCurrentLevel[i] >= createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks.Length
                && createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks.Length != 1)
            {
                createdAchievements[i].GetComponent<AchievementConfig>().achievement.description
               += "OVERFLOW! (" + playerData.achievementValue[i] + ")";
            }
            else if (createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks.Length > 1)
            {
                createdAchievements[i].GetComponent<AchievementConfig>().achievement.description
                += playerData.achievementValue[i] + "/"
                + createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks[playerData.achievementCurrentLevel[i]];
            }
        }
    }

    private void UpdateStarsAndSaveData(GameObject[] createdAchievements)
    {

        for (int i = 0; i < createdAchievements.Length; i++)
        {
            for (int j = 0; j < createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks.Length; j++)
            {
                if (playerData.achievementValue[i] >= createdAchievements[i].GetComponent<AchievementConfig>().achievement.tasks[j]
                    && playerData.achievementLastRewardClaimed[i] < j)
                {
                    playerData.stars += createdAchievements[i].GetComponent<AchievementConfig>().achievement.rewards[j];
                    playerData.achievementLastRewardClaimed[i] = j;

                    playerData.achievementCurrentLevel[i] = j + 1;

                    popupManager.CreateAchievementPopup(0,
                        "New achievement stage!",
                        "You have completed stage "
                        + j
                        + " of \""
                        + createdAchievements[i].GetComponent<AchievementConfig>().achievement.title
                        + "\" achievement!");
                }
            }
        }
        SaveSystem.SavePlayerData(playerData);
        Debug.Log("ACHIEVEMENT_MANAGER: Current stars: " + playerData.stars);
    }

    internal void RebuildAchievements()
    {
        Debug.Log("ACHIEVEMENT_MANAGER: Invoke -> Create achievements again!");

        foreach (var ach in achievementsObjects)
        {
            Destroy(ach);
        }

        playerData = SaveSystem.LoadPlayerData();

        SpawnAchievementsInContainer();
    }
}
