using UnityEngine;

[CreateAssetMenu(fileName = "BuyItem", menuName = "Custom/BuyItem")]
public class BuyItemSO : ScriptableObject
{

    public int index;

    public string itemName;
    public int cost;
    public Sprite itemSprite;

}
