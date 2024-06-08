using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private SkinMenager skinMenager;

    [SerializeField]
    private GameStatesSO gameStates;

    private MissionMenager missionMenager;

    private AchievementManager achievementManager;

    [SerializeField]
    private Button dailyRewardsButton;

    #region ShopPanels

    [SerializeField]
    private GameObject shopPanel;

    [SerializeField]
    private GameObject shoppingUpperPanel;

    [SerializeField]
    private GameObject shopSkinsUpperPanel;

    [SerializeField]
    private GameObject specialOffersPanel;
    [SerializeField]
    private GameObject buyGoldPanel;
    [SerializeField]
    private GameObject itemsPanel;

    [SerializeField]
    private GameObject shopPlayerSkinsPanel;
    [SerializeField]
    private GameObject shopGunSkinsPanel;

    #endregion

    #region ShopButtons

    [SerializeField]
    private GameObject shoppingButton;
    [SerializeField]
    private GameObject shopSkinsButton;

    [SerializeField]
    private GameObject buySpecialOffersButton;
    [SerializeField]
    private GameObject buyGoldButton;
    [SerializeField]
    private GameObject buyItemsButton;

    [SerializeField]
    private GameObject buyPlayerSkinsButton;
    [SerializeField]
    private GameObject buyGunSkinsButton;

    [SerializeField]
    private Button buyPlayerSkinButton;

    [SerializeField]
    private Button buyGunSkinButton;

    #endregion

    #region ShopTexts

    [SerializeField]
    private TextMeshProUGUI buyPlayerSkinButtonText;

    [SerializeField]
    private TextMeshProUGUI buyGunSkinButtonText;

    #endregion

    #region ProfilePanels

    [SerializeField]
    private GameObject profilePanel;

    [SerializeField]
    private GameObject youUpperPanel;
    [SerializeField]
    private GameObject progressUpperPanel;

    [SerializeField]
    private GameObject yourSkinsPanel;
    [SerializeField]
    private GameObject craftingPanel;

    [SerializeField]
    private GameObject missionsPanel;
    [SerializeField]
    private GameObject achievementsPanel;

    #endregion

    #region ProfileButtons

    [SerializeField]
    private GameObject youButton;
    [SerializeField]
    private GameObject progressButton;

    [SerializeField]
    private GameObject profileSkinsButton;
    [SerializeField]
    private GameObject craftingButton;

    [SerializeField]
    private GameObject missionsButton;
    [SerializeField]
    private GameObject achievementsButton;

    [SerializeField]
    private Button selectPlayerSkinButton;

    [SerializeField]
    private Button selectGunSkinButton;



    #endregion

    #region ProfileTexts

    [SerializeField]
    private TextMeshProUGUI selectPlayerSkinButtonText;

    [SerializeField]
    private TextMeshProUGUI selectGunSkinButtonText;

    #endregion

    #region HowToPlay

    [SerializeField]
    private GameObject firstStartPanel;

    [SerializeField]
    private Button howToPlayButton;

    [SerializeField]
    private GameObject howToPlayPanel;

    #endregion

    #region Settings

    [SerializeField]
    private GameObject settingsPanel;

    #endregion

    public void Start()
    {
        Debug.Log("MENU_MANAGER: Working!");
        skinMenager.UpdateSkinsData();
        skinMenager.UpdateSkins();
        missionMenager = FindObjectOfType<MissionMenager>();
        achievementManager = FindObjectOfType<AchievementManager>();

        AudioManager.Instance.onAudioManagerReady.AddListener(PlayStartMusic);

        if (AudioManager.Instance != null && AudioManager.Instance.characterSfxSource.isPlaying)
        {
            AudioManager.Instance.characterSfxSource.Stop();
        }

    }

    private void PlayStartMusic()
    {
        if (!AudioManager.Instance.musicSource.isPlaying || !AudioManager.Instance.musicSource.clip.name.Equals("Menu"))
        {
            AudioManager.Instance.PlayMusic("Menu");
        }
    }

    private void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("Click");
    }

    public void PlayGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMainMenu()
    {
        PlayClickSound();
        ResetAllMainPanels();
        dailyRewardsButton.gameObject.SetActive(true);
    }

    public void SettingsPanel()
    {
        PlayClickSound();
        settingsPanel.SetActive(true);
        dailyRewardsButton.gameObject.SetActive(false);
    }

    public void HowToPlayPanel()
    {
        PlayClickSound();
        gameStates.isThisTutorial = true;
        SceneManager.LoadScene("Game");
    }

    public void Shop()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(true);

        BuySpecialOffers();
    }

    public void BuySpecialOffers()
    {
        PlayClickSound();
        SetShopPanel(specialOffersPanel, true, buySpecialOffersButton, shoppingButton);
    }

    public void BuyGold()
    {
        PlayClickSound();
        SetShopPanel(buyGoldPanel, true, buyGoldButton, shoppingButton);
    }

    public void BuyItems()
    {
        PlayClickSound();
        SetShopPanel(itemsPanel, true, buyItemsButton, shoppingButton);
    }

    public void BuyPlayerSkins()
    {
        PlayClickSound();
        SetShopPanel(shopPlayerSkinsPanel, false, shopSkinsButton, buyPlayerSkinsButton);
        skinMenager.UpdateSkinsData();
        skinMenager.ShopShowPlayerSkin();
        BuyPlayerSkinButtonText();
    }

    public void ShopNextPlayerSkin()
    {
        PlayClickSound();
        skinMenager.ShopNextPlayerSkin();
        BuyPlayerSkinButtonText();
    }

    public void ShopPrevPlayerSkin()
    {
        PlayClickSound();
        skinMenager.ShopPrevPlayerSkin();
        BuyPlayerSkinButtonText();
    }

    public void BuyPlayerSkinButtonText()
    {
        if (skinMenager.GetPlayerSkinIsOwnedOnCurrentIndex())
        {
            buyPlayerSkinButtonText.text = "OWNED";
            buyPlayerSkinButton.interactable = false;
        }
        else
        {
            buyPlayerSkinButton.interactable = true;
            buyPlayerSkinButtonText.text = "BUY FOR " + skinMenager.GetPlayerSkinCostOnCurrentIndex() + " GOLD";
        }
    }

    public void BuyPlayerSkinButton()
    {
        PlayClickSound();
        skinMenager.BuyPlayerSkinOnCurrentIndex();
        BuyPlayerSkinButtonText();
    }

    public void BuyGunSkins()
    {
        PlayClickSound();
        SetShopPanel(shopGunSkinsPanel, false, shopSkinsButton, buyGunSkinsButton);
        skinMenager.UpdateSkinsData();
        skinMenager.ShopShowGunSkin();
        BuyGunSkinButtonText();
    }

    public void ShopNextGunSkin()
    {
        PlayClickSound();
        skinMenager.ShopNextGunSkin();
        BuyGunSkinButtonText();
    }

    public void ShopPrevGunSkin()
    {
        PlayClickSound();
        skinMenager.ShopPrevGunSkin();
        BuyGunSkinButtonText();
    }

    public void BuyGunSkinButtonText()
    {
        if (skinMenager.GetGunSkinIsOwnedOnCurrentIndex())
        {
            buyGunSkinButtonText.text = "OWNED";
            buyGunSkinButton.interactable = false;
        }
        else
        {
            buyGunSkinButton.interactable = true;
            buyGunSkinButtonText.text = "BUY FOR " + skinMenager.GetGunSkinCostOnCurrentIndex() + " GOLD";
        }
    }

    public void BuyGunSkinButton()
    {
        PlayClickSound();
        skinMenager.BuyGunSkinOnCurrentIndex();
        BuyGunSkinButtonText();
    }

    public void Profile()
    {
        mainMenuPanel.SetActive(false);
        profilePanel.SetActive(true);

        ProfileSkins();
    }

    public void ProfileSkins()
    {
        PlayClickSound();
        SetProfilePanel(yourSkinsPanel, true, youButton, profileSkinsButton);
        skinMenager.ResetPanel();
        skinMenager.UpdateSkinsData();
        skinMenager.ProfileShowSkins();
        SetProfileSelectPlayerSkinButton();
        SetProfileSelectGunSkinButton();
    }

    public void ProfileNextPlayerSkin()
    {
        PlayClickSound();
        skinMenager.ProfileNextPlayerSkin();
        SetProfileSelectPlayerSkinButton();
    }

    public void ProfilePrevPlayerSkin()
    {
        PlayClickSound();
        skinMenager.ProfilePrevPlayerSkin();
        SetProfileSelectPlayerSkinButton();
    }

    public void ProfileNextGunSkin()
    {
        PlayClickSound();
        skinMenager.ProfileNextGunSkin();
        SetProfileSelectGunSkinButton();
    }

    public void ProfilePrevGunSkin()
    {
        PlayClickSound();
        skinMenager.ProfilePrevGunSkin();
        SetProfileSelectGunSkinButton();
    }

    public void ProfileSelectPlayerSkin()
    {
        PlayClickSound();
        skinMenager.ConfirmSelectedPlayerSkin();
        SetProfileSelectPlayerSkinButton();
    }

    public void ProfileSelectGunSkin()
    {
        PlayClickSound();
        skinMenager.ConfirmSelectedGunSkin();
        SetProfileSelectGunSkinButton();
    }

    public void ProfileCrafting()
    {
        PlayClickSound();
        SetProfilePanel(craftingPanel, true, youButton, craftingButton);
    }

    public void ProfileMissions()
    {
        PlayClickSound();
        missionMenager.RebuildMissions();
        SetProfilePanel(missionsPanel, false, progressButton, missionsButton);
    }

    public void ProfileAchievements()
    {
        PlayClickSound();
        achievementManager.RebuildAchievements();
        SetProfilePanel(achievementsPanel, false, progressButton, achievementsButton);
    }

    private void SetProfileSelectPlayerSkinButton()
    {
        if (skinMenager.IsCurrentPlayerSkinSelected())
        {
            selectPlayerSkinButtonText.text = "SELECTED";
            selectPlayerSkinButton.interactable = false;
        }
        else
        {
            selectPlayerSkinButtonText.text = "SELECT SKIN";
            selectPlayerSkinButton.interactable = true;
        }
    }

    private void SetProfileSelectGunSkinButton()
    {
        if (skinMenager.IsCurrentGunSkinSelected())
        {
            selectGunSkinButtonText.text = "SELECTED";
            selectGunSkinButton.interactable = false;
        }
        else
        {
            selectGunSkinButtonText.text = "SELECT SKIN";
            selectGunSkinButton.interactable = true;
        }
    }

    private void SetShopPanel(GameObject selectedPanel, bool shoppingSection, params GameObject[] buttons)
    {
        if (shoppingSection)
        {
            shopSkinsUpperPanel.SetActive(false);
            shoppingUpperPanel.SetActive(true);
        }
        else
        {
            shoppingUpperPanel.SetActive(false);
            shopSkinsUpperPanel.SetActive(true);
        }

        ResetMainPanels(true);
        ChangeButtonsColor(true, buttons);
        selectedPanel.SetActive(true);
    }

    private void SetProfilePanel(GameObject selectedPanel, bool youSection, params GameObject[] buttons)
    {
        if (youSection)
        {
            progressUpperPanel.SetActive(false);
            youUpperPanel.SetActive(true);
        }
        else
        {
            youUpperPanel.SetActive(false);
            progressUpperPanel.SetActive(true);
        }

        ResetMainPanels(false);
        ChangeButtonsColor(false, buttons);
        selectedPanel.SetActive(true);
    }

    private void ResetMainPanels(bool isShopSection)
    {
        if (isShopSection)
        {
            shopPlayerSkinsPanel.SetActive(false);
            shopGunSkinsPanel.SetActive(false);
            buyGoldPanel.SetActive(false);
            itemsPanel.SetActive(false);
            specialOffersPanel.SetActive(false);
        }
        else
        {
            yourSkinsPanel.SetActive(false);
            craftingPanel.SetActive(false);
            missionsPanel.SetActive(false);
            achievementsPanel.SetActive(false);
        }
        skinMenager.ResetSkinsOnQuit();
        dailyRewardsButton.gameObject.SetActive(false);
    }

    private void ResetAllUpperPanels()
    {
        youUpperPanel.SetActive(false);
        progressUpperPanel.SetActive(false);
        shoppingUpperPanel.SetActive(false);
        shopSkinsUpperPanel.SetActive(false);
    }

    private void ResetAllMainPanels()
    {
        ResetMainPanels(true);
        ResetMainPanels(false);
        ResetAllUpperPanels();
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);

        craftingPanel.GetComponentInChildren<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        achievementsPanel.GetComponentInChildren<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        specialOffersPanel.GetComponentInChildren<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        howToPlayPanel.GetComponentInChildren<ScrollRect>().normalizedPosition = new Vector2(0, 1);

        skinMenager.UpdateSkinsData();
        skinMenager.UpdateSkins();

        mainMenuPanel.SetActive(true);
    }

    private void ChangeButtonsColor(bool shopSection, params GameObject[] buttons)
    {
        if (shopSection)
        {
            buySpecialOffersButton.GetComponent<Image>().color = Color.white;
            buyGoldButton.GetComponent<Image>().color = Color.white;
            buyItemsButton.GetComponent<Image>().color = Color.white;
            shopSkinsButton.GetComponent<Image>().color = Color.white;
            shoppingButton.GetComponent<Image>().color = Color.white;
            buyPlayerSkinsButton.GetComponent<Image>().color = Color.white;
            buyGunSkinsButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            youButton.GetComponent<Image>().color = Color.white;
            progressButton.GetComponent<Image>().color = Color.white;
            profileSkinsButton.GetComponent<Image>().color = Color.white;
            craftingButton.GetComponent<Image>().color = Color.white;
            missionsButton.GetComponent<Image>().color = Color.white;
            achievementsButton.GetComponent<Image>().color = Color.white;
        }

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = Color.gray;
        }
    }
}
