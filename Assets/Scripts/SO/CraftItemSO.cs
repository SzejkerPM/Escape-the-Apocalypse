using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Custom/CraftItem")]
public class CraftItemSO : ScriptableObject
{
    public int index;

    public int costItem;
    public string costItemName;
    public int craftedItemAmount;
    public string craftedItemName;

    public Sprite spriteArrow;
    public Sprite spriteUsedItem;
    public Sprite spriteCraftedItem;

}
