using UnityEngine;
using UnityEngine.UI;

public class ShopMenager : MonoBehaviour
{
    [SerializeField]
    private SpecialOfferSO[] specialOffers;

    [SerializeField]
    private BuyGoldSO[] buyGold;

    [SerializeField]
    private BuyItemSO[] buyItem;

    [SerializeField]
    private GameObject specialOffersContainer;

    [SerializeField]
    private GameObject buyGoldContainer;

    [SerializeField]
    private GameObject buyItemContainer;

    [SerializeField]
    private GameObject offerPrefab;

    [SerializeField]
    private GameObject buyGoldPrefab;

    [SerializeField]
    private GameObject buyItemPrefab;

    public bool isOfferButtonClicked = false;
    public bool isBuyGoldButtonClicked = false;
    public bool isBuyItemButtonClicked = false;

    public SpecialOfferButton offerButtonData;
    public BuyGoldData buyGoldData;
    public BuyItemData buyItemData;

    private PlayerData playerData;

    private InfoManager infoManager;

    private void Start()
    {
        SpawnOffersInContainer();
        SpawnBuyGoldInContainer();
        SpawnBuyItemInContainer();
        infoManager = FindObjectOfType<InfoManager>();
        playerData = SaveSystem.LoadPlayerData();
    }

    private void OnEnable()
    {
        playerData = SaveSystem.LoadPlayerData();
    }

    private void Update()
    {

        if (isOfferButtonClicked)
        {
            BuySpecialOffer();
        }

        if (isBuyGoldButtonClicked)
        {
            BuyGold();
        }

        if (isBuyItemButtonClicked)
        {
            BuyItem();
        }

    }

    private void SpawnBuyItemInContainer()
    {
        foreach (var recipe in buyItem)
        {
            GameObject buyItem = Instantiate(buyItemPrefab, buyItemContainer.transform);

            buyItem.GetComponent<BuyItemConfig>().itemName = recipe.itemName;
            buyItem.GetComponent<BuyItemConfig>().cost = recipe.cost;
            buyItem.GetComponent<Image>().sprite = recipe.itemSprite;
        }
    }

