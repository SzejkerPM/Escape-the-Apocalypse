using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{

    [SerializeField]
    private GameStatesSO gameStates;

    public void ReplayButton()
    {
        gameStates.isGameOver = false;
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene("Game");
    }

    public void MenuButton()
    {
        gameStates.isGameOver = false;
        gameStates.isThisMenuScene = true;
        gameStates.isGameStarded = false;
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX("Click");
        AudioManager.Instance.PlayMusic("Menu");
        SceneManager.LoadScene("Menu");
    }
}
