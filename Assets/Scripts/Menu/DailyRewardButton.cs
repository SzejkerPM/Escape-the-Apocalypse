using TMPro;
using UnityEngine;

public class DailyRewardButton : MonoBehaviour
{
    private int dailyRewardIndex;
    private int dailyRewardAmount;

    [SerializeField]
    private TextMeshProUGUI onButtonAmountText;

    public int DailyRewardIndex { get { return dailyRewardIndex; } set { dailyRewardIndex = value; } }
    public int DailyRewardAmount { get { return dailyRewardAmount; } set { dailyRewardAmount = value; } }

    public TextMeshProUGUI OnButtonAmountText { get { return onButtonAmountText; } set { onButtonAmountText = value; } }
}
