using TMPro;
using UnityEngine;

public class SpecialOfferConfig : MonoBehaviour
{
    public string offerName;
    public string price;

    public string firstItemName;
    public int firstItemQuantity;

    [SerializeField]
    private Sprite firstItemSprite;

    public string secondItemName;
    public int secondItemQuantity;

    [SerializeField]
    private Sprite secondItemSprite;

    public string thirdItemName;
    public int thirdItemQuantity;

    [SerializeField]
    private Sprite thirdItemSprite;

    public string fourthItemName;
    public int fourthItemQuantity;

    [SerializeField]
    private Sprite fourthItemSprite;


    [SerializeField]
    private TextMeshProUGUI offerNameText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI firstItemQuanitityText;

    [SerializeField]
    private TextMeshProUGUI secondItemQuanitityText;

    [SerializeField]
    private TextMeshProUGUI thirdItemQuanitityText;

    [SerializeField]
    private TextMeshProUGUI fourthItemQuanitityText;

    public SpecialOfferButton buttonData;

    private void Start()
    {
        offerNameText.text = offerName;
        priceText.text = price;
        firstItemQuanitityText.text = firstItemQuantity.ToString();
        secondItemQuanitityText.text = secondItemQuantity.ToString();
        if (thirdItemQuantity > 0)
        {
            thirdItemQuanitityText.text = thirdItemQuantity.ToString();
        }
        if (fourthItemQuantity > 0)
        {
            fourthItemQuanitityText.text = fourthItemQuantity.ToString();
        }

        buttonData = SetButtonData();
    }

    private SpecialOfferButton SetButtonData()
    {
        return new SpecialOfferButton(offerName, price, firstItemName,
         firstItemQuantity, secondItemName, secondItemQuantity,
         thirdItemName, thirdItemQuantity, fourthItemName,
         fourthItemQuantity);
    }

    public void TransferButtonDataToMenager()
    {
        var shopMenager = FindObjectOfType<ShopMenager>();

        shopMenager.offerButtonData = buttonData;
        shopMenager.isOfferButtonClicked = true;
    }
}
