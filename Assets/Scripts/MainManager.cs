using UnityEngine;

public class MainManager : MonoBehaviour
{

    [SerializeField]
    private GameStatesSO gameStates;

    private PlayerData playerData;

    void Start()
    {
        Debug.Log("MAIN_MANAGER: Working!");
        gameStates.isGameOver = false;
        gameStates.isGameStarded = false;
        gameStates.isThisTutorial = false;
        gameStates.isThisMenuScene = true;
        gameStates.isControllerOn = true;

        playerData = SaveSystem.LoadPlayerData();

        if (playerData.firstStart)
        {
            gameStates.isThisTutorial = true;

            playerData.firstStart = false;
            SaveSystem.SavePlayerData(playerData);
        }
    }

}
