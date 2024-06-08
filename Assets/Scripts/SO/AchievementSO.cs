using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Custom/Achievements")]
public class AchievementSO : ScriptableObject
{

    public int index;

    public string title;
    public string description;

    public bool areAllLevelsCompleted;

    public int[] tasks;

    public int[] rewards;

    public AchievementSO Clone()
    {
        AchievementSO clonedData = Instantiate(this);
        clonedData.index = index;
        clonedData.title = title;
        clonedData.description = description;
        clonedData.areAllLevelsCompleted = areAllLevelsCompleted;
        clonedData.tasks = tasks;
        clonedData.rewards = rewards;

        return clonedData;
    }

}
