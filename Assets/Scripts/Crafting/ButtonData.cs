public class ButtonData
{
    public int costItem;
    public string costItemName;

    public int craftedItemAmount;
    public string craftedItemName;

    public ButtonData(int costItem, string costItemName, int craftedItemAmount, string craftedItemName)
    {
        this.costItem = costItem;
        this.costItemName = costItemName;
        this.craftedItemAmount = craftedItemAmount;
        this.craftedItemName = craftedItemName;
    }
}
