using UnityEngine;

[CreateAssetMenu(fileName = "SpecialOffer", menuName = "Custom/SpecialOffer")]
public class SpecialOfferSO : ScriptableObject
{
    public int index;

    public string offerName;
    public string buyForPriceString;

    public string firstItemName;
    public int firstItemQuantity;
    public Sprite firstItemSprite;

    public string secondItemName;
    public int secondItemQuantity;
    public Sprite secondItemSprite;

    public string thirdItemName; //opcjonalne
    public int thirdItemQuantity;
    public Sprite thirdItemSprite;

    public string fourthItemName; //opcjonalne
    public int fourthItemQuantity;
    public Sprite fourthItemSprite;
}
