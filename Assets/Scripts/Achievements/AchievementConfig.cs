using TMPro;
using UnityEngine;

public class AchievementConfig : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI titleText;

    [SerializeField]
    public TextMeshProUGUI descriptionText;

    [SerializeField]
    public TextMeshProUGUI currentAndMaxLevel;

    [SerializeField]
    public AchievementSO achievement;


    private void Start()
    {
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;
    }
}
