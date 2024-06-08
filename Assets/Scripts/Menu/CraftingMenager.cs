using UnityEngine;
using UnityEngine.UI;

public class CraftingMenager : MonoBehaviour
{

    [SerializeField]
    private CraftItemSO[] craftItems;

    [SerializeField]
    private GameObject itemsContainer;

    [SerializeField]
    private GameObject prefab;

    public bool isButtonClicked = false;

    public ButtonData buttonData;

    private PlayerData playerData;

    private MissionMenager missionMenager;

    private PopupManager popupManager;

    private InfoManager infoManager;

    private void Start()
    {
        missionMenager = FindObjectOfType<MissionMenager>();
        popupManager = FindObjectOfType<PopupManager>();
        infoManager = FindObjectOfType<InfoManager>();
        SpawnItemsInCointainer();
    }

    private void Update()
    {
        if (isButtonClicked)
        {
            CraftItem();
        }
    }

    private void SpawnItemsInCointainer()
    {
        foreach (var recipe in craftItems)
        {

            GameObject item = Instantiate(prefab, itemsContainer.transform);

            item.GetComponent<ItemConfig>().costItem = recipe.costItem;
            item.GetComponent<ItemConfig>().costItemName = recipe.costItemName;
            item.GetComponent<ItemConfig>().craftedItemAmount = recipe.craftedItemAmount;
            item.GetComponent<ItemConfig>().craftedItemName = recipe.craftedItemName;

            foreach (Transform child in item.transform)
            {
                if (child.name.Equals("SpriteUsedItem"))
                {
                    child.GetComponent<Image>().sprite = recipe.spriteUsedItem;
                }
                else if (child.name.Equals("SpriteCraftedItem"))
                {
                    child.GetComponent<Image>().sprite = recipe.spriteCraftedItem;
                }
            }

        }
    }

    private void UpdateQuestsIfFindAny(string itemName)
    {

        if (missionMenager.Quests[0].GetComponent<QuestConfig>().allQuestData.requirements.craftItemName.Equals(itemName))
        {
            playerData.quest1Value--;
        }

        if (missionMenager.Quests[1].GetComponent<QuestConfig>().allQuestData.requirements.craftItemName.Equals(itemName))
        {
            playerData.quest2Value--;
        }

        if (missionMenager.Quests[2].GetComponent<QuestConfig>().allQuestData.requirements.craftItemName.Equals(itemName))
        {
            playerData.quest3Value--;
        }

    }

    private void CraftItem()
    {
        playerData = SaveSystem.LoadPlayerData();

        string itemCostName = buttonData.costItemName;
        string itemToCraftName = buttonData.craftedItemName;

        switch (itemCostName)
        {
            case "Meds":
                if (playerData.meds >= buttonData.costItem && itemToCraftName.Equals("FirstAid"))
                {
                    playerData.meds -= buttonData.costItem;
                    playerData.firstAid++;
                    UpdateQuestsIfFindAny("FirstAid");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: First Aid\nTotal: " + playerData.firstAid);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else if (playerData.meds >= buttonData.costItem && itemToCraftName.Equals("Stimpack"))
                {
                    playerData.meds -= buttonData.costItem;
                    playerData.stimpack++;
                    UpdateQuestsIfFindAny("Stimpack");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: Stimpack\nTotal: " + playerData.stimpack);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else
                {
                    popupManager.CreateAlertPopup(1, "Not enough resources!");
                }
                break;

            case "Leftovers":
                if (playerData.leftover >= buttonData.costItem && itemToCraftName.Equals("Meal"))
                {
                    playerData.leftover -= buttonData.costItem;
                    playerData.meal++;
                    UpdateQuestsIfFindAny("Meal");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: Meal\nTotal: " + playerData.meal);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else if (playerData.leftover >= buttonData.costItem && itemToCraftName.Equals("Drink"))
                {
                    playerData.leftover -= buttonData.costItem;
                    playerData.drink++;
                    UpdateQuestsIfFindAny("Drink");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: Drink\nTotal: " + playerData.drink);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else
                {
                    popupManager.CreateAlertPopup(1, "Not enough resources!");
                }
                break;

            case "Scraps":
                if (playerData.scraps >= buttonData.costItem && itemToCraftName.Equals("Shield"))
                {
                    playerData.scraps -= buttonData.costItem;
                    playerData.shield++;
                    UpdateQuestsIfFindAny("Shield");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: Shield\nTotal: " + playerData.shield);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else if (playerData.scraps >= buttonData.costItem && itemToCraftName.Equals("Magnet"))
                {
                    playerData.scraps -= buttonData.costItem;
                    playerData.magnet++;
                    UpdateQuestsIfFindAny("Magnet");
                    AddValueToCraftingAchievement();
                    popupManager.CreateAlertPopup(2, "Crafted: Magnet\nTotal: " + playerData.magnet);
                    SaveSystem.SavePlayerData(playerData);
                    UpdateSaveFileForInfoManager();
                }
                else
                {
                    popupManager.CreateAlertPopup(1, "Not enough resources!");
                }
                break;
        }

        //Dla testów
        Debug.Log("Stan zasobów (meds, leftovers, scraps): " + playerData.meds + " / " + playerData.leftover + " / " + playerData.scraps);
        Debug.Log("Stan itemów (firstAid, stimpack, meal, drink, shield, magnet): " +
            playerData.firstAid + " / " + playerData.stimpack + " / " + playerData.meal + " / " + playerData.drink +
            " / " + playerData.shield + " / " + playerData.magnet);

        isButtonClicked = false;
    }

    private void AddValueToCraftingAchievement()
    {
        playerData.achievementValue[14]++;
    }

    private void UpdateSaveFileForInfoManager()
    {
        infoManager.UpdateSaveFile();
    }

}
