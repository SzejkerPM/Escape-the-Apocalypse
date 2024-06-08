using UnityEngine;

[CreateAssetMenu(fileName = "QuestRequirements", menuName = "Custom/QuestRequirements")]
public class QuestRequirementsSO : ScriptableObject
{
    public int index;

    public string description;

    public string craftItemName;
    public int craftItemAmount;

    public string itemToUse;
    public int itemToUseAmount;

    public string itemToCollect;
    public int itemToCollectAmount;

    public int dieAfterScoreX;
    public int dieBeforeScoreY;

    public int enemiesToKill;

    public int scoreToAchieve;

    public int jumpsAmount;

    public int slidesAmount;

    public int achieveMaxSpeed;

    public QuestRequirementsSO Clone()
    {
        QuestRequirementsSO clonedData = Instantiate(this);
        clonedData.index = index;
        clonedData.description = description;
        clonedData.craftItemName = craftItemName;
        clonedData.craftItemAmount = craftItemAmount;
        clonedData.itemToUse = itemToUse;
        clonedData.itemToUseAmount = itemToUseAmount;
        clonedData.itemToCollect = itemToCollect;
        clonedData.itemToCollectAmount = itemToCollectAmount;
        clonedData.dieAfterScoreX = dieAfterScoreX;
        clonedData.dieBeforeScoreY = dieBeforeScoreY;
        clonedData.enemiesToKill = enemiesToKill;
        clonedData.scoreToAchieve = scoreToAchieve;
        clonedData.jumpsAmount = jumpsAmount;
        clonedData.slidesAmount = slidesAmount;
        clonedData.achieveMaxSpeed = achieveMaxSpeed;

        return clonedData;
    }
}