    private void BuyItem()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.gold >= buyItemData.cost)
        {
            playerData.gold -= buyItemData.cost;
            AddItemsForPlayer(buyItemData.itemName, 1);
            SaveSystem.SavePlayerData(playerData);
            infoManager.UpdateSaveFile();

            AudioManager.Instance.PlaySFX("Buy");

            // do testów
            Debug.Log("Zakupi³eœ: " + buyItemData.itemName + " | w iloœci: " + 1 + " | za cenê: " + buyItemData.cost + " gold");
        }
        else
        {
            Debug.Log("Nie wystarczaj¹ca iloœæ: gold");
        }

        isBuyItemButtonClicked = false;
    }

    private void SpawnBuyGoldInContainer()
    {
        foreach (var recipe in buyGold)
        {
            GameObject buyGold = Instantiate(buyGoldPrefab, buyGoldContainer.transform);

            buyGold.GetComponent<BuyGoldConfig>().goldAmount = recipe.goldAmount;
            buyGold.GetComponent<BuyGoldConfig>().goldPrice = recipe.goldPrice;
            buyGold.GetComponent<Button>().image.sprite = recipe.goldSprite;

        }
    }

    private void BuyGold()
    {
        // TODO: P³atnoœci i ich sprawdzenie
        bool isPaymentCompleted = true;

        if (isPaymentCompleted)
        {
            playerData.gold += buyGoldData.goldAmount;
            SaveSystem.SavePlayerData(playerData);
            infoManager.UpdateSaveFile();

            AudioManager.Instance.PlaySFX("Buy");
        }
        else
        {
            Debug.Log("Payment failed!");
        }

        isBuyGoldButtonClicked = false;

        Debug.Log("Zakupi³eœ gold w iloœci: " + buyGoldData.goldAmount + " | w cenie: " + buyGoldData.goldPrice + "$");
    }

    private void SpawnOffersInContainer()
    {
        foreach (var recipe in specialOffers)
        {

            GameObject offer = Instantiate(offerPrefab, specialOffersContainer.transform);

            offer.GetComponent<SpecialOfferConfig>().offerName = recipe.offerName;
            offer.GetComponent<SpecialOfferConfig>().price = recipe.buyForPriceString;
            offer.GetComponent<SpecialOfferConfig>().firstItemName = recipe.firstItemName;
            offer.GetComponent<SpecialOfferConfig>().firstItemQuantity = recipe.firstItemQuantity;
            offer.GetComponent<SpecialOfferConfig>().secondItemName = recipe.secondItemName;
            offer.GetComponent<SpecialOfferConfig>().secondItemQuantity = recipe.secondItemQuantity;

            if (recipe.thirdItemName != null)
            {
                offer.GetComponent<SpecialOfferConfig>().thirdItemName = recipe.thirdItemName;
                offer.GetComponent<SpecialOfferConfig>().thirdItemQuantity = recipe.thirdItemQuantity;
            }

            if (recipe.fourthItemName != null)
            {
                offer.GetComponent<SpecialOfferConfig>().fourthItemName = recipe.fourthItemName;
                offer.GetComponent<SpecialOfferConfig>().fourthItemQuantity = recipe.fourthItemQuantity;
            }

            foreach (Transform child in offer.transform)
            {
                if (child.name.Equals("Item1"))
                {
                    child.GetComponent<Image>().sprite = recipe.firstItemSprite;
                }
                else if (child.name.Equals("Item2"))
                {
                    child.GetComponent<Image>().sprite = recipe.secondItemSprite;
                }
                else if (child.name.Equals("Item3"))
                {
                    if (recipe.thirdItemSprite != null)
                    {
                        child.GetComponent<Image>().sprite = recipe.thirdItemSprite;
                    }
                    else
                    {
                        child.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    }
                }
                else if (child.name.Equals("Item4"))
                {
                    if (recipe.fourthItemSprite != null)
                    {
                        child.GetComponent<Image>().sprite = recipe.fourthItemSprite;
                    }
                    else
                    {
                        child.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    }


                }
            }

        }
    }

    private void BuySpecialOffer()
    {
        // TODO: P³atnoœci i ich sprawdzenie
        bool isPaymentCompleted = true;

        if (isPaymentCompleted)
        {
            AddItemsForPlayer(offerButtonData.firstItemName, offerButtonData.firstItemQuantity);
            AddItemsForPlayer(offerButtonData.secondItemName, offerButtonData.secondItemQuantity);
            if (offerButtonData.thirdItemName != null)
            {
                AddItemsForPlayer(offerButtonData.thirdItemName, offerButtonData.thirdItemQuantity);
            }
            if (offerButtonData.fourthItemName != null)
            {
                AddItemsForPlayer(offerButtonData.fourthItemName, offerButtonData.fourthItemQuantity);
            }
            SaveSystem.SavePlayerData(playerData);
            infoManager.UpdateSaveFile();

            AudioManager.Instance.PlaySFX("Buy");
        }
        else
        {
            Debug.Log("Payment failed!");
        }

        isOfferButtonClicked = false;
        // Do testów
        Debug.Log("Otrzyma³eœ: " + offerButtonData.firstItemName + " w iloœci: " + offerButtonData.firstItemQuantity);
        Debug.Log("Otrzyma³eœ: " + offerButtonData.secondItemName + " w iloœci: " + offerButtonData.secondItemQuantity);
        Debug.Log("Otrzyma³eœ: " + offerButtonData.thirdItemName + " w iloœci: " + offerButtonData.thirdItemQuantity);
        Debug.Log("Otrzyma³eœ: " + offerButtonData.fourthItemName + " w iloœci: " + offerButtonData.fourthItemQuantity);
    }

    private void AddItemsForPlayer(string itemName, int itemQuantity)
    {
        switch (itemName)
        {
            case ("Gold"):
                playerData.gold += itemQuantity;
                break;
            case ("Meds"):
                playerData.meds += itemQuantity;
                break;
            case ("FirstAid"):
                playerData.firstAid += itemQuantity;
                break;
            case ("Stimpack"):
                playerData.stimpack += itemQuantity;
                break;
            case ("Leftovers"):
                playerData.leftover += itemQuantity;
                break;
            case ("Meal"):
                playerData.meal += itemQuantity;
                break;
            case ("Drink"):
                playerData.drink += itemQuantity;
                break;
            case ("Scraps"):
                playerData.scraps += itemQuantity;
                break;
            case ("Magnet"):
                playerData.magnet += itemQuantity;
                break;
            case ("Shield"):
                playerData.shield += itemQuantity;
                break;
            case ("Coins"):
                playerData.totalCoins += itemQuantity;
                break;
        }
    }
}
