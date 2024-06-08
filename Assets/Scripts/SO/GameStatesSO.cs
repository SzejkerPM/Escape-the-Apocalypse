using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Custom/GameState")]
public class GameStatesSO : ScriptableObject
{

    public bool isGameOver = false;
    public bool isGameStarded = false;
    public bool isThisTutorial = false;
    public bool isThisMenuScene = true;
    public bool isControllerOn = true;

}
