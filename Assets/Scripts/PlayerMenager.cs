using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenager : MonoBehaviour
{
    #region PlayerDataForRun
    private float scoreMultiplier = 1;
    private int scoreBonus = 0;
    private float stimpackTime = 0;
    private float magnetTime = 0;
    private bool shieldActive = false;
    #endregion

    #region Game Panels
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject inGamePanel;
    [SerializeField]
    private GameObject gameStartPanel;
    #endregion

    #region SO
    [SerializeField]
    private InGameInfoSO inGameInfo;
    [SerializeField]
    private GameStatesSO gameStates;
    #endregion

    #region UI
    [SerializeField]
    private TextMeshProUGUI coinsTotalText;
    [SerializeField]
    private TextMeshProUGUI coinsThisRunText;
    [SerializeField]
    private TextMeshProUGUI coinsCollectedText;
    [SerializeField]
    private TextMeshProUGUI scoreTextInGamePanel;
    [SerializeField]
    private TextMeshProUGUI scoreThisRunText;
    [SerializeField]
    private TextMeshProUGUI scoreBestText;
    [SerializeField]
    private TextMeshProUGUI newRecordText;
    [SerializeField]
    private TextMeshProUGUI fireButtonText;
    [SerializeField]
    private TextMeshProUGUI medsText;
    [SerializeField]
    private TextMeshProUGUI leftoversText;
    [SerializeField]
    private TextMeshProUGUI scrapsText;
    [SerializeField]
    private Button mealButton;
    [SerializeField]
    private Button drinkButton;
    [SerializeField]
    private TextMeshProUGUI mealsOwned;
    [SerializeField]
    private TextMeshProUGUI drinksOwned;
    [SerializeField]
    private TextMeshProUGUI mealCurrentBoost;
    [SerializeField]
    private TextMeshProUGUI drinkCurrentBoost;
    [SerializeField]
    private Button firstAidButton;
    [SerializeField]
    private TextMeshProUGUI firstAidButtonText;
    [SerializeField]
    private GameObject tempGhostImage;
    #endregion

    #region ItemsPanel

    [SerializeField]
    private GameObject itemPanel;
    [SerializeField]
    private Button stimpackButton;
    [SerializeField]
    private Button magnetButton;
    [SerializeField]
    private Button shieldButton;
    [SerializeField]
    private Button showHideButton;
    [SerializeField]
    private TextMeshProUGUI stimpackCooldown;
    [SerializeField]
    private TextMeshProUGUI stimpackQuantity;
    [SerializeField]
    private TextMeshProUGUI magnetCooldown;
    [SerializeField]
    private TextMeshProUGUI magnetQuantity;
    [SerializeField]
    private TextMeshProUGUI shieldCooldown;
    [SerializeField]
    private TextMeshProUGUI shieldQuantity;
    private bool isStimpackOnCooldown;
    private float stimpackCooldownValue;
    private bool isMagnetOnCooldown;
    private float magnetCooldownValue;
    private bool isShieldOnCooldown;
    private float shieldkCooldownValue;

    #endregion

    #region Achievements Data

    private int allInOneCount = 0;

    private bool allInOneFirstAid = false;
    private bool allInOneStimpack = false;
    private bool allInOneMeal = false;
    private bool allInOneDrink = false;
    private bool allInOneShield = false;
    private bool allInOneMagnet = false;

    private int imBuiltDifferentFirstAid;
    private int imBuiltDifferentStimpack;

    private bool drinkAndLose = false;

    private int shieldCount;

    private float noResurcesTime;
    private bool noResourcesCompleted;

    #endregion

    private PlayerController playerController;

    private float offset;

    [SerializeField]
    private SwipeMenager swipeMenager;
    private bool isDataSaved = false;

    [SerializeField]
    private SkinMenager skinMenager;

    private TileMenager tileMenager;

    private bool ghostMode = false;

    internal PlayerData mainPlayerData;

    private int prevCoins;

    private int prevEnemiesScore;

    private bool isMusicStarted;

    void Start()
    {
        if (gameStates.isThisTutorial)
        {
            gameStartPanel.SetActive(false);
        }

        if (!AudioManager.Instance.musicSource.clip.name.Equals("Pause"))
        {
            AudioManager.Instance.PlayMusic("Pause");
        }
        skinMenager.InGameShowSkins();
        playerController = FindObjectOfType<PlayerController>();
        tileMenager = FindObjectOfType<TileMenager>();
        ResetValues();
        offset = Mathf.Abs(Vector3.zero.z - playerController.transform.position.z);
        HideGameOverPanel();
        mainPlayerData = SaveSystem.LoadPlayerData();
        gameStates.isThisMenuScene = false;
    }

    void Update()
    {
        StartMusic();
        ShowGameOverPanel();
        if (gameStartPanel != null)
        {
            UpdateGameStartScreen();
        }
        UpdateCollectedCoins();
        UpdateScore();
        UpdateFireButtonText();
        UpdateItemUI();
        UpdateItemQuantity();
        UpdateNoResourcesTimer();

    }

    private void FixedUpdate()
    {
        if (isStimpackOnCooldown && stimpackTime > 0)
        {
            stimpackTime -= Time.fixedDeltaTime;

            if (gameStates.isThisTutorial && stimpackTime < 1)
            {
                inGameInfo.tutorialCounter = 1;
            }
        }
        else if (isStimpackOnCooldown && stimpackTime <= 0)
        {
            stimpackCooldownValue -= Time.fixedDeltaTime;

            if (stimpackCooldownValue <= 0)
            {
                isStimpackOnCooldown = false;
            }
        }


        if (isMagnetOnCooldown && magnetTime > 0)
        {
            magnetTime -= Time.fixedDeltaTime;

            if (gameStates.isThisTutorial && magnetTime < 1)
            {
                inGameInfo.tutorialCounter = 1;
            }
        }
        else if (isMagnetOnCooldown && magnetTime <= 0)
        {
            magnetCooldownValue -= Time.fixedDeltaTime;

            if (magnetCooldownValue <= 0)
            {
                isMagnetOnCooldown = false;
            }
        }

        if (isShieldOnCooldown && !shieldActive)
        {
            shieldkCooldownValue -= Time.fixedDeltaTime;
            if (shieldkCooldownValue <= 0)
            {
                isShieldOnCooldown = false;
            }
        }

    }

    private void StartMusic()
    {
        if (!isMusicStarted && gameStates.isGameStarded)
        {
            AudioManager.Instance.PlayMusic("Game");
            isMusicStarted = true;
        }
    }

    private void ShowGameOverPanel()
    {
        if (gameStates.isGameOver && !gameStates.isThisTutorial)
        {
            inGamePanel.SetActive(false);
            Time.timeScale = 0f;

            if (!isDataSaved)
            {
                AudioManager.Instance.PlaySFX("GameOver");

                if (CheckBestScore())
                {
                    newRecordText.text = "NEW RECORD!";
                }
                else
                {
                    newRecordText.text = string.Empty;
                }

                AddCurrentValuesToMainSave();
                SetAllScores();
                CheckForMissionsAtGameoverScreen();
                CheckForAchievementsAtGameoverScreen();

                firstAidButtonText.text = "Use First Aid \n(" + mainPlayerData.firstAid + " left)";

                AudioManager.Instance.PlayMusic("Pause");

                gameOverPanel.SetActive(true);

                SaveSystem.SavePlayerData(mainPlayerData);
                isDataSaved = true;
            }
        }
    }

    private void CheckForAchievementsAtGameoverScreen()
    {
        if (!gameStates.isThisTutorial)
        {
            AddOrRemoveValueToRunAchievement(false);

            if (allInOneFirstAid)
            {
                CheckIfAchievementAllInOneIsCompleted();
            }

            if (imBuiltDifferentStimpack > 0 && imBuiltDifferentFirstAid > 0)
            {
                CheckIfAchievementImBuildDifferentIsCompleted();
            }

            CheckForScoreAchievement();
            CheckForDrinkAndLoseAchievement();
            CheckForShieldAchievement();
            CheckForNoResourcesAchievement();
        }

    }

    private void CheckForNoResourcesAchievement()
    {
        if (noResourcesCompleted)
        {
            mainPlayerData.achievementValue[9] = 1;
        }
    }

    private void UpdateNoResourcesTimer()
    {
        if (!gameStates.isThisTutorial)
        {
            if (mainPlayerData.achievementValue[9] != 1 && gameStates.isGameStarded)
            {
                noResurcesTime += 1 * Time.deltaTime;

                if (noResurcesTime >= 300)
                {
                    mainPlayerData.achievementValue[9] = 1;
                    noResourcesCompleted = true;
                    return;
                }
            }
        }
    }

    internal void StartNoResourcesTimer()
    {
        if (!gameStates.isThisTutorial)
        {
            noResurcesTime = 0;
        }
    }

    private void CheckForShieldAchievement()
    {
        if (shieldCount == 10)
        {
            mainPlayerData.achievementValue[7] = 1;
        }
    }

    private void CheckForDrinkAndLoseAchievement()
    {
        if (drinkAndLose && inGameInfo.currentScore < 10100)
        {
            mainPlayerData.achievementValue[6] = 1;
        }
    }

    private void CheckForScoreAchievement()
    {
        mainPlayerData.achievementValue[5] = mainPlayerData.bestScore;
    }

    private void AddOrRemoveValueToRunAchievement(bool isFirstAidUsed)
    {
        if (!isFirstAidUsed)
        {
            mainPlayerData.achievementValue[0]++;
        }
        else
        {
            mainPlayerData.achievementValue[0]--;
        }
    }

    private void AddValueToAllInOneAchievement()
    {
        if (!gameStates.isThisTutorial)
        {
            allInOneCount++;
        }
    }

    private void CheckIfAchievementImBuildDifferentIsCompleted()
    {
        if (imBuiltDifferentFirstAid >= 5 && imBuiltDifferentStimpack >= 5)
        {
            mainPlayerData.achievementValue[2] = 10;
        }
    }

    private void CheckIfAchievementAllInOneIsCompleted()
    {
        if (allInOneCount == 6)
        {
            mainPlayerData.achievementValue[1] = 6;
        }
    }

    private void ContinueGameWithFirstAid()
    {
        AddOrRemoveValueToRunAchievement(true);
        prevCoins = inGameInfo.currentCoins;
        prevEnemiesScore = inGameInfo.enemiesScore;
        playerController.SetPlayerPositionAfterFirstAid();
        gameStates.isGameOver = false;
        AudioManager.Instance.PlayMusic("Game");
        HideGameOverPanel();
        StartGame();
    }

    private void SetAllScores()
    {
        coinsTotalText.text = "Total coins: " + mainPlayerData.totalCoins;
        coinsThisRunText.text = "Coins collected: " + inGameInfo.currentCoins;
        scoreThisRunText.text = "Score: " + inGameInfo.currentScore;
        scoreBestText.text = "Best score: " + mainPlayerData.bestScore;
        medsText.text = "Meds collected/total: " + inGameInfo.currentMeds + "/" + mainPlayerData.meds;
        leftoversText.text = "Leftovers collected/total: " + inGameInfo.currentLeftovers + "/" + mainPlayerData.leftover;
        scrapsText.text = "Scraps collected/total: " + inGameInfo.currentScraps + "/" + mainPlayerData.scraps;
    }

    private void AddCurrentValuesToMainSave()
    {
        if (!gameStates.isThisTutorial)
        {
            mainPlayerData.totalCoins += inGameInfo.currentCoins - prevCoins;
            mainPlayerData.achievementValue[3] += inGameInfo.enemiesScore - prevEnemiesScore;
            mainPlayerData.leftover += inGameInfo.currentLeftovers;
            mainPlayerData.meds += inGameInfo.currentMeds;
            mainPlayerData.scraps += inGameInfo.currentScraps;
        }
    }

    private void HideGameOverPanel()
    {
        if (!gameStates.isGameOver)
        {
            Time.timeScale = 1f;
        }
        gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySFX("Click");
        if (!gameStates.isGameOver)
        {
            gameStates.isGameStarded = true;
            if (gameStartPanel != null)
            {
                Destroy(gameStartPanel);
            }
            inGamePanel.SetActive(true);
        }
        isDataSaved = false;
    }

    public void OnClickMealButton()
    {
        AudioManager.Instance.PlaySFX("Click");

        if (mainPlayerData.meal > 0)
        {
            mainPlayerData.meal--;
            scoreMultiplier = 1.5f;
            mealButton.interactable = false;
            CheckForUseMissions("useMeal");
            if (!allInOneMeal && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneMeal = true;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
        }
    }

    public void OnClickDrinkButton()
    {
        AudioManager.Instance.PlaySFX("Click");
        //TODO (zaczęcie gry później/dalej)
        if (mainPlayerData.drink > 0)
        {
            mainPlayerData.drink--;
            scoreBonus = 10000;
            drinkButton.interactable = false;
            CheckForUseMissions("useDrink");
            if (!allInOneDrink && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneDrink = true;
            }
            if (!drinkAndLose && mainPlayerData.achievementValue[6] != 1)
            {
                drinkAndLose = true;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
        }

    }

    public void OnClickShowHideItemPanelButton()
    {
        AudioManager.Instance.PlaySFX("Click");
        if (itemPanel.activeSelf)
        {
            itemPanel.SetActive(false);
        }
        else
        {
            itemPanel.SetActive(true);
        }
    }

    public void OnClickStimpack()
    {
        AudioManager.Instance.PlaySFX("Stimpack");
        isStimpackOnCooldown = true;
        stimpackTime = 12f;

        if (!gameStates.isThisTutorial)
        {
            stimpackCooldownValue = 60f;
            mainPlayerData.stimpack--;
        }
        else
        {
            stimpackCooldownValue = 0f;
        }

        if (!gameStates.isThisTutorial)
        {
            CheckForUseMissions("useStimpack");
            if (!allInOneStimpack && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneStimpack = true;
            }
            if (mainPlayerData.achievementValue[2] != 10)
            {
                imBuiltDifferentStimpack++;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
        }
    }

    public void OnClickMagnet()
    {
        AudioManager.Instance.PlaySFX("Magnet");
        isMagnetOnCooldown = true;
        magnetTime = 10f;
        if (!gameStates.isThisTutorial)
        {
            magnetCooldownValue = 60f;
            mainPlayerData.magnet--;
        }
        else
        {
            magnetCooldownValue = 0f;
        }

        if (!gameStates.isThisTutorial)
        {

            CheckForUseMissions("useMagnet");
            if (!allInOneMagnet && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneMagnet = true;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
        }
    }

    public void OnClickShield()
    {
        AudioManager.Instance.PlaySFX("Shield");
        isShieldOnCooldown = true;
        shieldActive = true;

        if (!gameStates.isThisTutorial)
        {
            shieldkCooldownValue = 120f;
            mainPlayerData.shield--;
        }
        else
        {
            shieldkCooldownValue = 0f;
        }


        if (!gameStates.isThisTutorial)
        {
            CheckForUseMissions("useShield");
            if (!allInOneShield && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneShield = true;
            }
            if (shieldCount < 10 && mainPlayerData.achievementValue[7] != 1)
            {
                shieldCount++;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
        }
    }

    public void OnClickFirstAid()
    {
        AudioManager.Instance.PlaySFX("Click");
        if (mainPlayerData.firstAid > 0)
        {
            mainPlayerData.firstAid--;
            CheckForUseMissions("useFirstAid");
            if (!allInOneFirstAid && mainPlayerData.achievementValue[1] != 6)
            {
                AddValueToAllInOneAchievement();
                allInOneFirstAid = true;
            }
            if (mainPlayerData.achievementValue[2] != 10)
            {
                imBuiltDifferentFirstAid++;
            }
            SaveSystem.SavePlayerData(mainPlayerData);
            ContinueGameWithFirstAid();
            AudioManager.Instance.PlaySFX("FirstAid");
            StartCoroutine(GhostModeCoroutine());
        }
        else
        {
            Debug.Log("No First Aids!");
        }
    }

    private void CheckForUseMissions(string useItemName)
    {
        if (mainPlayerData.quest1ValueName.Contains(useItemName))
        {
            mainPlayerData.quest1Value--;
        }
        else if (mainPlayerData.quest2ValueName.Contains(useItemName))
        {
            mainPlayerData.quest2Value--;
        }
        else if (mainPlayerData.quest3ValueName.Contains(useItemName))
        {
            mainPlayerData.quest3Value--;
        }
    }

    private void CheckForDieAfterScoreXMissions()
    {
        if (mainPlayerData.quest1ValueName.Contains("dieAfterScoreX"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest1Value && inGameInfo.currentScore <= (mainPlayerData.quest1Value + 200))
            {
                mainPlayerData.quest1Value = 0;
            }
        }
        else if (mainPlayerData.quest2ValueName.Contains("dieAfterScoreX"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest2Value && inGameInfo.currentScore <= (mainPlayerData.quest2Value + 200))
            {
                mainPlayerData.quest2Value = 0;
            }
        }
        else if (mainPlayerData.quest3ValueName.Contains("dieAfterScoreX"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest3Value && inGameInfo.currentScore <= (mainPlayerData.quest3Value + 200))
            {
                mainPlayerData.quest3Value = 0;
            }
        }
    }

    private void CheckForZombieMissions()
    {
        if (mainPlayerData.quest1ValueName.Contains("enemiesToKill"))
        {
            mainPlayerData.quest1Value -= inGameInfo.enemiesScore;
        }
        else if (mainPlayerData.quest2ValueName.Contains("enemiesToKill"))
        {
            mainPlayerData.quest2Value -= inGameInfo.enemiesScore;
        }
        else if (mainPlayerData.quest3ValueName.Contains("enemiesToKill"))
        {
            mainPlayerData.quest3Value -= inGameInfo.enemiesScore;
        }
    }

    private void CheckForAchieveScoreMisions()
    {
        if (mainPlayerData.quest1ValueName.Contains("scoreToAchieve"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest1Value)
            {
                mainPlayerData.quest1Value = 0;
            }
        }
        else if (mainPlayerData.quest2ValueName.Contains("scoreToAchieve"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest2Value)
            {
                mainPlayerData.quest2Value = 0;
            }
        }
        else if (mainPlayerData.quest3ValueName.Contains("scoreToAchieve"))
        {
            if (inGameInfo.currentScore >= mainPlayerData.quest3Value)
            {
                mainPlayerData.quest3Value = 0;
            }
        }
    }

    private void CheckForJumpMissions()
    {
        if (mainPlayerData.quest1ValueName.Contains("jumpsAmount"))
        {
            mainPlayerData.quest1Value -= inGameInfo.jumpsScore;
        }
        else if (mainPlayerData.quest2ValueName.Contains("jumpsAmount"))
        {
            mainPlayerData.quest2Value -= inGameInfo.jumpsScore;
        }
        else if (mainPlayerData.quest3ValueName.Contains("jumpsAmount"))
        {
            mainPlayerData.quest3Value -= inGameInfo.jumpsScore;
        }
    }

    private void CheckForSlideMissions()
    {
        if (mainPlayerData.quest1ValueName.Contains("slidesAmount"))
        {
            mainPlayerData.quest1Value -= inGameInfo.slidesScore;
        }
        else if (mainPlayerData.quest2ValueName.Contains("slidesAmount"))
        {
            mainPlayerData.quest2Value -= inGameInfo.slidesScore;
        }
        else if (mainPlayerData.quest3ValueName.Contains("slidesAmount"))
        {
            mainPlayerData.quest3Value -= inGameInfo.slidesScore;
        }
    }

    private void CheckForMaxSpeedMissions()
    {
        if (mainPlayerData.quest1ValueName.Contains("achieveMaxSpeed"))
        {
            if (inGameInfo.maxSpeed)
            {
                mainPlayerData.quest1Value = 0;
            }
        }
        else if (mainPlayerData.quest2ValueName.Contains("achieveMaxSpeed"))
        {
            if (inGameInfo.maxSpeed)
            {
                mainPlayerData.quest2Value = 0;
            }
        }
        else if (mainPlayerData.quest3ValueName.Contains("achieveMaxSpeed"))
        {
            if (inGameInfo.maxSpeed)
            {
                mainPlayerData.quest3Value = 0;
            }
        }
    }

    private void CheckForMissionsAtGameoverScreen()
    {
        CheckForDieAfterScoreXMissions();
        CheckForZombieMissions();
        CheckForAchieveScoreMisions();
        CheckForJumpMissions();
        CheckForSlideMissions();
        CheckForMaxSpeedMissions();
    }

    private IEnumerator GhostModeCoroutine()
    {
        ghostMode = true;
        tempGhostImage.SetActive(true);
        yield return new WaitForSeconds(3f);
        tempGhostImage.SetActive(false);
        ghostMode = false;
    }

    private void UpdateItemUI()
    {
        if (isStimpackOnCooldown && stimpackTime > 0)
        {
            stimpackButton.interactable = false;
            //stimpackButton.GetComponent<Image>().color = Color.green;
            stimpackCooldown.text = Mathf.FloorToInt(stimpackTime).ToString();
        }
        else if (isStimpackOnCooldown && stimpackTime <= 0)
        {
            stimpackButton.interactable = false;
            //stimpackButton.GetComponent<Image>().color = Color.red;
            stimpackCooldown.text = Mathf.FloorToInt(stimpackCooldownValue).ToString();
        }
        else if (gameStates.isThisTutorial)
        {
            //stimpackButton.interactable = true;
            //stimpackButton.GetComponent<Image>().color = Color.green;
            stimpackCooldown.text = "0";
        }
        else
        {
            stimpackButton.interactable = true;
            stimpackButton.GetComponent<Image>().color = Color.white;
            stimpackCooldown.text = string.Empty;
        }

        if (isMagnetOnCooldown && magnetTime > 0)
        {
            magnetButton.interactable = false;
            //magnetButton.GetComponent<Image>().color = Color.green;
            magnetCooldown.text = Mathf.FloorToInt(magnetTime).ToString();
        }
        else if (isMagnetOnCooldown && magnetTime <= 0)
        {
            magnetButton.interactable = false;
            //magnetButton.GetComponent<Image>().color = Color.red;
            magnetCooldown.text = Mathf.FloorToInt(magnetCooldownValue).ToString();
        }
        else if (gameStates.isThisTutorial)
        {
            //magnetButton.interactable = true;
            //magnetButton.GetComponent<Image>().color = Color.green;
            magnetCooldown.text = "0";
        }
        else
        {
            magnetButton.interactable = true;
            magnetButton.GetComponent<Image>().color = Color.white;
            magnetCooldown.text = string.Empty;
        }


        if (isShieldOnCooldown && shieldActive)
        {
            shieldButton.interactable = false;
            //shieldButton.GetComponent<Image>().color = Color.green;
            shieldCooldown.text = "IMMORTAL";
        }
        else if (isShieldOnCooldown && !shieldActive)
        {
            shieldButton.interactable = false;
            //shieldButton.GetComponent<Image>().color = Color.red;
            shieldCooldown.text = Mathf.FloorToInt(shieldkCooldownValue).ToString();
        }
        else if (gameStates.isThisTutorial)
        {
            //shieldButton.interactable = true;
            //shieldButton.GetComponent<Image>().color = Color.green;
            shieldCooldown.text = "0";
        }
        else
        {
            shieldButton.interactable = true;
            shieldButton.GetComponent<Image>().color = Color.white;
            shieldCooldown.text = string.Empty;
        }


    }

    private void UpdateItemQuantity()
    {

        if (!gameStates.isThisTutorial)
        {
            stimpackQuantity.text = mainPlayerData.stimpack.ToString();
            magnetQuantity.text = mainPlayerData.magnet.ToString();
            shieldQuantity.text = mainPlayerData.shield.ToString();

            if (mainPlayerData.stimpack < 1)
            {
                stimpackButton.interactable = false;
            }

            if (mainPlayerData.magnet < 1)
            {
                magnetButton.interactable = false;
            }

            if (mainPlayerData.shield < 1)
            {
                shieldButton.interactable = false;
            }
        }
        else
        {
            stimpackQuantity.text = "999+";
            magnetQuantity.text = "999+";
            shieldQuantity.text = "999+";
        }

    }

    private void UpdateGameStartScreen()
    {
        if (!gameStates.isThisTutorial)
        {
            if (gameStartPanel.activeSelf)
            {
                if (mainPlayerData.meal < 1)
                {
                    mealButton.interactable = false;
                }

                if (mainPlayerData.drink < 1)
                {
                    drinkButton.interactable = false;
                }

                mealsOwned.text = "Your meals: " + mainPlayerData.meal;
                mealCurrentBoost.text = "Score: " + "x1.5";
                drinksOwned.text = "Your drinks: " + mainPlayerData.drink;
                drinkCurrentBoost.text = "Staring score +" + "10000";
                // na razie niezmienne wartości itemów
            }
        }
    }

    private void UpdateCollectedCoins()
    {
        coinsCollectedText.text = "Coins: " + inGameInfo.currentCoins;
    }

    private void UpdateScore()
    {
        float calculatingScore = offset + playerController.transform.position.z + (inGameInfo.currentCoins * 5) + (inGameInfo.enemiesScore * 50);

        float finalScore = scoreBonus + (calculatingScore * scoreMultiplier);

        inGameInfo.currentScore = Mathf.RoundToInt(finalScore);

        scoreTextInGamePanel.text = "Score: " + inGameInfo.currentScore;

        //for enemies info
        inGameInfo.playerPositionZ = playerController.transform.position.z;
    }

    private bool CheckBestScore()
    {
        if (!gameStates.isThisTutorial)
        {
            if (inGameInfo.currentScore >= mainPlayerData.bestScore)
            {
                mainPlayerData.bestScore = inGameInfo.currentScore;
                AudioManager.Instance.PlaySFX("Achievement");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void UpdateFireButtonText()
    {
        if (inGameInfo.currentAmmo > 0)
        {
            fireButtonText.text = inGameInfo.currentAmmo + "/" + mainPlayerData.maxAmmo;
        }
        else
        {
            fireButtonText.text = "No Ammo";
        }
    }

    private void ResetValues()
    {
        inGameInfo.currentAmmo = 0;
        inGameInfo.currentCoins = 0;
        inGameInfo.enemiesScore = 0;
        inGameInfo.currentScore = 0;
        inGameInfo.currentLeftovers = 0;
        inGameInfo.currentMeds = 0;
        inGameInfo.currentScraps = 0;
        inGameInfo.jumpsScore = 0;
        inGameInfo.slidesScore = 0;
        inGameInfo.maxSpeed = false;
        inGameInfo.playerPositionZ = 0;
        scoreMultiplier = 1f;
        scoreBonus = 0;
        stimpackCooldownValue = 0;
        magnetCooldownValue = 0;
        shieldkCooldownValue = 0;
        isDataSaved = false;
        allInOneFirstAid = false;
        allInOneStimpack = false;
        allInOneMeal = false;
        allInOneDrink = false;
        allInOneShield = false;
        allInOneMagnet = false;
        allInOneCount = 0;
        imBuiltDifferentFirstAid = 0;
        imBuiltDifferentStimpack = 0;
        isMusicStarted = false;
    }

    public float StimpackTime { get { return stimpackTime; } set { stimpackTime = value; } }
    public float MagnetTime { get { return magnetTime; } set { magnetTime = value; } }
    public bool ShieldActive { get { return shieldActive; } set { shieldActive = value; } }
    public bool GhostMode { get { return ghostMode; } set { ghostMode = value; } }
    public PlayerData MainPlayerData { get { return mainPlayerData; } set { mainPlayerData = value; } }
}
