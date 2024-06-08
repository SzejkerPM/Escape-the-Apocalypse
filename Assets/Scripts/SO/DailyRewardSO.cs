using UnityEngine;

[CreateAssetMenu(fileName = "DailyReward", menuName = "Custom/DailyRewards")]
public class DailyRewardSO : ScriptableObject
{
    public int index;
    public string rewardName;
    public Sprite rewardImage;
    public int rewardAmount;
}
