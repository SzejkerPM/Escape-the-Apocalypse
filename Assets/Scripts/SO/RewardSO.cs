using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Custom/reward")]
public class RewardSO : ScriptableObject
{
    public int index;

    public string rewardItemName;

    public int rewardAmount;

    public RewardSO Clone()
    {
        RewardSO clonedData = Instantiate(this);
        clonedData.index = index;
        clonedData.rewardItemName = rewardItemName;
        clonedData.rewardAmount = rewardAmount;

        return clonedData;
    }
}
