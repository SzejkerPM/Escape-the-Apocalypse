public class SpecialOfferButton
{
    public string offerName;
    public string price;

    public string firstItemName;
    public int firstItemQuantity;

    public string secondItemName;
    public int secondItemQuantity;

    public string thirdItemName; //opcjonalne
    public int thirdItemQuantity;

    public string fourthItemName; //opcjonalne
    public int fourthItemQuantity;

    public SpecialOfferButton(string offerName, string price, string firstItemName,
        int firstItemQuantity, string secondItemName, int secondItemQuantity,
        string thirdItemName, int thirdItemQuantity, string fourthItemName,
        int fourthItemQuantity)
    {
        this.offerName = offerName;
        this.price = price;
        this.firstItemName = firstItemName;
        this.firstItemQuantity = firstItemQuantity;
        this.secondItemName = secondItemName;
        this.secondItemQuantity = secondItemQuantity;
        this.thirdItemName = thirdItemName;
        this.thirdItemQuantity = thirdItemQuantity;
        this.fourthItemName = fourthItemName;
        this.fourthItemQuantity = fourthItemQuantity;
    }
}
