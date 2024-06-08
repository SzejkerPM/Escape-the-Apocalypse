using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private InGameInfoSO inGameInfo;

    [SerializeField]
    private GameStatesSO gameStates;

    [SerializeField]
    private float rotationSpeed;

    private PlayerMenager playerMenager;

    private void Start()
    {
        playerMenager = FindObjectOfType<PlayerMenager>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("CoinPickup");

            inGameInfo.currentCoins++;

            if (!gameStates.isThisTutorial)
            {

                if (playerMenager.mainPlayerData.quest1ValueName.Contains("collectCoins"))
                {
                    playerMenager.mainPlayerData.quest1Value--;
                }
                else if (playerMenager.mainPlayerData.quest2ValueName.Contains("collectCoins"))
                {
                    playerMenager.mainPlayerData.quest2Value--;
                }
                else if (playerMenager.mainPlayerData.quest3ValueName.Contains("collectCoins"))
                {
                    playerMenager.mainPlayerData.quest3Value--;
                }

                playerMenager.mainPlayerData.achievementValue[13]++;

            }

            Destroy(gameObject);
        }
    }
}
