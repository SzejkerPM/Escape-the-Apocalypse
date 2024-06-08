using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Custom/Quest")]
public class QuestSO : ScriptableObject
{

    public Sprite background;

    public int multiplier;

    public RewardSO reward;

    public QuestRequirementsSO requirements;

    public bool completed = false;

    public bool rewardClaimed = false;

    public QuestSO Clone()
    {
        QuestSO clonedData = Instantiate(this);
        clonedData.requirements = null;
        clonedData.reward = null;
        clonedData.multiplier = 0;
        clonedData.completed = false;
        clonedData.background = null;
        return clonedData;
    }

}
