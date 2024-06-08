using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{

    private PlayerData playerData;

    [SerializeField]
    private TextMeshProUGUI buyItemGoldText;

    [SerializeField]
    private TextMeshProUGUI buyPlayerSkinGoldText;

    [SerializeField]
    private TextMeshProUGUI buyGunSkinGoldText;

    [SerializeField]
    private TextMeshProUGUI medsAmountText;

    [SerializeField]
    private TextMeshProUGUI leftoversAmountText;

    [SerializeField]
    private TextMeshProUGUI scrapsAmountText;

    [SerializeField]
    private TextMeshProUGUI firstAidText;

    [SerializeField]
    private TextMeshProUGUI stimpackText;

    [SerializeField]
    private TextMeshProUGUI mealText;

    [SerializeField]
    private TextMeshProUGUI drinkText;

    [SerializeField]
    private TextMeshProUGUI shieldText;

    [SerializeField]
    private TextMeshProUGUI magnetText;

    [SerializeField]
    private TextMeshProUGUI starsText;

    [SerializeField]
    private AchievementSO[] achievements;

    private int totalStars;



    void Start()
    {
        playerData = SaveSystem.LoadPlayerData();
        CalculateTotalAchievementsStars();
    }

    void Update()
    {
        buyItemGoldText.text = playerData.gold.ToString();
        buyPlayerSkinGoldText.text = playerData.gold.ToString();
        buyGunSkinGoldText.text = playerData.gold.ToString();

        medsAmountText.text = playerData.meds.ToString();
        leftoversAmountText.text = playerData.leftover.ToString();
        scrapsAmountText.text = playerData.scraps.ToString();

        firstAidText.text = playerData.firstAid.ToString();
        stimpackText.text = playerData.stimpack.ToString();
        mealText.text = playerData.meal.ToString();
        drinkText.text = playerData.drink.ToString();
        shieldText.text = playerData.shield.ToString();
        magnetText.text = playerData.magnet.ToString();

        starsText.text = playerData.stars.ToString() + " / " + totalStars;
    }

    private void CalculateTotalAchievementsStars()
    {
        totalStars = 0;

        foreach (var achievement in achievements)
        {
            foreach (var reward in achievement.rewards)
            {
                totalStars += reward;
            }
        }
    }

    internal void UpdateSaveFile()
    {
        playerData = SaveSystem.LoadPlayerData();
    }
}
