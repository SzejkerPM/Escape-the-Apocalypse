using UnityEngine;

[CreateAssetMenu(fileName = "InGameInfo", menuName = "Custom/InGameInfo")]
public class InGameInfoSO : ScriptableObject
{
    public int currentCoins;

    public int currentAmmo;

    public int currentScore;

    public int enemiesScore;

    public int currentLeftovers;

    public int currentMeds;

    public int currentScraps;

    public int jumpsScore;

    public int slidesScore;

    public bool maxSpeed;

    public float playerPositionZ;

    public int tutorialCounter;
}
