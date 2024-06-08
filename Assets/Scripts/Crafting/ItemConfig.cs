using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemConfig : MonoBehaviour
{
    public int costItem;
    public string costItemName;
    public int craftedItemAmount;
    public string craftedItemName;

    [SerializeField]
    private Image spriteArrow;
    [SerializeField]
    private Image spriteUsedItem;
    [SerializeField]
    private Image spriteCraftedItem;

    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    public ButtonData buttonData;

    private void Start()
    {
        costText.text = costItem.ToString();
        amountText.text = craftedItemAmount.ToString();
        buttonData = SetButtonData();
    }

    private ButtonData SetButtonData()
    {
        return new ButtonData(costItem, costItemName, craftedItemAmount, craftedItemName);
    }

    public void TransferButtonDataToMenager()
    {
        var craftingMenager = FindObjectOfType<CraftingMenager>();

        craftingMenager.buttonData = buttonData;
        craftingMenager.isButtonClicked = true;
    }

}
