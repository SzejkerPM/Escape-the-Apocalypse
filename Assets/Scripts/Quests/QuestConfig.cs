using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestConfig : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI descriptionText;

    [SerializeField]
    public TextMeshProUGUI rewardButtonText;

    [SerializeField]
    public Button button;

    [SerializeField]
    public QuestSO allQuestData;

    public MissionButtonData buttonData;

    private void Start()
    {
        buttonData = SetButtonData();
    }

    private void Update()
    {
        if (!allQuestData.completed)
        {
            button.interactable = false;
            button.GetComponent<Image>().color = Color.red;
        }
        else if (allQuestData.rewardClaimed)
        {
            button.interactable = false;
            button.GetComponent<Image>().color = Color.gray;
            descriptionText.text = "Completed!";
        }
        else
        {
            button.interactable = true;
            button.GetComponent<Image>().color = Color.green;
            descriptionText.text = "Good job!\nClaim your reward!";
        }

    }

    private MissionButtonData SetButtonData()
    {
        return new MissionButtonData(allQuestData.reward.rewardItemName, allQuestData.reward.rewardAmount);
    }

    public void TransferButtonDataToMissionManager()
    {
        var missionManager = FindObjectOfType<MissionMenager>();

        missionManager.buttonData = buttonData;
        missionManager.isButtonClicked = true;
        allQuestData.rewardClaimed = true;
        missionManager.SaveMissionsWhenRewardClaimed();
    }

}
