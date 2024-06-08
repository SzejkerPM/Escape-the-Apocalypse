using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private SwipeMenager swipeMenager;

    [SerializeField]
    private GameObject tutorialWelcomePanel;

    [SerializeField]
    private GameObject startGamePanel;

    [SerializeField]
    private GameObject tutorialMainPanel;

    [SerializeField]
    private GameObject inGamePanel; //from game

    [SerializeField]
    private GameObject itemsPanel;

    [SerializeField]
    private GameObject itemHideButton;

    [SerializeField]
    private GameObject fireButton;

    [SerializeField]
    private GameObject scoreText;

    [SerializeField]
    private GameObject coinText;

    [SerializeField]
    private Button stimpackButton;

    [SerializeField]
    private Button magnetButton;

    [SerializeField]
    private Button shieldButton;

    [SerializeField]
    private TextMeshProUGUI tutorialTextField;

    [SerializeField]
    private InGameInfoSO inGameInfo;

    [SerializeField]
    private GameObject fireButtonArrow;

    [SerializeField]
    private GameObject hideButtonArrow;

    [SerializeField]
    private GameObject itemPanelArrow;

    [SerializeField]
    private GameObject itemOverlayPanel;

    [SerializeField]
    private TextMeshProUGUI counterText;

    [SerializeField]
    private GameObject counterPanel;

    private float writingSpeed = 20;

    [SerializeField]
    private GameStatesSO gameStates;

    private string[] texts;

    private string tryAgain;

    private int currentTutorialStage = 0;

    private int currentMap = 0;

    private int lastStringIndex = 0;

    private bool[] stagesCompleted;

    private bool isWriting;

    private bool isInStage = false;

    private bool playerFailed = false;

    private PlayerController playerController;

    private TileMenager tileMenager;

    void Start()
    {
        if (gameStates.isThisTutorial)
        {
            tutorialWelcomePanel.SetActive(true);
            startGamePanel.SetActive(false);

            SetUpStrings();
            stagesCompleted = new bool[10];

            playerController = FindObjectOfType<PlayerController>();
            tileMenager = FindObjectOfType<TileMenager>();

            inGameInfo.tutorialCounter = 0;
            gameStates.isControllerOn = false;
        }
        else
        {
            Destroy(tutorialWelcomePanel);
            Destroy(tutorialMainPanel);
            Destroy(counterPanel);
            Destroy(gameObject);
        }

    }


    void Update()
    {
        if (isInStage)
        {
            CheckIfStageIsCompletedYet();
            counterPanel.SetActive(true);
        }
        else
        {
            counterPanel.SetActive(false);
        }

    }

    private void MakeActionByIndex(int index)
    {
        if (isWriting)
        {
            isWriting = false;

            StopAllCoroutines();

            if (!playerFailed)
            {
                tutorialTextField.text = texts[lastStringIndex];
            }
            else
            {
                tutorialTextField.text = tryAgain + texts[lastStringIndex];
                playerFailed = false;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    TypeTextFromIndex(0);
                    lastStringIndex = 0;
                    break;
                case 1:
                    TypeTextFromIndex(1);
                    lastStringIndex = 1;
                    break;
                case 2:
                    TypeTextFromIndex(2);
                    lastStringIndex = 2;
                    break;
                case 3:
                    StartStage(); //0
                    break;
                case 4:
                    TypeTextFromIndex(3);
                    lastStringIndex = 3;
                    break;
                case 5:
                    StartStage(); //1
                    break;
                case 6:
                    TypeTextFromIndex(4);
                    lastStringIndex = 4;
                    break;
                case 7:
                    StartStage(); //2
                    break;
                case 8:
                    TypeTextFromIndex(5);
                    lastStringIndex = 5;
                    break;
                case 9:
                    SetUpStageForShooting(true);
                    StartStage(); //3
                    break;
                case 10:
                    TypeTextFromIndex(6);
                    lastStringIndex = 6;
                    SetUpStageForShooting(true);
                    fireButtonArrow.SetActive(true);
                    break;
                case 11:
                    fireButtonArrow.SetActive(false);
                    StartStage(); //4
                    break;
                case 12:
                    SetUpStageForItems(true);
                    itemPanelArrow.SetActive(true);
                    TypeTextFromIndex(7);
                    lastStringIndex = 7;
                    break;
                case 13:
                    TypeTextFromIndex(8);
                    itemPanelArrow.SetActive(false);
                    hideButtonArrow.SetActive(true);
                    itemOverlayPanel.SetActive(true);
                    lastStringIndex = 8;
                    break;
                case 14:
                    TypeTextFromIndex(9);
                    itemOverlayPanel.SetActive(false);
                    hideButtonArrow.SetActive(false);
                    lastStringIndex = 9;
                    break;
                case 15:
                    stimpackButton.interactable = true;
                    magnetButton.interactable = false;
                    shieldButton.interactable = false;
                    StartStage(); //5
                    break;
                case 16:
                    TypeTextFromIndex(10);
                    lastStringIndex = 10;
                    break;
                case 17:
                    stimpackButton.interactable = false;
                    magnetButton.interactable = true;
                    shieldButton.interactable = false;
                    StartStage(); //6
                    break;
                case 18:
                    TypeTextFromIndex(11);
                    lastStringIndex = 11;
                    break;
                case 19:
                    stimpackButton.interactable = false;
                    magnetButton.interactable = false;
                    shieldButton.interactable = true;
                    StartStage(); //7
                    break;
                case 20:
                    TypeTextFromIndex(12);
                    itemsPanel.SetActive(false);
                    itemHideButton.SetActive(false);
                    lastStringIndex = 12;
                    break;
                case 21:
                    TypeTextFromIndex(13);
                    lastStringIndex = 13;
                    break;
                case 22:
                    TypeTextFromIndex(14);
                    lastStringIndex = 14;
                    break;
                case 23:
                    ExitTutorial();
                    break;

            }
        }
    }

    private void SetUpStrings()
    {
        texts = new string[15];
        tryAgain = "(Try Again!)\n";
        texts[0] = "Welcome!\nThis is an ETA tutorial!\n(tap text panel to speed up, tap again to continue)";
        texts[1] = "The game is about surviving as long as possible and breaking records.";
        texts[2] = "Swipe right or left to change the running track.\nTry to change the running track 6 times!";
        texts[3] = "Swipe down to slide. Swipe up to jump.\nTry to overcome the hurdles 6 times!";
        texts[4] = "With resources, you can create useful items.\nCollect 6 resources by running into them!";
        texts[5] = "You will collect ammunition in the same way.\nTry it!";
        texts[6] = "The red button is for shooting and shows you how much ammo you have.\nDefeat 6 zombies!";
        texts[7] = "On the right side of the screen you have a backpack with your items.";
        texts[8] = "You can use: Stimpack, Magnet and Shield.\nYou can also hide the bar by clicking the arrow above it.";
        texts[9] = "Stimpack allows you to remove obstacles that you run into.\nTry using a Stimpack now!";
        texts[10] = "The magnet attracts coins and resources.\nTry using a Magnet!";
        texts[11] = "The shield grants immunity to the first damage received and for a moment after receiving it.\nTry using a shield!";
        texts[12] = "Congratulations!\nYou already know the basics of the game! I'm sure you'll figure out the rest yourself.";
        texts[13] = "In menu panels such as crafting, you will find a tooltip icon that contains detailed descriptions.\nYou can also play the tutorial again.";
        texts[14] = "And one more thing.\nThe character speeds up the longer you run, so the game will become more difficult over time.\nGood luck breaking records!";
    }

    private void StartStage()
    {
        tutorialMainPanel.SetActive(false);
        isInStage = true;
        SetPlayerOnStartAndEndOfTheStage();
        gameStates.isControllerOn = true;

        if (currentMap == 3 || currentMap == 4)
        {
            SetUpStageForShooting(true);
        }
        else if (currentMap >= 5 && currentMap <= 7)
        {
            SetUpStageForItems(true);
        }
    }

    private void CheckIfStageIsCompletedYet()
    {

        switch (currentMap) // 0-swipes, 1-hurdles, 2-resources, 3-ammo, 4-zombie, 5-stimpack, 6-magnet, 7-shield
        {
            case 0:

                counterText.text = inGameInfo.tutorialCounter + "/6";

                if (inGameInfo.tutorialCounter >= 6)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 0));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 0));
                }
                break;

            case 1:

                counterText.text = inGameInfo.tutorialCounter + "/6";

                if (inGameInfo.tutorialCounter >= 6)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 1));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 1));
                }
                break;

            case 2:

                counterText.text = inGameInfo.tutorialCounter + "/6";

                if (inGameInfo.tutorialCounter >= 6)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 2));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 2));
                }
                break;

            case 3:

                counterText.text = inGameInfo.tutorialCounter + "/1";

                if (inGameInfo.tutorialCounter >= 1)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 3));
                    SetUpStageForShooting(true);
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 3));
                    SetUpStageForShooting(true);
                }
                break;

            case 4:

                counterText.text = inGameInfo.tutorialCounter + "/6";

                if (inGameInfo.tutorialCounter >= 6)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 4));
                    SetUpStageForShooting(false);
                }
                else if (gameStates.isGameOver || inGameInfo.currentAmmo <= 0)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 4));
                    SetUpStageForShooting(true);
                    fireButtonArrow.SetActive(true);
                    inGameInfo.currentAmmo = 10;
                }
                break;

            case 5:

                counterText.text = inGameInfo.tutorialCounter + "/1";

                if (inGameInfo.tutorialCounter == 1)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 5));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 5));
                }
                break;

            case 6:

                counterText.text = inGameInfo.tutorialCounter + "/1";

                if (inGameInfo.tutorialCounter == 1)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 6));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 6));
                }
                break;

            case 7:

                counterText.text = inGameInfo.tutorialCounter + "/1";

                if (inGameInfo.tutorialCounter == 1)
                {
                    StartCoroutine(EndStageWithWait(1f, true, 7));
                }
                else if (gameStates.isGameOver)
                {
                    StartCoroutine(EndStageWithWait(0f, false, 7));
                }
                break;
        }
    }

    private void SetUpStageForShooting(bool isOn)
    {
        if (isOn)
        {
            inGamePanel.SetActive(true);
            coinText.SetActive(false);
            scoreText.SetActive(false);
            fireButton.SetActive(true);
            itemsPanel.SetActive(false);
            itemHideButton.SetActive(false);
        }
        else
        {
            inGamePanel.SetActive(false);
            coinText.SetActive(true);
            scoreText.SetActive(true);
            fireButton.SetActive(true);
            itemsPanel.SetActive(true);
            itemHideButton.SetActive(true);
        }
    }

    private void SetUpStageForItems(bool isOn)
    {
        if (isOn)
        {
            inGamePanel.SetActive(true);
            coinText.SetActive(false);
            scoreText.SetActive(false);
            fireButton.SetActive(false);
            itemsPanel.SetActive(true);
            itemHideButton.SetActive(true);
        }
        else
        {
            inGamePanel.SetActive(false);
            coinText.SetActive(true);
            scoreText.SetActive(true);
            fireButton.SetActive(true);
            itemsPanel.SetActive(true);
            itemHideButton.SetActive(true);
        }
    }

    private void TypeTextFromIndex(int textIndex)
    {
        if (!isWriting)
        {
            isWriting = true;

            tutorialTextField.text = string.Empty;
            StartCoroutine(TypeText(texts[textIndex]));
        }

    }

    private IEnumerator TypeText(string text)
    {
        if (!playerFailed)
        {
            currentTutorialStage++;

            for (int i = 0; i < text.Length; i++)
            {
                tutorialTextField.text += text[i];
                yield return new WaitForSeconds(1.0f / writingSpeed);
            }
        }
        else
        {
            for (int i = 0; i < tryAgain.Length; i++)
            {
                tutorialTextField.text += tryAgain[i];
                yield return new WaitForSeconds(1.0f / writingSpeed);
            }

            for (int i = 0; i < text.Length; i++)
            {
                tutorialTextField.text += text[i];
                yield return new WaitForSeconds(1.0f / writingSpeed);
            }

            playerFailed = false;
        }

        isWriting = false;
    }

    private IEnumerator EndStageWithWait(float seconds, bool isCompleted, int stageNumber)
    {
        yield return new WaitForSeconds(seconds);

        if (!stagesCompleted[stageNumber])
        {
            if (isCompleted)
            {
                stagesCompleted[stageNumber] = true;
                isInStage = false;
                tutorialMainPanel.SetActive(true);
                SetPlayerOnStartAndEndOfTheStage();
                currentTutorialStage++;
                currentMap++;
                MakeActionByIndex(currentTutorialStage);
            }
            else
            {
                isInStage = false;
                playerFailed = true;
                gameStates.isGameOver = false;
                tutorialMainPanel.SetActive(true);
                SetPlayerOnStartAndEndOfTheStage();
                TypeTextFromIndex(lastStringIndex);
            }

            inGameInfo.tutorialCounter = 0;
            gameStates.isControllerOn = false;
            counterText.text = string.Empty;
        }
    }

    private void SetPlayerOnStartAndEndOfTheStage()
    {
        playerController.TutorialSetPlayerOnStartStage();
        tileMenager.TutorialGenerateMapForNextStage();
    }

    public void OnClickContinueTutorial()
    {
        if (tutorialMainPanel.activeSelf)
        {
            MakeActionByIndex(currentTutorialStage);
        }
    }

    public void LoadTutorialPanel()
    {
        if (tutorialWelcomePanel != null)
        {
            Destroy(tutorialWelcomePanel);
        }
        SetPlayerOnStartAndEndOfTheStage();
        tutorialMainPanel.SetActive(true);
        gameStates.isGameStarded = true;
        gameStates.isGameOver = false;
        TypeTextFromIndex(0);
    }

    public void SkipTutorial()
    {
        gameStates.isThisTutorial = false;
        gameStates.isGameStarded = false;
        gameStates.isGameOver = false;
        gameStates.isControllerOn = true;
        SceneManager.LoadScene("Game");
    }

    public void ExitTutorial()
    {
        gameStates.isThisTutorial = false;
        gameStates.isGameStarded = false;
        gameStates.isGameOver = false;
        gameStates.isControllerOn = true;
        AudioManager.Instance.PlayMusic("Menu");
        SceneManager.LoadScene("Menu");
    }

    public int CurrentMap { get { return currentMap; } }

    public bool IsInStage { get { return isInStage; } }
}
