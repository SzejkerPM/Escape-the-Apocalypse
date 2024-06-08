using UnityEngine;

[CreateAssetMenu(fileName = "BuyGold", menuName = "Custom/BuyGold")]
public class BuyGoldSO : ScriptableObject
{

    public int index;

    public Sprite goldSprite;
    public int goldAmount;
    public string goldPrice;

}
