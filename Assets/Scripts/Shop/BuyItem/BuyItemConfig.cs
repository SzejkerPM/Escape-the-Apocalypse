using TMPro;
using UnityEngine;

public class BuyItemConfig : MonoBehaviour
{
    public string itemName;
    public int cost;

    [SerializeField]
    private TextMeshProUGUI costText;

    private void Start()
    {
        costText.text = "Buy for " + cost + " gold";
    }

    public void TransferDataToShopMenager()
    {
        var shopMenager = FindObjectOfType<ShopMenager>();

        shopMenager.buyItemData = new BuyItemData(itemName, cost);
        shopMenager.isBuyItemButtonClicked = true;
    }
}
