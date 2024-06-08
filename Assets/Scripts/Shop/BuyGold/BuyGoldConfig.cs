using TMPro;
using UnityEngine;

public class BuyGoldConfig : MonoBehaviour
{
    public int goldAmount;
    public string goldPrice;

    [SerializeField]
    private TextMeshProUGUI goldAmountText;

    [SerializeField]
    private TextMeshProUGUI BuyText;

    private void Start()
    {
        goldAmountText.text = goldAmount.ToString();
        BuyText.text = "Buy for " + goldPrice + "$";
    }

    public void TransferDataToShopMenager()
    {
        var shopMenager = FindObjectOfType<ShopMenager>();

        shopMenager.buyGoldData = new BuyGoldData(goldAmount, goldPrice);
        shopMenager.isBuyGoldButtonClicked = true;
    }
}
