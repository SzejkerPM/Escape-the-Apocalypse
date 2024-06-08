using UnityEngine;

public class Meds : MonoBehaviour
{
    [SerializeField]
    private InGameInfoSO inGameInfo;

    [SerializeField]
    private GameStatesSO gameStates;

    private PlayerMenager playerMenager;

    private void Start()
    {
        playerMenager = FindObjectOfType<PlayerMenager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameInfo.currentMeds++;

            AudioManager.Instance.PlaySFX("ResourcesPickup");

            if (!gameStates.isThisTutorial)
            {

                if (playerMenager.mainPlayerData.quest1ValueName.Contains("collectMeds"))
                {
                    playerMenager.mainPlayerData.quest1Value--;
                }
                else if (playerMenager.mainPlayerData.quest2ValueName.Contains("collectMeds"))
                {
                    playerMenager.mainPlayerData.quest2Value--;
                }
                else if (playerMenager.mainPlayerData.quest3ValueName.Contains("collectMeds"))
                {
                    playerMenager.mainPlayerData.quest3Value--;
                }

                if (playerMenager.mainPlayerData.achievementValue[9] != 1)
                {
                    playerMenager.StartNoResourcesTimer();
                }

                playerMenager.mainPlayerData.achievementValue[10]++;

            }

            Destroy(gameObject);
        }
    }
}
